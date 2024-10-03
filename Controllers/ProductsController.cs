using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppApi.Authorization;
using WebAppApi.Data;
using WebAppApi.Models;

namespace WebAppApi.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ProductsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Policy = "AgeGreaterThan25")]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var username = User.Identity.Name;
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var records = _dbContext.Set<Product>().ToList();
            return Ok(records);
        }

        [HttpGet]
        [Route("GetById")]
        public ActionResult<Product> GetById([FromQuery(Name ="key")] int id)
        {
            var record = _dbContext.Set<Product>().Find(id);
            return record == null ? NotFound() : Ok(record);
        }


        [HttpPost]
        [Route("")]
        public ActionResult<int> CreateProduct([FromQuery]Product product)
        {
            _dbContext.Set<Product>().Add(product);
            _dbContext.SaveChanges();
            return Ok(product.Id);

        }


        [HttpPut]
        [Route("")]
        public ActionResult<int> UpdateProduct(Product product)
        {
            var existingProduct = _dbContext.Set<Product>().Find(product.Id);
            existingProduct.ProductName = product.ProductName;
            existingProduct.Sku = product.Sku;
            _dbContext.Set<Product>().Update(existingProduct);
            _dbContext.SaveChanges();
            return Ok();

        }


        [HttpDelete]
        [Route("")]
        public ActionResult DeleteProduct(int id)
        {
            var existingProduct = _dbContext.Set<Product>().Find(id);

            _dbContext.Set<Product>().Remove(existingProduct);
            _dbContext.SaveChanges();
            return Ok();
        }


    }
}