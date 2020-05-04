using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRestCustomSales.Models;
using APIRestCustomSales.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRestCustomSales.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {

        private readonly ProductsService _productsService;

        public ProductsController(ProductsService productsService) {
            _productsService = productsService;
        }

        [HttpGet]
        public ActionResult<List<Product>> Get() {
            return _productsService.Get();
        }

        [HttpPost]
        public ActionResult<Product> Create(Product product) {
            _productsService.Create(product);
            return CreatedAtRoute(new { id = product.Id.ToString() }, product);
        }
    }
}
