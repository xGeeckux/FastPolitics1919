﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Data.Common
{
    public interface IIdentification
    {
        int ID { get; set; }
        string Name { get; set; }
    }
}
