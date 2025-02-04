﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRestCustomSales.Models {

    public class Order {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Client Client { get; set; }

        public List<OrderProduct> Products { get; set; }

        public double TotalPrice { get; set; }

    }
}
