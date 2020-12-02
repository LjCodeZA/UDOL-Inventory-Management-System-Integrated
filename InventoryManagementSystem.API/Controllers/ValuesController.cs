using InventoryManagementSystem.API.ViewModels;
using InventoryManagementSystem.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InventoryManagementSystem.API.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("api/values/GetProducts")]
        public JToken GetProducts()
        {
            var dbContext = new IMSContext();
            var productDashboardViewModelList = new List<ProductDashboardViewModel>();

            var products = (from product in dbContext.Product select product);

            foreach (var item in products)
            {
                var productItem = new ProductDashboardViewModel();
                productItem.ProductId = item.ProductId;
                productItem.Name = item.Name;
                productItem.Description = item.Description;
                productItem.ImageURL = item.ImageURL;
                productItem.ListPrice = (from product in dbContext.ProductVendor where product.ProductId == item.ProductId select product.ListPrice).FirstOrDefault();

                productDashboardViewModelList.Add(productItem);
            }

            return JsonConvert.SerializeObject(productDashboardViewModelList);
        }

        [HttpGet]
        [Route("api/values/GetListPriceByProductId/{productId}")]
        public float GetListPriceByProductId(int productId)
        {
            var dbContext = new IMSContext();
            return (from product in dbContext.ProductVendor where product.ProductId == productId select product.ListPrice).FirstOrDefault();
        }

        [HttpGet]
        [Route("api/values/GetProductVendorIdFromProductId/{productId}")]
        public int GetProductVendorIdFromProductId(int productId)
        {
            var dbContext = new IMSContext();
            return (from productVendor in dbContext.ProductVendor where productVendor.ProductId == productId select productVendor.ProductVendorId).FirstOrDefault();
        }
    }
}
