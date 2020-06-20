using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRestCustomSales {

    public class DatabaseSettings : IDatabaseSettings {

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ProductsCollectionName { get; set; }
        public string ClientsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string OrdersCollectionName { get; set; }
    }
}
