using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FastPolitics1919.Common;
using FastPolitics1919.Gfx;
using FastPolitics1919.History.Cultures;
using FastPolitics1919.History.Ideologies;

namespace FastPolitics1919.Data.Tmp
{
    public static class ContentGenerator
    {
        public static void Run()
        {
            Party(new Party(0, "Reichspartei zu Bayern", "RPB", null, Ideology.Get(Ideologies.Conservative)));
            Party(new Party(1, "Konservative Partei Deutschland", "KonPD", null, Ideology.Get(Ideologies.Conservative)));
            Party(new Party(2, "Demokratische Partei Deutschland", "DPD", null, Ideology.Get(Ideologies.Democrat)));
            Party(new Party(3, "Frei Wirtschaftsfront", "FW", null, Ideology.Get(Ideologies.Liberal)));
            Party(new Party(4, "Kommunistische Partei Deutschland", "KPD", null, Ideology.Get(Ideologies.Communist)));
            Party(new Party(5, "Nationalistische Front", "NF", null, Ideology.Get(Ideologies.Conservative)));

            Engine.Game.Cultures.Add(new Culture("Deutsch"), (int)Cultures.Deutsch);
            Engine.Game.Cultures.Add(new Culture("Österreichisch"), (int)Cultures.Austria);

            Country bayern = new Country();
            bayern.ID = 0;
            bayern.Name = "Bayern";
            bayern.RGBColor = "53-64-57";
            bayern.CapitalID = 158;
            bayern.Government = new History.Governments.CountryGovernment(bayern);
            bayern.Government.RegisteredParties.Add(Engine.Game.FindParty(0));
            bayern.Government.RegisteredParties.Add(Engine.Game.FindParty(1));
            bayern.Government.RegisteredParties.Add(Engine.Game.FindParty(2));
            bayern.Government.RegisteredParties.Add(Engine.Game.FindParty(3));
            bayern.Government.RegisteredParties.Add(Engine.Game.FindParty(4));
            bayern.Army = new Army("Bayrische Reichs-Armee");
            bayern.Army.Owner = bayern;
            bayern.Flag = Images.FromPath(@"gfx\flags\BAV");
            Country(bayern);

            Country rebel = new Country();
            rebel.ID = 1;
            rebel.Name = "Rebellen";
            rebel.RGBColor = "180-0-0";
            rebel.Government = new History.Governments.CountryGovernment(rebel);
            rebel.Government.RegisteredParties.Add(Engine.Game.FindParty(5));
            rebel.Army = new Army("Rebellen Armee");
            rebel.Army.Owner = rebel;
            rebel.Flag = Images.FlagREB;
            Country(rebel);

            //- Provinces
            Province province = new Province();
            province.ID = 0;
            province.Name = "Erste Province";
            province.Owner = bayern;
            Province(province);
        }
        private static void CIdeology(Ideology ideology)
        {
            Engine.Game.Ideologies.Add(ideology, ideology.ID);
        }
        private static void City(City city)
        {
            Engine.Game.Tiles.Add(city, city.ID);
        }
        private static void Citizen(Citizen citizen)
        {
            Engine.Game.Citizens.Add(citizen, citizen.ID);
        }
        private static void Province(Province province)
        {
            Engine.Game.Provinces.Add(province, province.ID);
        }
        private static void Country(Country country)
        {
            Engine.Game.Countries.Add(country, country.ID);
        }
        private static void Party(Party party)
        {
            Engine.Game.Parties.Add(party, party.ID);
        }
    }
}
