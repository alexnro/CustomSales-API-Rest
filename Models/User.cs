using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace APIRestCustomSales.Models {

    public class User {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        [BsonIgnoreIfNull]
        public string AccessToken { get; set; }

        public RoleType Role { get; set; }

    }

}