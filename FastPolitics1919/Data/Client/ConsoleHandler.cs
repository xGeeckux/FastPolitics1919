using FastPolitics1919.Data.Managers;
using System;
using System.Threading;

namespace FastPolitics1919.Data.Client
{
    public class ConsoleHandler
    {
        public static void Start()
        { 
            Thread console_thread = new Thread(Run);
            //console_thread.Start();
        }
        private static void Run()
        {
            string input;
            while (true)
            {
                //Console.Write(Current.Path + ">");
                input = Console.ReadLine();
                //Current.Input(input);
                //try
                //{
                //    Current.Input(input);
                //}
                //catch (Exception e)
                //{
                //    Log.Write(e.Message);
                //}
            }
        }
    }
}
