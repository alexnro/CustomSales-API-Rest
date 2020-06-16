using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRestCustomSales.Models;
using APIRestCustomSales.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRestCustomSales.Controllers {

    [Authorize]
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

        [HttpPut]
        public IActionResult Update(Product updatedProduct) {
            var product = _productsService.GetById(updatedProduct.Id);

            if (product == null) {
                return NotFound();
            }

            _productsService.Update(updatedProduct);
            return NoContent();

        }

        [HttpDelete]
        public IActionResult Delete(string productId) {
            var product = _productsService.GetById(productId);

            if (product == null) {
                return NotFound();
            }

            _productsService.Delete(productId);
            return NoContent();
        }
    }
}
