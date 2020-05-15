using FastPolitics1919.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919
{
    public class Log
    {
        //- Variablen
        private List<string> LogList { get; set; }

        //- Konstruktor
        public Log()
        {
            LogList = new List<string>();
        }

        //- Init-Methode
        public void Init()
        {
            Write("Log inizialisiert.");
        }

        //- static Write
        private static void WritePrivate(string sign, object text)
        {
            string txt = "[" + sign + "]: " + text.ToString();
            Console.WriteLine(txt);
            Engine.Log.LogList.Add(txt);
        }
        public static void Write(LogWriter writer, object text)
        {
            WritePrivate(writer.Sign, text);
        }
        public static void Write(object text)
        {
            WritePrivate("Log", text);
        }
    }
}
