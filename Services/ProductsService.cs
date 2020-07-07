using APIRestCustomSales.Models;
using APIRestCustomSales.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRestCustomSales.Services {

    public class ProductsService {

        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<Client> _clients;

        public ProductsService(IDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _products = database.GetCollection<Product>(settings.ProductsCollectionName);
            _clients = database.GetCollection<Client>(settings.ClientsCollectionName);
        }

        public List<Product> Get() {
            return _products.Find(product => true).ToList();
        }

        public Product GetById(string id) {
            return _products.Find(product => product.Id == id).FirstOrDefault();
        }

        public Product Create(Product product) {
            _products.InsertOne(product);
            var clients = GetClients();
            var clientPriceCalculator = new ClientsProductsHelper(_products);
            foreach (var client in clients) {
                client.VisibleProducts.Add(product);
                var updatedClient = clientPriceCalculator.CalculatePriceVariations(client);
                _clients.ReplaceOne(clientDB => clientDB.Id == updatedClient.Id, updatedClient);
            }
            return product;
        }

        public void Update(Product updatedProduct) {
            _products.ReplaceOne(product => product.Id == updatedProduct.Id, updatedProduct);
            var clients = GetClients();
            var clientPriceCalculator = new ClientsProductsHelper(_products);
            foreach (var client in clients) {
                var productToUpdate = client.VisibleProducts.Find(product => product.Id == updatedProduct.Id);
                if (productToUpdate != null) {
                    client.VisibleProducts.Remove(productToUpdate);
                    client.VisibleProducts.Add(updatedProduct);
                    var updatedClient = clientPriceCalculator.CalculatePriceVariations(client);
                    _clients.ReplaceOne(clientDB => clientDB.Id == updatedClient.Id, updatedClient);
                }
            }
        }

        public void Delete(string productId) {
            var clients = GetClients();
            var clientPriceCalculator = new ClientsProductsHelper(_products);
            foreach (var client in clients) {
                var productToUpdate = client.VisibleProducts.Find(product => product.Id == productId);
                if (productToUpdate != null) {
                    client.VisibleProducts.Remove(productToUpdate);
                    _clients.ReplaceOne(clientDB => clientDB.Id == client.Id, client);
                }

            }
            _products.DeleteOne(product => product.Id == productId);
        }

        private List<Client> GetClients() {
            return _clients.Find(client => true).ToList();
        }
    }
}
