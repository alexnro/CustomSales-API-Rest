using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRestCustomSales {

    public interface IDatabaseSettings {

        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string ProductsCollectionName { get; set; }
        string ClientsCollectionName { get; set; }
    }
}
