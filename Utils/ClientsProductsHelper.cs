using APIRestCustomSales.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRestCustomSales.Utils {

    public class ClientsProductsHelper {

        private readonly IMongoCollection<Product> _products;

        public ClientsProductsHelper(IMongoCollection<Product> products) {
            _products = products;
        }

        public Client CalculatePriceVariations(Client client) {
            var products = _products.Find(product => true).ToList();
            foreach (Product product in products) {
                Product availableProduct = client.VisibleProducts.Find(visibleProduct => visibleProduct.Id == product.Id);          
                if (availableProduct != null) {
                    if (client.PriceVariation != 0d) {
                        availableProduct.Price = Math.Round(product.Price * (1 + client.PriceVariation / 100), 2);
                    } else {
                        availableProduct.Price = product.Price;
                    }
                }
            }
            return client;
        }
    }
}
