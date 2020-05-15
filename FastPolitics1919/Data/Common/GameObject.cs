using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Data.Common
{
    [Serializable]
    public class GameObject : IIdentification, IProcess
    {
        //- IIdentification
        public int ID { get; set; }
        public string Name { get; set; }

        //- IProcess
        public virtual bool IsProcessable => false;
        public virtual int ProcessRound => BaseProcessRound;
        public virtual int BaseProcessRound => -1;
        public BuildProcess MyProcess { get; set; }
    }
}
