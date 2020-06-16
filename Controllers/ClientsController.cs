using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIRestCustomSales.Services;
using APIRestCustomSales.Models;
using Microsoft.AspNetCore.Authorization;

namespace APIRestCustomSales.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase {

        private readonly ClientsService _clientsService;

        public ClientsController(ClientsService clientsService) {
            _clientsService = clientsService;
        }

        [HttpGet]
        public ActionResult<List<Client>> Get() {
            return _clientsService.Get();
        }


        [HttpGet]
        [Route("{id}")]
        public ActionResult<Client> GetById(string id) {
            return _clientsService.GetById(id);
        }


    }
}
