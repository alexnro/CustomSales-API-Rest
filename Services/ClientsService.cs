using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRestCustomSales.Models;
using APIRestCustomSales.Utils;
using MongoDB.Driver;

namespace APIRestCustomSales.Services {

    public class ClientsService {

        private readonly IMongoCollection<Client> _clients;
        private readonly IMongoCollection<Product> _products;

        public ClientsService(IDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _clients = database.GetCollection<Client>(settings.ClientsCollectionName);
            _products = database.GetCollection<Product>(settings.ProductsCollectionName);
        }

        public List<Client> Get() {
            return _clients.Find(client => true).ToList();
        }

        public Client GetById(string id) {
            return _clients.Find(client => client.Id == id).FirstOrDefault();
        }

        public Client GetByName(string clientName) {
            return _clients.Find(client => client.Name == clientName).FirstOrDefault();
        }

        public void Create(Client client) {
            client.VisibleProducts = GetProducts();
            var priceCalculator = new ClientsProductsHelper(_products);
            client = priceCalculator.CalculatePriceVariations(client);
            _clients.InsertOne(client);
        }

        public void Update(Client updatedClient) {
            var priceCalculator = new ClientsProductsHelper(_products);
            updatedClient = priceCalculator.CalculatePriceVariations(updatedClient);
            _clients.ReplaceOne(client => client.Id == updatedClient.Id, updatedClient);
        }

        public void Delete(string clientId) {
            _clients.DeleteOne(client => client.Id == clientId);
        }

        public List<Product> GetProducts() {
            return _products.Find(product => true).ToList();
        }
    }
}
