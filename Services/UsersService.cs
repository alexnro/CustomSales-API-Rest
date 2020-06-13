using APIRestCustomSales.Models;
using APIRestCustomSales.Utils;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public User HandleLogin(LoginUser loginUser) {
            var user = _users.Find(user => user.Username == loginUser.Username).FirstOrDefault();
            if (user != null && ComparePasswordWithEncrypt(user, loginUser.Password)) {
                // Return only minimum data to avoid having access to
                // password or encryption keys in client-side
                var returnedUser = new User(user.Id, user.Username, user.Email, user.Role);
                return returnedUser;
            }
            return null;
        }

        public bool ComparePasswordWithEncrypt(User user, string password) {
            string roundtrip = EncryptionHelper.DecryptStringFromBytes(user.Password, user.EncryptionKey, user.EncryptionIV);

            if (roundtrip == password) {
                return true;
            }

            return false;

        }

        public void CreateEncryptedPassword(User user, LoginUser loginUser) {
            using (Rijndael rijndael = Rijndael.Create()) {
                byte[] encrypted = EncryptionHelper.EncryptStringToBytes(loginUser.Password, rijndael.Key, rijndael.IV);
                var updatedUser = user;
                updatedUser.Password = encrypted;
                updatedUser.EncryptionKey = rijndael.Key;
                updatedUser.EncryptionIV = rijndael.IV;
                _users.ReplaceOne(user => user.Username == loginUser.Username, updatedUser);
            }
        }

    }

}