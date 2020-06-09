using APIRestCustomSales.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRestCustomSales.Services {

    public class UsersService {

        private readonly IMongoCollection<User> _users;

        public UsersService(IDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> GetUsers() {
            return _users.Find(user => true).ToList();
        }

        public User GetUserByName(string name) {
            return _users.Find(user => user.Name == name).FirstOrDefault();
        }

    }

}