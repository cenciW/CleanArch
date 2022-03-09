using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchMvc.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categories = await _categoryService.GetCategories();
            if (categories == null)
            {
                return NotFound("Categories not found");
            }
            return Ok(categories);
        }


        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            /*Nunca cai nessa condição
            if(id == null)
            {
                return NotFound(new {Status = "false", Message = "Invalid Id" });
            }*/

            var category = await _categoryService.GetById(id);
            //Consula do EF no banco, entao tenho que específicar o retorno que desejo!!!

            if (category == null)
            {
                return NotFound(new { Status = "false", Message = "Category not found" });
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(new { Status = false, Message = "Invalid data" });
            }

            await _categoryService.Add(categoryDto);

            //Retornar 201 - Created
            return new CreatedAtRouteResult("GetCategory", new { id = categoryDto.Id }, categoryDto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int? id, [FromBody] CategoryDTO categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest(new { Status = false, Message = "Invalid Id" });
            }

            if (categoryDto == null)
            {
                return BadRequest(new { Status = false, Message = "Invalid data" });
            }

            await _categoryService.Update(categoryDto);

            return Ok(categoryDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Delete(int? id)
        {
            var category = await _categoryService.GetById(id);
            
            if(category == null)
            {
                return NotFound(new { Status = "False", Message = "Category not found" });
            }


            await _categoryService.Remove(id);
            return Ok(new { Status = "True", Message = "Category deleted was: " + category.Name});
        }

    }
}
