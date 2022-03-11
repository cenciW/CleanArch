using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchMvc.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await _productService.GetProducts();

            if (products == null)
                return NotFound(new {Status = "false", Message = "Products not founded" });

            return Ok(products);

        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int? id)
        {
            var product = await _productService.GetById(id);

            if(product == null)
            {
                return NotFound(new { Status = "false", Message = "Products not founded" });
            }
            
            return Ok(product);            
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDto)
        {
            if(productDto == null)
            {
                return BadRequest(new {Status = "False", Message= "Invalid Data" });
            }

            await _productService.Add(productDto);

            return new CreatedAtRouteResult("GetProduct",
                new { id = productDto.Id }, productDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int? id, [FromBody] ProductDTO productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest("Data invalid");
            }

            if (productDto == null)
                return BadRequest("Data invalid");

            await _productService.Update(productDto);

            return Ok(productDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int? id)
        {
            var product = _productService.GetById(id);
            if(product == null)
            {
                return NotFound("Product not found");
            }

            await _productService.Remove(id);
            return Ok(product);

        }
    }
}
