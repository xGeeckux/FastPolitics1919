using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastPolitics1919.Data.Managers
{
    public class WindowManager : Manager
    {
        public override string Sign => "WindowManager";

        public List<Window> OpenWindows { get; set; }

        public WindowManager()
        {
            OpenWindows = new List<Window>();
            Write("inizialisiert");
        }

        //- static-Methodes
        private static WindowManager Manager => Engine.WindowManager;
        public static void Show(Window window)
        {
            if (window == null || Manager.OpenWindows.Contains(window))
                return;
            window.Show();
            window.Closing += WindowClosing;
            Manager.OpenWindows.Add(window);

        }
        private static void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window window = (Window)sender;
            if (Manager.OpenWindows.Contains(window))
                Manager.OpenWindows.Remove(window);
        }
        public static void Close(Window window)
        {
            window.Close();
            if (Manager.OpenWindows.Contains(window))
                Manager.OpenWindows.Remove(window);
        }
        public static void Clear()
        {
            int count = Manager.OpenWindows.Count;
            while(count > 0)
            {
                Manager.OpenWindows[0].Close();
                count--;
            }
            Manager.OpenWindows.Clear();
        }

        public static void WriteWindows()
        {
            Manager.Write("Open Windows:");
            foreach (Window window in Manager.OpenWindows)
                Manager.Write("\t" + window);
        }
    }
}
