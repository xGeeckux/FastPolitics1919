using FastPolitics1919.Common;
using FastPolitics1919.Data.Common;
using FastPolitics1919.Data.Common.BuildProcesses;
using FastPolitics1919.Data.GoogleTabellen;
using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Data.Tmp;
using FastPolitics1919.Events;
using FastPolitics1919.History.Actions;
using FastPolitics1919.History.Cultures;
using FastPolitics1919.History.Events;
using FastPolitics1919.History.Ideologies;
using LibFastPolitics1919.AVL;
using LibFastPolitics1919.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FastPolitics1919
{
    [Serializable]
    public sealed class Game
    {
        public Round Current { get; set; }

        //- Processes
        public SortList<BuildProcess> EveryProcess = new SortList<BuildProcess>();

        //- Action
        public List<Type> EveryAction { get; set; }

        //- MapModes
        public SortList<MapMode> EveryMapMode = new SortList<MapMode>();

        //- Events
        public List<Event> EventTypes { get; set; }
        public List<Event> EventBlackList { get; set; }

        #region Database
        public SortList<Ideology> Ideologies = new SortList<Ideology>();
        public Ideology FindIdeology(Ideologies ideology) => Ideologies.Find((int)ideology);
        public Ideology FindIdeology(int ideology) => Ideologies.Find(ideology);

        public SortList<Country> Countries = new SortList<Country>();
        public Country FindCountry(int id) => Countries.Find(id);

        public SortList<Province> Provinces = new SortList<Province>();
        public Province FindProvince(int id) => Provinces.Find(id);

        public SortList<Tile> Tiles = new SortList<Tile>();
        public Tile FindTile(int id) => Tiles.Find(id);
        public City FindCity(int id) => (City)Tiles.Find(id);
        public Landscape FindLandscape(int id) => (Landscape)Tiles.Find(id);

        public SortList<Citizen> Citizens = new SortList<Citizen>();
        public Citizen FindCitizen(int id) => Citizens.Find(id);

        public SortList<Building> Buildings = new SortList<Building>();
        public Building FindBuilding(int id) => Buildings.Find(id);

        public SortList<Culture> Cultures = new SortList<Culture>();
        public Culture FindCulture(Cultures culture) => Cultures.Find((int)culture);
        public Culture FindCulture(int culture) => Cultures.Find(culture);

        public SortList<Party> Parties = new SortList<Party>();
        public Party FindParty(int id) => Parties.Find(id);

        public SortList<Unit> Units = new SortList<Unit>();
        public Unit FindUnit(int id) => Units.Find(id);

        public SortList<Army> Armys = new SortList<Army>();
        public Army FindArmy(int id) => Armys.Find(id);

        public SortList<Bank> Banks = new SortList<Bank>();
        public Bank FindBank(int id) => Banks.Find(id);
        #endregion

        public List<string> Vornamen = new List<string>();
        public List<string> Nachnamen = new List<string>();
        public static Random Random = new Random();
        public string GetRandomName()
        {
            string full = "";

            int n1 = Random.Next(Vornamen.Count);
            if (Vornamen.Count > 0)
                full += Vornamen[n1];
            int n2 = Random.Next(Nachnamen.Count);
            if (Nachnamen.Count > 0)
                full += Nachnamen[n2];
            if (full == "")
                return "Bürger";
            return full;
        }

        #region Serzi
        public void Serialize()
        {
            string path = Environment.CurrentDirectory + @"\game.fp";
            FileStream stream = new FileStream(path, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, this);
            stream.Close();
        }
        public Game Deserialize()
        {
            string path = Environment.CurrentDirectory + @"\game.fp";
            Game game;
            FileStream stream = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            game = (Game)bf.Deserialize(stream);
            stream.Close();
            return game;
        }
        #endregion

        #region Load
        public void Load()
        {
            Modifier.LoadModifiers();
            //Engine.Game = Deserialize();
            LoadTypes();
            LoadNames();
            LoadIdeologies();
            ContentGenerator.Run();
            LoadParties();
            LoadPlayers();
            LoadPersons();
        }
        private void LoadTypes()
        {
            LoadActions();
            LoadMapModes();
            LoadEvents();
        }
        private void LoadActions()
        {
            string nspace1 = "FastPolitics1919.History.Actions";
            var q1 = from t in Assembly.GetExecutingAssembly().GetTypes()
                     where t.IsClass && t.Namespace == nspace1
                     select t;
            EveryAction = q1.ToList();
        }
        private void LoadMapModes()
        {
            string nspace2 = "FastPolitics1919.History.MapModes";
            var q2 = from t in Assembly.GetExecutingAssembly().GetTypes()
                     where t.IsClass && t.Namespace == nspace2
                     select t;

            List<Type> types = q2.ToList();
            foreach (Type type in types)
            {
                MapMode mode = Engine.CreateClass<MapMode>(type);
                EveryMapMode.Add(mode, mode.ID);
            }
        }
        private void LoadEvents()
        {
            string nspace1 = "FastPolitics1919.History.Events";
            var q1 = from t in Assembly.GetExecutingAssembly().GetTypes()
                     where t.IsClass && t.Namespace == nspace1
                     select t;
            List<Type> types = q1.ToList();

            EventTypes = new List<Event>();
            foreach (Type type in types)
            {
                Event local = Engine.CreateClass<Event>(type);
                if (!local.IsTriggeredOnly)
                    EventTypes.Add(local);
            }

            //- Sollte von einer Datenbank gezogen werden
            EventBlackList = new List<Event>();
        }
        public void UpdateStatic()
        {
            Citizen[] list = Citizens.Get();
            for (int i = 0; i < Citizens.Length; i++)
            {
                if (list[i].ID > Citizen.Index)
                    Citizen.Index = list[i].ID;
            }
        }
        
        public void AfterMapLoad()
        {
            City city = FindCity(158);
            city.SetBuilding(new History.Buildings.Bürgerbräukeller());
            city.SetBuilding(new History.Buildings.ZirkusKrone());
            city.SetBuilding(new History.Buildings.Bank(FindBank(1)));
            city.SetBuilding(new History.Buildings.SimpleGovernmentBuilding());
            city.Government = new History.Governments.CityGovernment(city);
            city.Government.MaxSeats = 40;
            ((Person)Engine.Game.FindCitizen(10)).FoundParty("Deutsche Arbeiter Partei", "DAP", Engine.Game.FindIdeology(1));
            ((Person)Engine.Game.FindCitizen(10)).Party.Color = "100-50-0";
            city.Government.Election();

            city = FindCity(118);
            city.SetBuilding(new History.Buildings.Bank(FindBank(1)));

            city = FindCity(102);
            city.SetBuilding(new History.Buildings.Bank(FindBank(1)));

            Map.TileGenerator.FinishLoading();
            Engine.GameInterface.Update();
        }

        //- Loads
        private GoogleSheet Sheet = new GoogleSheet();
        private void LoadPlayers()
        {
            Sheet.SetSheetUrl("1ZjzbXTa93K7G3zTYgRdyk7jA7aS0i4Mo_myblGjOE18");
            Sheet.SetSheetTab("Players");
            List<GoogleCell[]> cells = Sheet.ReadCells("A2:C");

            foreach (GoogleCell[] cell in cells)
            {
                if (cell.Length < 3 || cell[0] == null || cell[1] == null)
                    continue;
                Player player = new Player();
                player.ID = Convert.ToInt32(cell[0].Content.ToString());
                player.Name = cell[1].Content.ToString();
                player.OriginID = Convert.ToInt32(cell[2].Content.ToString());
                player.LocationID = player.OriginID;
                Engine.Game.Citizens.Add(player, player.ID);
            }
        }
        private void LoadPersons()
        {
            Sheet.SetSheetTab("Persons");
            List<GoogleCell[]> cells = Sheet.ReadCells("A2:G");

            foreach (GoogleCell[] cell in cells)
            {
                if (cell.Length < 3 || cell[0] == null || cell[1] == null)
                    continue;
                Person person = new Person();
                person.ID = Convert.ToInt32(cell[0].Content.ToString());
                person.Name = cell[1].Content.ToString();
                person.OriginID = Convert.ToInt32(cell[2].Content.ToString());
                person.LocationID = person.OriginID;
                if (cell.Length > 3)
                {
                    person.ImageName = cell[3].Content.ToString();
                }
                if (cell.Length > 4)
                {
                    person.Ideology = FindIdeology(Convert.ToInt32(cell[4].Content.ToString()));
                }
                if (cell.Length > 5)
                {
                    if (cell[5] != null && cell[5].Content.ToString() != "-1")
                        person.JoinParty(FindParty(Convert.ToInt32(cell[5].Content.ToString())));
                }
                if (cell.Length > 6)
                {
                    person.Culture = FindCulture(Convert.ToInt32(cell[6].Content.ToString()));
                }
                Engine.Game.Citizens.Add(person, person.ID);
            }

            int tmp = 0;
            foreach (Citizen citizen in Citizens.Get())
                if (citizen.ID > tmp)
                    tmp = citizen.ID;
            Citizen.Index = tmp;
        }
        private void LoadNames()
        {
            Sheet.SetSheetUrl("1ZjzbXTa93K7G3zTYgRdyk7jA7aS0i4Mo_myblGjOE18");
            Sheet.SetSheetTab("Names");
            List<GoogleCell[]> cells = Sheet.ReadCells("A2:B");

            foreach (GoogleCell[] cell in cells)
            {
                if (cell.Length == 0)
                    continue;
                if (cell.Length == 1 && cell[0] != null)
                    Vornamen.Add(cell[0].Content.ToString());
                if (cell.Length == 2 && cell[1] != null)
                    Nachnamen.Add(cell[1].Content.ToString());
            }
        }
        private void LoadIdeologies()
        {
            string nspace = "FastPolitics1919.History.Ideologies";
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == nspace
                    select t;
            List<Type> ideology_types = q.ToList();

            foreach (Type type in ideology_types)
            {
                Ideology ideology = Engine.CreateClass<Ideology>(type);
                Ideologies.Add(ideology, ideology.ID);
            }
        }
        private void LoadParties()
        {
            int tmp = 0;
            foreach (Party party in Parties.Get())
                if (party.ID > tmp)
                    tmp = party.ID;
            Party.Index = tmp;
        }
        #endregion

        #region Update
        public void Update()
        {
            Current.Number++;

            //- Battles
            Log.Write("Checking Battles..");
            foreach (Tile tile in Tiles.Get())
            {
                tile.Update();
            }

            //- Processes
            BuildProcess[] now_finished = EveryProcess.FindEveryUnefficient(Current.Number);
            if (now_finished.Length != 0)
            {
                Log.Write("Dealing with Finished Processes..");
                foreach (BuildProcess process in now_finished)
                {
                    process.Done();
                }
            }
            EveryProcess = EveryProcess.RemoveEveryUnefficient(Current.Number);

            //- Events
            foreach (Event event_type in EventTypes)
            {
                if (event_type is PlayerEvent player_event)
                    foreach (Citizen person in Citizens.Get())
                        if (person is Player player)
                        {
                            PlayerEvent local = Engine.CreateClass<PlayerEvent>(player_event.GetType());
                            local.Init(player);
                            if (local.Condition())
                            {
                                local.Fire();
                                Log.Write("Fire");
                            }
                        }
            }

            Unit.HasMoved.Clear();
            Engine.Map.ChangeMapMode(Engine.Map.CurrentMapMode);
            Engine.UpdateEverySubWindow();
        }
        #endregion

        //private GoogleSheet Sheet = new GoogleSheet();
        //private void LoadCountries()
        //{
        //    GoogleSheet Sheet = new GoogleSheet();
        //    Sheet.SetSheetUrl("1ZjzbXTa93K7G3zTYgRdyk7jA7aS0i4Mo_myblGjOE18");
        //    Countries = new SortList<Country>();
        //    Sheet.SetSheetTab("Countries");
        //    List<GoogleCell[]> cells = Sheet.ReadCells("A2:C");
        //    if (cells.Count == 0)
        //        return;
        //    foreach (GoogleCell[] row in cells)
        //    {
        //        Country local = new Country
        //        {
        //            ID = Convert.ToInt32(row[0].Content),
        //            Name = row[1].Content.ToString(),
        //            RGBColor = row[2].Content.ToString()
        //        };
        //        Countries.Add(local, local.ID);
        //    }
        //    Log.Write("Countries loaded!");
        //}
        //private void LoadProvinces()
        //{
        //    GoogleSheet Sheet = new GoogleSheet();
        //    Sheet.SetSheetUrl("1ZjzbXTa93K7G3zTYgRdyk7jA7aS0i4Mo_myblGjOE18");
        //    Provinces = new SortList<Province>();
        //    Sheet.SetSheetTab("Provinces");
        //    List<GoogleCell[]> cells = Sheet.ReadCells("A2:C");
        //    if (cells.Count == 0)
        //        return;
        //    foreach (GoogleCell[] row in cells)
        //    {
        //        Province local = new Province
        //        {
        //            ID = Convert.ToInt32(row[0].Content),
        //            Name = row[1].Content.ToString(),
        //            Owner = Countries.Find(Convert.ToInt32(row[2].Content))
        //        };
        //        Provinces.Add(local, local.ID);
        //    }
        //    Log.Write("Provinces loaded!");
        //}
        //private void LoadCities()
        //{
        //    GoogleSheet Sheet = new GoogleSheet();
        //    Sheet.SetSheetUrl("1ZjzbXTa93K7G3zTYgRdyk7jA7aS0i4Mo_myblGjOE18");
        //    Cities = new SortList<City>();
        //    Sheet.SetSheetTab("Cities");
        //    List<GoogleCell[]> cells = Sheet.ReadCells("A2:C");
        //    if (cells.Count == 0)
        //        return;
        //    foreach (GoogleCell[] row in cells)
        //    {
        //        City local = new City
        //        {
        //            ID = Convert.ToInt32(row[0].Content),
        //            Name = row[1].Content.ToString(),
        //        };
        //        if (row.Length >= 3)
        //        {
        //            string[] citizens = row[2].Content.ToString().Split(',');
        //            for (int i = 0; i < citizens.Length; i++)
        //            {
        //                int id = Convert.ToInt32(citizens[i]);
        //                Citizen c = FindCitizen(id);
        //                if (c == null)
        //                    continue;
        //                local.Citizens.Add(c);
        //            }
        //        }
        //        Cities.Add(local, local.ID);
        //    }
        //    Log.Write(Cities.Length + " Cities loaded!");
        //}
        //private void LoadCitizens()
        //{
        //    GoogleSheet Sheet = new GoogleSheet();
        //    Sheet.SetSheetUrl("1ZjzbXTa93K7G3zTYgRdyk7jA7aS0i4Mo_myblGjOE18");
        //    Citizens = new SortList<Citizen>();
        //    Sheet.SetSheetTab("Citizens");
        //    List<GoogleCell[]> cells = Sheet.ReadCells("A2:B");
        //    if (cells.Count == 0)
        //        return;
        //    foreach (GoogleCell[] row in cells)
        //    {
        //        Citizen local = new Citizen
        //        {
        //            ID = Convert.ToInt32(row[0].Content),
        //            Name = row[1].Content.ToString(),
        //        };
        //        if (Citizen.Index < local.ID)
        //            Citizen.Index = local.ID;
        //        Citizens.Add(local, local.ID);
        //    }
        //    Log.Write("Citizens loaded!");
        //}

        //#endregion

        //#region Upload
        //public void Upload()
        //{

        //}
        //#endregion
    }
}
