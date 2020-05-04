using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRestCustomSales {

    interface IDatabaseSettings {

        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
