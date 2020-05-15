using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Interface.Game
{
    public class PersonSubWindow : SubWindow
    {
        //- Variable
        public Person Person { get; set; }
        public PersonWindow Window { get; set; }

        //- Constructor
        public PersonSubWindow(Person person)
        {
            Person = person;
            Constructor(Window = new PersonWindow(Person));
        }

        protected override void InternLoad()
        {

        }

        public override void Update() => Window.Update();

    }
}
