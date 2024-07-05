using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
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
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _ProductsRepo;
        private readonly IGenericRepository<ProductBrand> _BrandsRepo;
        private readonly IGenericRepository<ProductType> _TypesRepo;
        private readonly IMapper _Mapper;
        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductBrand> brandsRepo, IGenericRepository<ProductType> typesRepo, IMapper mapper)
        {
            _ProductsRepo = productsRepo;
            _BrandsRepo = brandsRepo;
            _TypesRepo = typesRepo;
            _Mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams productParams)
        {
            //var Products = (List<Product>)await _ProductsRepo.GetListAsync();

            // ------------- apply specifications -------------
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _ProductsRepo.CountAsync(countSpec);
            var Products = await _ProductsRepo.ListAsync(spec);

            var data = _Mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(Products);

            return Ok(new Pagination<ProductDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductID(int id)
        {
            //var Product = await _ProductsRepo.GetByIdAsync(id);

            // ------------- apply specifications -------------
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var Product = await _ProductsRepo.GetEntityWithSpec(spec);

            if (Product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(_Mapper.Map<Product, ProductDto>(Product));
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
