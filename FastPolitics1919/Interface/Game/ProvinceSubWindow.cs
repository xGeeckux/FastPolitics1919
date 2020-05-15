using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Interface.Game
{
    public class ProvinceSubWindow : SubWindow
    {
        //- Variable
        public Province Province { get; set; }
        public ProvinceWindow Window { get; set; }

        //- Constructor
        public ProvinceSubWindow(Province province)
        {
            Province = province;
            Constructor(Window = new ProvinceWindow(Province));
        }

        //- Load City specific content
        protected override void InternLoad()
        {

        }

        public override void Update() => Window.Update();

        //- City Window Exit
        protected override void ExitWindow()
        {
            //- Deselect
            if (Engine.Map.CurrentSelected is Province && Engine.Map.CurrentSelected == Province)
                Engine.Map.SetCurrentSelected(Engine.Map.CurrentSelected);
        }
    }
}
