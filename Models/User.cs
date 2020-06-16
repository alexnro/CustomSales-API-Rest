using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace APIRestCustomSales.Models {

    public class User {

        public User(string id, string username, string email, string token, RoleType role) {
            Id = id;
            Username = username;
            Email = email;
            Token = token;
            Role = role;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Username { get; set; }
        
        public string Email { get; set; }

        [JsonIgnore]
        public byte[] Password { get; set; }
        
        [BsonIgnoreIfNull]
        public string Token { get; set; }

        public RoleType Role { get; set; }

        [BsonIgnoreIfNull]
        [JsonIgnore]
        public byte[] EncryptionKey { get; set; }

        [BsonIgnoreIfNull]
        [JsonIgnore]
        public byte[] EncryptionIV { get; set; }

    }

}
