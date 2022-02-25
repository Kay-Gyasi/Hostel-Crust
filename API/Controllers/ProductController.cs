using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IDIFactory factory;

        public ProductController(IUnitOfWork uow, IDIFactory factory)
        {
            this.uow = uow;
            this.factory = factory;
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await uow.ProductRepo.GetProductsAsync();

            if (products == null)
            {
                return NotFound();
            }
            var productDto = from c in products
                         select new ProductsDto()
                         {
                             ProductID = c.ProductID,
                             CategoryName = uow.ProductRepo.GetCategoryName(c.CategoryID),
                             Title = c.Name,
                             Price = c.Price,
                             isAvailable = c.isAvailable
                         };

            return Ok(productDto);
        }


        [HttpPost("PostProduct")]
        public async Task<IActionResult> PostProduct(ProductsDto productsDto)
        {
            if (!uow.ProductRepo.ProductExists(productsDto.Title))
            {
                return BadRequest();
            }

            var CId = uow.ProductRepo.GetCategoryId(productsDto.CategoryName);

            Products product = factory.Products();
            #region Mapping
            product.ProductID = productsDto.ProductID;
            product.CategoryID = CId;
            product.Name = productsDto.Title;
            product.Price = productsDto.Price;
            product.isAvailable = productsDto.isAvailable;
            #endregion

            uow.ProductRepo.AddProduct(product);
            await uow.SaveAsync();

            return CreatedAtAction("GetProducts", new { id = product.ProductID }, productsDto);
        }


        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (await uow.ProductRepo.GetProductById(id) == null)
            {
                return BadRequest();
            }

            uow.ProductRepo.DeleteProduct(id);

            await uow.SaveAsync();

            return NoContent();
        }


        [HttpPut("PutProduct/{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductsDto productsDto)
        {

            var product = uow.ProductRepo.GetProductById(id).Result;

            if(product == null)
            {
                return NotFound();
            }

            var CId = uow.ProductRepo.GetCategoryId(productsDto.CategoryName);

            product.ProductID = productsDto.ProductID;
            product.CategoryID = CId;
            product.Name = productsDto.Title;
            product.Price = productsDto.Price;
            product.isAvailable = productsDto.isAvailable;

            await uow.SaveAsync();

            return NoContent();
        }
    }
}
