using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRestCustomSales.Models;
using MongoDB.Driver;

namespace APIRestCustomSales.Services {

    public class ClientsService {

        private readonly IMongoCollection<Client> _clients;

        public ClientsService(IDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _clients = database.GetCollection<Client>(settings.ClientsCollectionName);
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
            _clients.InsertOne(client);
        }

        public void Update(Client updatedClient) {
            _clients.ReplaceOne(client => client.Id == updatedClient.Id, updatedClient);
        }
    }
}
