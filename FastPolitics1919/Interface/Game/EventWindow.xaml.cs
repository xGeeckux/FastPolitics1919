using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FastPolitics1919.Interface.Game
{
    /// <summary>
    /// Interaktionslogik für EventWindow.xaml
    /// </summary>
    public partial class EventWindow : Window
    {
        //- Variables
        public Event Event { get; set; }

        public EventWindow(Event @event)
        {
            Event = @event;
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            lbl_event_name.Content = Event.Name;
            panel_options.Children.Clear();
            foreach (EffectButton button in Event.Effects)
            {
                panel_options.Children.Add(button);
            }
        }

        public void Update()
        {

        }
    }
}
