using FastPolitics1919.Common;
using FastPolitics1919.Data.Client;
using FastPolitics1919.Data.Common;
using FastPolitics1919.Data.Common.BuildProcesses;
using FastPolitics1919.Data.Common.MapObjects;
using FastPolitics1919.Data.Managers;
using FastPolitics1919.Events;
using FastPolitics1919.History.Titles;
using FastPolitics1919.History.Units;
using FastPolitics1919.History.Units.Attachments;
using FastPolitics1919.History.Units.Companies;
using FastPolitics1919.History.Units.Hqs;
using FastPolitics1919.Interface;
using FastPolitics1919.Interface.Game;
using LibFastPolitics1919.AVL;
using LibFastPolitics1919.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FastPolitics1919
{
    public static class Engine
    {
        public static Log Log { get; set; }
        public static Game Game { get; set; }
        public static Map Map { get; set; }

        //- Current
        public static Person CurrentPerson { get; set; }
        public static bool IsPlayer => CurrentPerson is Player;

        #region Managers
        public static WindowManager WindowManager { get; set; }
        #endregion
        #region Interfaces
        public static GameInterface GameInterface { get; set; }
        //- SubWindows
        public static List<SubWindow> ActiveSubWindows = new List<SubWindow>();
        public static void Open(SubWindow window)
        {
            GameInterface.Window.gui_grid.Children.Add(window);
            ActiveSubWindows.Add(window);
        }
        public static void Close(SubWindow window)
        {
            ((Grid)window.Parent).Children.Remove(window);
            ActiveSubWindows.Remove(window);
        }
        public static void UpdateEverySubWindow()
        {
            foreach (SubWindow window in ActiveSubWindows)
                window.Update();

            //- Update Unit Window (if Open) sepretly
            if (Map.CurrentSelected != null && Map.CurrentSelected is Unit unit)
                OpenUnit(unit);
        }
        //- Bsp Windows
        public static void OpenTile(Tile tile)
        {
            Open(new CitySubWindow(tile));
        }
        public static void OpenProvince(Province province)
        {
            Open(new ProvinceSubWindow(province));
        }
        public static void OpenCountry(Country country)
        {
            MessageBox.Show(country.Name);
        }
        public static void OpenPerson(Person person)
        {
            Open(new PersonSubWindow(person));
        }
        public static void OpenUnit(Unit unit)
        {
            GameInterface.OpenUnit(unit);
        }
        public static void OpenEvent(Event @event)
        {
            @event.Open();
            Open(@event.Window);
        }
        #endregion

        /// <summary>
        /// Inizialisiert das Program an dieser Stelle.
        /// </summary>
        /// <param name="args">Übergibt die dem Program enthaltenen Debug-Argumente</param>
        [STAThread]
        public static void Init(string[] args)
        {
            //- Load Log
            Log = new Log();
            Log.Init();
            //- Load WindowManager
            WindowManager = new WindowManager();
            //ConsoleHandler.Start();

            //- Debug
            Debug();
        }

        private static void Debug()
        {
            ConsoleHandler.Start();
            Game = new Game();
            Game.Load();
            Game.Current = new Round(1);
            Game.UpdateStatic();

            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
            GameInterface = new GameInterface();
            ((Interface.GameWindow)GameInterface.Window).grid.Children.Add(Map = Map.TileGenerator.CreateTiles());

            #region Debug
            //- Player
            CurrentPerson = (Person)Game.FindCitizen(0);
            CurrentPerson.ImageName = "person_geecku";
            CurrentPerson.Culture = Game.FindCulture(History.Cultures.Cultures.Deutsch);
            CurrentPerson.Ideology = Game.FindIdeology(1);
            CurrentPerson.Party = CurrentPerson.Location.CountryOwner.Government.RegisteredParties[0];

            //- Banks
            Bank test_bank = new Bank("Bank von Bayern");
            test_bank.GoldQuanity = 7;
            Game.Banks.Add(test_bank, test_bank.ID);

            //- Provinces
            Game.FindProvince(1).SetOwner(Game.FindCountry(1));
            Game.FindProvince(2).SetOwner(Game.FindCountry(1));
            Game.FindProvince(5).SetOwner(Game.FindCountry(1));

            //- Eckard
            Person person = (Person)Game.FindCitizen(10);
            person.Ideology = Game.FindIdeology(1);
            person.Culture = Game.FindCulture(History.Cultures.Cultures.Deutsch);

            CurrentPerson.TalkTo(person);
            CurrentPerson.ChangeFollower(person);

            //- Göring
            person = (Person)Game.FindCitizen(11);
            person.JoinArmy(Game.FindArmy(1));
            person.Army.PromotePerson(person, 8);

            //- von Kahr
            person = (Person)Game.FindCitizen(12);
            person.Party.DefineNewLeader(person);

            //- von Lossow
            person = (Person)Game.FindCitizen(13);
            person.JoinArmy(Game.FindArmy(1));
            person.Army.PromotePerson(person, 2);
            person.Army.DefineNewLeader(person);

            //- Röhm
            person = (Person)Game.FindCitizen(14);
            person.JoinArmy(Game.FindArmy(1));
            person.Army.PromotePerson(person, 9);

            //- Hitler
            person = (Person)Game.FindCitizen(16);
            person.JoinArmy(Engine.Game.FindArmy(1));
            person.Army.PromotePerson(person, 19);

            //- Mayer
            person = (Person)Game.FindCitizen(15);
            person.JoinArmy(Engine.Game.FindArmy(1));
            person.Army.PromotePerson(person, 8);
            #endregion

            #region Historical OOB
            //- 'Infanterieführer VII (Reichswehr)' (17. Infanterie-Division)
            Person franz_epp = (Person)Game.FindCitizen(17);
            franz_epp.JoinArmy(Game.FindArmy(1));
            franz_epp.Army.PromotePerson(franz_epp, 3);

            Division inf_vii = new Division(franz_epp.LocationID);
            Game.Units.Add(inf_vii, inf_vii.ID);
            inf_vii.Name = "Infanterieführer VII. (Reichswehr)";
            inf_vii.DefineNewCommander(franz_epp);
            inf_vii.ClearLocalUnits();
            inf_vii.Owner = Game.FindCountry(0);
            inf_vii.HQ.Name = "Divisions Kommando VII. der Reichswehr";

            #region Division LocalUnits
            RegularCavalry cav2 = new RegularCavalry(inf_vii);
            cav2.Name = "Divisions-Kavallerie 302";
            cav2.SetUnitType(UnitTypes.Platoon);
            inf_vii.AddLocalUnit(cav2);

            RegularInfantry inf = new RegularInfantry(inf_vii);
            inf.Name = "Guard Infanterie 'Landwehr'";
            inf.SetUnitType(UnitTypes.Group);
            inf_vii.AddLocalUnit(inf);

            RegularCavalry cav1 = new RegularCavalry(inf_vii);
            cav1.Name = "Kavalleriestand Reichswehr";
            cav1.SetUnitType(UnitTypes.Team);
            inf_vii.AddLocalUnit(cav1);
            #endregion

            #region Regiments
            //- 19. Infanterie-Regiment "Friedrich von Haack"
            Person p1 = (Person)Game.FindCitizen(18);
            p1.JoinArmy(Game.FindArmy(1));
            p1.Army.PromotePerson(p1, 5);
            Regiment reg1 = Regiment.CreateDefault(p1.LocationID);
            inf_vii.AddSubUnit(reg1);
            reg1.Name = "19. Infanterie-Regiment";
            reg1.Owner = Game.FindCountry(0);
            reg1.DefineNewCommander(p1);
            reg1.Parent = inf_vii;

            //- 20. Infanterie-Regiment "Ludwig Leupold"
            Person p2 = (Person)Game.FindCitizen(19);
            p2.JoinArmy(Game.FindArmy(1));
            p2.Army.PromotePerson(p2, 5);
            Regiment reg2 = new Regiment(p2.LocationID);
            inf_vii.AddSubUnit(reg2);
            reg2.Name = "20. Infanterie-Regiment";
            reg2.Owner = Game.FindCountry(0);
            reg2.DefineNewCommander(p2);
            reg2.Parent = inf_vii;
            Game.Units.Add(reg2, reg2.ID);
            #region 20. Inf-Regiment
            Person c = (Person)Game.FindCitizen(14);
            Bataillon b1 = Bataillon.CreateDefault(p2.LocationID);
            b1.Name = "1. Bataillon 'Rheinmayer-Köln'";
            b1.Owner = Game.FindCountry(0);
            b1.Parent = reg2;
            b1.DefineNewCommander(c);
            reg2.AddSubUnit(b1);
            reg2.LinkUp(b1);
            
            Bataillon b2 = Bataillon.CreateDefault(114);
            b2.Name = "2. Bataillon 'Pait'";
            b2.Owner = Game.FindCountry(0);
            b2.Parent = reg2;
            reg2.AddSubUnit(b2);

            Bataillon b3 = Bataillon.CreateDefault(151);
            b3.Name = "3. Bataillon 'Kaiser'";
            b3.Owner = Game.FindCountry(0);
            b3.Parent = reg2;
            reg2.AddSubUnit(b3);
            #endregion

            //- 21. Infanterie-Regiment "Leonhard Haussel"
            Person p3 = (Person)Game.FindCitizen(20);
            p3.JoinArmy(Game.FindArmy(1));
            p3.Army.PromotePerson(p3, 5);
            Regiment reg3 = Regiment.CreateDefault(p3.LocationID);
            inf_vii.AddSubUnit(reg3);
            reg3.Name = "21. Infanterie-Regiment";
            reg3.Owner = Game.FindCountry(0);
            reg3.DefineNewCommander(p3);
            reg3.Parent = inf_vii;
            #endregion

            #endregion
            

            Game.AfterMapLoad();
        }

        //- Use Static
        public static int GetBiggestIndex(int[] array)
        {
            int tmp = int.MinValue;
            int index = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (tmp < array[i])
                {
                    tmp = array[i];
                    index = i;
                }
            }
            return index;
        }
        public static int GetBiggestNumber(int[] array)
        {
            return array[GetBiggestIndex(array)];
        }

        //- Copy Every Property of any Class to other Class
        public static void CopyProperties(this object source, object destination)
        {
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }
                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }

        //- Create Instance of 'Vererbten' -Classes
        public static T CreateClass<T>(Type sub_class)
        {
            return (T)Activator.CreateInstance(sub_class);
        }
        public static T CreateClass<T>(Type sub_class, object[] constructor_args)
        {
            return (T)Activator.CreateInstance(sub_class, constructor_args);
        }
    }
}
