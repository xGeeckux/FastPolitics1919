using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Gfx
{
    public static class Images
    {
        #region paths
        //- Base
        private static string exe = Environment.CurrentDirectory + @"\";
        private static string image_prefix = ".png";
        //- Folders
        private static string gfx = @"gfx\";
        private static string map = @"map\";
        private static string army = @"army\";
        private static string default_ranks = @"default_ranks\";
        private static string default_sizes = @"default_sizes\";
        private static string default_counters = @"default_counters\";
        private static string city = @"city_gfx\";
        private static string flag = @"flags\";
        private static string top_bar = @"top_bar\";
        private static string person = @"person_gfx\";
        private static string city_background = @"tab_backgrounds\";
        private static string unique = @"unique\";
        private static string persons = @"persons\";
        private static string building = @"buildings\";
        private static string modes = @"map_modes\";
        private static string tile = @"tile\";
        //- Defined
        public static string unique_buildings = gfx + city + building + unique;
        public static string person_potrai = gfx + persons;
        public static string military_ranks = gfx + army + default_ranks;
        public static string military_counters = gfx + army + default_counters;
        public static string map_modes = gfx + map + modes;
        public static string map_tiles = gfx + map + tile;
        #endregion

        //- Map
        public static BitmapImage Map = new BitmapImage(new Uri(exe + gfx + "Map" + image_prefix));
        
        //- Default
        public static BitmapImage IconQuestionmark = new BitmapImage(new Uri(exe + gfx + "icon_questionmark" + image_prefix));

        //- Gui for TopBar
        public static BitmapImage IconMoney = new BitmapImage(new Uri(exe + gfx + map + top_bar + "icon_money" + image_prefix));
        public static BitmapImage IconTime = new BitmapImage(new Uri(exe + gfx + map + top_bar + "icon_time" + image_prefix));

        public static BitmapImage TileLandschaft = new BitmapImage(new Uri(exe + gfx + map + tile + "tile_landschaft" + image_prefix));
        public static BitmapImage TileCity = new BitmapImage(new Uri(exe + gfx + map + tile + "tile_city" + image_prefix));

        public static BitmapImage FlagREB = new BitmapImage(new Uri(exe + gfx + flag + "REB" + image_prefix));
        
        public static BitmapImage IconPlayer = new BitmapImage(new Uri(exe + gfx + map + "icon_player" + image_prefix));
        public static BitmapImage IconPerson = new BitmapImage(new Uri(exe + gfx + persons + "icon_person" + image_prefix));
        public static BitmapImage IconFollower = new BitmapImage(new Uri(exe + gfx + person + "icon_follower" + image_prefix));

        public static BitmapImage IconIdeology = new BitmapImage(new Uri(exe + gfx + city + "icon_ideology" + image_prefix));

        public static BitmapImage IconCity = new BitmapImage(new Uri(exe + gfx + city + "icon_city" + image_prefix));
        public static BitmapImage IconLandscape = new BitmapImage(new Uri(exe + gfx + city + "icon_landscape" + image_prefix));
        
        public static BitmapImage IconCapital = new BitmapImage(new Uri(exe + gfx + city + "icon_capital" + image_prefix));
        public static BitmapImage IconDevelopment = new BitmapImage(new Uri(exe + gfx + city + "icon_development" + image_prefix));

        public static BitmapImage IconBackgroundCity = new BitmapImage(new Uri(exe + gfx + city + city_background + "img_city" + image_prefix));
        public static BitmapImage IconBackgroundLandscape = new BitmapImage(new Uri(exe + gfx + city + city_background + "img_landscape" + image_prefix));
        public static BitmapImage IconBackground1 = new BitmapImage(new Uri(exe + gfx + city + city_background + "img_city1" + image_prefix));
        public static BitmapImage IconBackground2 = new BitmapImage(new Uri(exe + gfx + city + city_background + "img_city2" + image_prefix));
        public static BitmapImage IconBackground3 = new BitmapImage(new Uri(exe + gfx + city + city_background + "img_city3" + image_prefix));
        public static BitmapImage IconBackground4 = new BitmapImage(new Uri(exe + gfx + city + city_background + "img_city4" + image_prefix));
        public static BitmapImage IconBackground5 = new BitmapImage(new Uri(exe + gfx + city + city_background + "img_city5" + image_prefix));

        public static BitmapImage IconTabCommon = new BitmapImage(new Uri(exe + gfx + city + "icon_tab_common" + image_prefix));
        public static BitmapImage IconTabParlament = new BitmapImage(new Uri(exe + gfx + city + "icon_tab_parlament" + image_prefix));

        public static BitmapImage IconBuilding = new BitmapImage(new Uri(exe + gfx + city + "icon_building" + image_prefix));
        public static BitmapImage IconEmptyBuilding = new BitmapImage(new Uri(exe + gfx + city + "icon_empty_building" + image_prefix));
        public static BitmapImage IconPartyBuilding = new BitmapImage(new Uri(exe + gfx + city + building + "icon_partybuilding" + image_prefix));
        public static BitmapImage IconGovernmentBuilding = new BitmapImage(new Uri(exe + gfx + city + building + "icon_government_building" + image_prefix));

        public static BitmapImage IconMalePerson = new BitmapImage(new Uri(exe + gfx + city + "icon_person" + image_prefix));
        public static BitmapImage IconFemalePerson = new BitmapImage(new Uri(exe + gfx + city + "icon_person_woman" + image_prefix));

        #region Military
        public static BitmapImage IconUnitSizeTeam = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_-2" + image_prefix));
        public static BitmapImage IconUnitSizeGroup = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_-1" + image_prefix));
        public static BitmapImage IconUnitSizePlatoon = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_0" + image_prefix));
        public static BitmapImage IconUnitSizeAttachment = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_0" + image_prefix));
        public static BitmapImage IconUnitSizeCompany = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_1" + image_prefix));
        public static BitmapImage IconUnitSizeBataillony = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_2" + image_prefix));
        public static BitmapImage IconUnitSizeRegiment = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_3" + image_prefix));
        public static BitmapImage IconUnitSizeDivision = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_4" + image_prefix));
        public static BitmapImage IconUnitSizeCorps = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_5" + image_prefix));
        public static BitmapImage IconUnitSizeArmy = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_6" + image_prefix));
        public static BitmapImage IconUnitSizeArmygroup = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_7" + image_prefix));
        public static BitmapImage IconUnitSizeCommando = new BitmapImage(new Uri(exe + gfx + army + default_sizes + "size_8" + image_prefix));

        #endregion

        public static BitmapImage FromPath(string path)
        {
            BitmapImage img = null;
            try
            {
                img = new BitmapImage(new Uri(exe + path + image_prefix));
            }
            catch (Exception)
            {
                Log.Write("Image not found: " + path);
                img = Images.IconQuestionmark;
            }
            return img;
        }
    }
}
