using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Data.Common
{
    public interface IProcess
    {
        bool IsProcessable { get; }
        int ProcessRound { get; }
    }
}
