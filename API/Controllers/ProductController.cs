using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepository<Product> _ProductsRepo;
        private readonly IGenericRepository<ProductBrand> _BrandsRepo;
        private readonly IGenericRepository<ProductType> _TypesRepo;
        public ProductController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductBrand> brandsRepo, IGenericRepository<ProductType> typesRepo)
        {
            _ProductsRepo = productsRepo;
            _BrandsRepo = brandsRepo;
            _TypesRepo = typesRepo;
        }


        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            //var Products = (List<Product>)await _ProductsRepo.GetListAsync();

            // ------------- apply specifications -------------
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var Products = await _ProductsRepo.ListAsync(spec);

            return Ok(Products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductID(int id)
        {
            //var Product = await _ProductsRepo.GetByIdAsync(id);

            // ------------- apply specifications -------------
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var Product = await _ProductsRepo.GetEntityWithSpec(spec);

            if (Product == null)
            {
                return NotFound();
            }
            return Ok(Product);
        }


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _BrandsRepo.GetListAsync());
        }


        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _TypesRepo.GetListAsync());
        }
    }
}
