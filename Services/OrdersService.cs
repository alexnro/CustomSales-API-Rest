using APIRestCustomSales.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRestCustomSales.Services {

    public class OrdersService {

        private readonly IMongoCollection<Order> _orders;

        public OrdersService(IDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _orders = database.GetCollection<Order>(settings.OrdersCollectionName);
        }

        public List<Order> GetOrders() {
            return _orders.Find(order => true).ToList();
        }

        public Order AddOrder(Order order) {
            _orders.InsertOne(order);
            return order;
        }

    }
}
