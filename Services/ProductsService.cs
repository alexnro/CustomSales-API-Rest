using APIRestCustomSales.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRestCustomSales.Services {

    public class ProductsService {

        private readonly IMongoCollection<Product> _products;

        public ProductsService(IDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _products = database.GetCollection<Product>(settings.ProductsCollectionName);
        }

        public List<Product> Get() {
            return _products.Find(product => true).ToList();
        }

        public Product GetById(string id) {
            return _products.Find(product => product.Id == id).FirstOrDefault();
        }

        public Product Create(Product product) {
            _products.InsertOne(product);
            return product;
        }

        public void Delete(string productId) {
            _products.DeleteOne(product => product.Id == productId);
        }
    }
}
