using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Services.IServices;

namespace SpendingTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryIncomeController : ControllerBase
    {
        private readonly ICategoryIncomeService _categoryIncomeService;
        public CategoryIncomeController(ICategoryIncomeService categoryIncomeService)
        {
            _categoryIncomeService = categoryIncomeService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _categoryIncomeService.GetCategories(User);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryIncomeDto categoryIncomeDto)
        {
            var response = await _categoryIncomeService.CreateCategoryIncome(categoryIncomeDto, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CategoryIncomeDto categoryIncomeDto)
        {
            var response = await _categoryIncomeService.UpdateCategoryIncome(categoryIncomeDto, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid categoryId)
        {
            var response = await _categoryIncomeService.DeleteCategoryIncome(categoryId, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
