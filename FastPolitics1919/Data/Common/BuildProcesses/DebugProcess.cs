using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastPolitics1919.Common;

namespace FastPolitics1919.Data.Common.BuildProcesses
{
    public class DebugProcess : BuildProcess
    {
        public DebugProcess(GameObject @object)
        {
            GameObject = @object;
        }

        public override void Done()
        {
            base.Done();
            if(GameObject is Tile)
            {
                Tile tile = (Tile)GameObject;
                tile.Name = "Changed !!!!";
            }
        }
    }
}
