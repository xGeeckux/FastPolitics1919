using FastPolitics1919.Data;
using FastPolitics1919.Data.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FastPolitics1919.Interface
{
    public abstract class BaseInterface : LogWriter
    {
        public Window Window { get; set; }

        public abstract ImageSource WindowIcon { get; }
        public abstract string WindowTitle { get; }

        public BaseInterface()
        {
            Activate();
            Init();
        }
        
        public abstract void Activate();

        public virtual void Init()
        {
            Window.Icon = WindowIcon;
            Window.Title = WindowTitle;
            Window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Window.ResizeMode = ResizeMode.NoResize;
            Window.KeyUp += KeyUp;
        }

        public void Show()
        {
            WindowManager.Show(Window);
        }
        public void Close()
        {
            WindowManager.Close(Window);
        }

        //- KeyUp
        private void KeyUp(object sender, KeyEventArgs e)
        {
            OnPress(e);
        }

        public virtual void OnPress(KeyEventArgs e)
        {

        }
    }
}
