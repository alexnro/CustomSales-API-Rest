using APIRestCustomSales.Auth;
using APIRestCustomSales.Models;
using APIRestCustomSales.Utils;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APIRestCustomSales.Services {

    public class UsersService {

        private readonly IMongoCollection<User> _users;
        private readonly AuthSettings _authSettings;

        public UsersService(IDatabaseSettings settings, IOptions<AuthSettings> authSettings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
            _authSettings = authSettings.Value;
        }

        public List<User> GetUsers() {
            return _users.Find(user => true).ToList();
        }

        public User GetUserByUsername(string username) {
            return _users.Find(user => user.Username == username).FirstOrDefault();
        }

        public User GetUserByToken(string token) {
            return _users.Find(user => user.Token != null && user.Token == token).FirstOrDefault();
        }

        public User GetUserById(string userId) {
            return _users.Find(user => user.Id == userId).FirstOrDefault();
        }

        public User AddUser(User newUser) {
            _users.InsertOne(newUser);
            return newUser;
        }

        public void Update(User updatedUser) {
            _users.ReplaceOne(user => user.Id == updatedUser.Id, updatedUser);
        }

        public void Delete(string userId) {
            _users.DeleteOne(user => user.Id == userId);
        }

        public User HandleLogin(LoginUser loginUser) {
            var user = GetUserByUsername(loginUser.Username);
            try {
                if (user != null && ComparePasswordWithEncrypt(user, loginUser.Password)) {
                    User tokenUser = Authenticate(loginUser);
                    return tokenUser;
                }
            } catch (ArgumentNullException) {
                return null;
            }
            return null;
        }

        public User HandleCreatePassword(LoginUser loginUser) {
            var user = GetUserByUsername(loginUser.Username);
            if (user != null && user.Password == null) {
                CreateEncryptedPassword(user, loginUser);
                user = Authenticate(loginUser);
                return user;
            }
            return null;
        }

        public void ResetPassword(User user) {
            user.Password = null;
            user.EncryptionIV = null;
            user.EncryptionKey = null;
            Update(user);
        }

        public void HandleLogout(User logoutUser) {
            logoutUser.Token = null;
            _users.ReplaceOne(user => user.Username == logoutUser.Username, logoutUser);
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

        public User Authenticate(LoginUser loginUser) {
            var user = GetUserByUsername(loginUser.Username);

            if (user == null) return null;

            var token = GenerateJwtToken(user);
            var updatedUser = user;
            updatedUser.Token = token;
            _users.ReplaceOne(user => user.Username == loginUser.Username, updatedUser);

            return user;
        }

        private string GenerateJwtToken(User user) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }

}