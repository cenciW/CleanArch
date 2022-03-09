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


        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            /*Nunca cai nessa condição
            if(id == null)
            {
                return NotFound(new {Status = "false", Message = "Invalid Id" });
            }*/

            var category = await _categoryService.GetById(id);
            //Consula do EF no banco, entao tenho que específicar o retorno que desejo!!!

            if(category == null)
            {
                return NotFound(new { Status = "false", Message = "Category not found" });
            }
            return Ok(category);
        }



    }
}
