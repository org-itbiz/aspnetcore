using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entity;
using IOService.DiscService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace web_api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        //private ISellerService _sellerService { get; set; }
        private ISellerService _sellerService;

        public ValuesController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        // GET api/values
        [HttpGet]
        public JsonResult Get()
        {
            Task<IPagedList<Seller>> data = _sellerService.GetList();
            
            return Json(data.Result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
