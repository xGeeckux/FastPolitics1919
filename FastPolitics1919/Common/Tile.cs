using FastPolitics1919.Data.Common;
using FastPolitics1919.Data.Common.MapObjects;
using FastPolitics1919.History.Buildings;
using FastPolitics1919.History.Governments;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Common
{
    public abstract class Tile : PathFinderTile
    {
        public override bool IsProcessable => true;
        public override int BaseProcessRound => 4 * 10;
        public override int ProcessRound
        {
            get
            {
                ModifierItem item = GetModifier(Data.Common.Modifiers.LocalCityFoundModifier);

                return (int)(BaseProcessRound * (1 + item.Value));
            }
        }
        public bool Upgrading { get; set; }

        #region Modifiers
        protected override void LoadEveryModifier()
        {
            base.LoadEveryModifier();
            //- Persons can carry Modifiers
            foreach (Person person in Persons)
                Append(person);

            //- Modifier for Neighbour Cities
            //-     Neighbour is City
            Modifier neighbour_build_modifier = new Modifier("Grenzt an eine Stadt");
            neighbour_build_modifier.Items.Add(new ModifierItem(Data.Common.Modifiers.LocalCityFoundModifier, 1));
            foreach (Tile tile in GetNeighbours())
                if (tile is City)
                {
                    OtherModifiers.Add(neighbour_build_modifier);
                    break;
                }
            //-     Big Population
            Modifier pop_build_modifier = new Modifier("Bevölkerungsgröße");
            double mod = CitizenCount * 0.02;
            if (mod > 0.8)
                mod = 0.8;
            pop_build_modifier.Items.Add(new ModifierItem(Data.Common.Modifiers.LocalCityFoundModifier, -mod));
            OtherModifiers.Add(pop_build_modifier);
        }
        #endregion

        #region Owner
        public Province Owner { get; set; }
        public Country CountryOwner
        {
            get
            {
                if (Owner != null && Owner.Owner != null)
                    return Owner.Owner;
                return null;
            }
        }
        public GameObject Controller { get; set; }
        public Country CountryController => Controller != null && Controller is Country country ? country : null;
        //- For City Founding
        public GameObject Founder { get; set; }
        #endregion

        #region Area
        public double Area => (3 / 2 * Math.Pow(2, 2) * Math.Sqrt(3));
        public double Dichte => CitizenAmount / Area;
        public abstract BitmapImage Icon { get; }
        public abstract BitmapImage BackgroundImage { get; }
        public BitmapImage BackGroundImage { get; set; }
        public Culture Culture { get; set; }
        #endregion

        #region Population
        public int CitizenCount => Citizens.Count;
        public int CitizenAmount => CitizenCount * Citizen.AmountPerCitizen + Persons.Count;
        public List<Citizen> Citizens
        {
            get
            {
                List<Citizen> local = new List<Citizen>();
                foreach (Citizen citizen in Engine.Game.Citizens.Get())
                    if (!(citizen is Person) && citizen.LocationID == ID)
                        local.Add(citizen);
                return local;
            }
        }
        public List<Person> Persons
        {
            get
            {
                List<Person> local = new List<Person>();
                foreach (Citizen citizen in Engine.Game.Citizens.Get())
                    if (citizen is Person person && person.LocationID == ID)
                        local.Add(person);
                return local;
            }
        }
        private static Random Random = new Random();
        public virtual void CreateCitizen()
        {
            Citizen local = new Citizen();

            Citizen.Index++;
            local.ID = Citizen.Index;
            local.Name = Engine.Game.GetRandomName();
            local.LocationID = ID;
            local.OriginID = ID;
            local.Culture = Engine.Game.FindCulture(History.Cultures.Cultures.Deutsch);
            int n = Random.Next(0, Engine.Game.Ideologies.Count);
            local.Ideology = Engine.Game.Ideologies.Get()[n];

            Engine.Game.Citizens.Add(local, local.ID);
        }
        public virtual void CreatePeasant()
        {
            Citizen local = new Citizen();

            Citizen.Index++;
            local.ID = Citizen.Index;
            local.Name = Engine.Game.GetRandomName();
            local.LocationID = ID;
            local.OriginID = ID;
            local.Culture = Engine.Game.FindCulture(History.Cultures.Cultures.Deutsch);
            int n = Random.Next(0, Engine.Game.Ideologies.Count);
            local.Ideology = Engine.Game.Ideologies.Get()[n];

            Engine.Game.Citizens.Add(local, local.ID);
        }
        #endregion

        #region Buildings
        public List<Building> Buildings = new List<Building>();
        public GovernmentBuilding GovernmentBuilding { get; set; }
        public MilitaryBuilding[] MilitaryBuildings = new MilitaryBuilding[3];
        public UniqueBuilding[] UniqueBuildings = new UniqueBuilding[4];
        public int BuildingCount => Buildings.Count + MilitaryBuildingCount + UniqueBuildingCount + (GovernmentBuilding != null ? 1 : 0);
        public int MilitaryBuildingCount
        {
            get
            {
                int count = 0;
                foreach (MilitaryBuilding buidling in MilitaryBuildings)
                    if (buidling != null)
                        count++;
                return count;
            }
        }
        public int UniqueBuildingCount
        {
            get
            {
                int count = 0;
                foreach (UniqueBuilding buidling in UniqueBuildings)
                    if (buidling != null)
                        count++;
                return count;
            }
        }
        public int BuildingProcesses { get; set; }
        public void AddBuilding(Building building)
        {

        }
        public void SetBuilding(Building building)
        {
            if (building is GovernmentBuilding government)
            {
                GovernmentBuilding = government;
            }
            else if (building is MilitaryBuilding military)
            {
                if (MilitaryBuildingCount != MilitaryBuildings.Length)
                    for (int i = 0; i < MilitaryBuildings.Length; i++)
                        if(MilitaryBuildings[i] == null)
                        {
                            MilitaryBuildings[i] = military;
                            break;
                        }
            }
            else if (building is UniqueBuilding unique)
            {
                if (UniqueBuildingCount != UniqueBuildings.Length)
                    for (int i = 0; i < UniqueBuildings.Length; i++)
                    {
                        if (UniqueBuildings[i] != null && unique != null && UniqueBuildings[i].GetType() == unique.Ancestor)
                        {
                            Engine.Game.Buildings.Remove(UniqueBuildings[i].ID);
                            UniqueBuildings[i] = null;
                            UniqueBuildings[i] = unique;
                            break;
                        }
                        if (UniqueBuildings[i] == null)
                        {
                            UniqueBuildings[i] = unique;
                            break;
                        }
                    }
            }
            else
            {
                Buildings.Add(building);
            }
            building.LocationID = ID;
        }
        #endregion

        #region Ideology
        public Ideology.Amount[] Ideologies
        {
            get
            {
                Ideology.Amount[] local = new Ideology.Amount[Engine.Game.Ideologies.Count];
                Ideology[] list = Engine.Game.Ideologies.Get();
                for (int i = 0; i < Engine.Game.Ideologies.Count; i++)
                    local[i] = new Ideology.Amount(list[i], 0);
                foreach (Citizen citizen in Citizens)
                    local[citizen.Ideology.ID].Proportion += 1;
                return local;
            }
        }
        #endregion

        #region Units
        public List<Unit> Units
        {
            get
            {
                List<Unit> list = new List<Unit>();

                foreach (Unit unit in Engine.Game.Units.Get())
                    if (unit.LocationID == ID && !unit.IsLinked)
                        list.Add(unit);

                return list;
            }
        }
        public void UpdateUnits()
        {
            foreach (Unit unit in Units)
                unit.Update();
        }
        #endregion

        #region Battle
        public bool HasBattle => Battles.Count > 0;
        public List<Battle> Battles = new List<Battle>();
        public void CheckForBattles()
        {
            if (Battles.Count != 0)
                return;
            List<Unit> hostiles = new List<Unit>();
            List<Unit> friendly = new List<Unit>();
            foreach (Unit unit in Units)
            {
                if (unit.IsHostile)
                    hostiles.Add(unit);
                else
                    friendly.Add(unit);
            }

            if (hostiles.Count > 0 && friendly.Count > 0)
            {
                Battles.Add(new Battle(hostiles, friendly));
            }
        }
        public void UpdateBattles()
        {
            List<Battle> new_battles = new List<Battle>();
            foreach (Battle bt in Battles)
                if (!bt.Update())
                    new_battles.Add(bt);
            Battles = new_battles;
        }
        #endregion

        #region Updates
        public void Update()
        {
            CheckForBattles();
            UpdateBattles();
            UpdateUnits();
        }
        #endregion

        #region Map
        public List<Tile> GetNeighbours()
        {
            List<Tile> tiles = new List<Tile>();
            foreach (HexagonTile tile in Hex.GetNeighbours())
            {
                tiles.Add(tile.Tile);
            }
            return tiles;
        }
        public void SetController(GameObject obj)
        {
            Controller = obj;
        }
        #endregion

        public static City FromTile(Tile tile)
        {
            City local = new City();
            Engine.CopyProperties(tile, local);
            return local;
        }
    }
}
