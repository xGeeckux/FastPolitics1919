using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Orders
{
    public class MoveOrder : Order
    {
        public int LocationID { get; set; }
        public override string OrderText => Name + " nach " + Engine.Game.FindTile(LocationID).Name;

        public Unit Unit { get; set; }
        public MoveOrder(Unit unit, int location)
        {
            Unit = unit;
            LocationID = location;
            Name = "Bewegungsbefehl";
        }
        
        public override void Effect()
        {
            Unit.MoveTo(Engine.Game.FindTile(LocationID));
        }
    }
}
