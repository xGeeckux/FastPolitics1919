using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FastPolitics1919
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void GameStart(object sender, StartupEventArgs e)
        {
            Engine.Init(e.Args);
        }
    }
}
