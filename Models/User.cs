using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Security.Cryptography;

namespace APIRestCustomSales.Models {

    public class User {

        public User(string id, string username, string email, RoleType role) {
            Id = id;
            Username = username;
            Email = email;
            Role = role;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Username { get; set; }
        
        public string Email { get; set; }

        public byte[] Password { get; set; }
        
        [BsonIgnoreIfNull]
        public string AccessToken { get; set; }

        public RoleType Role { get; set; }

        [BsonIgnoreIfNull]
        public byte[] EncryptionKey { get; set; }

        [BsonIgnoreIfNull]
        public byte[] EncryptionIV { get; set; }

    }

}
