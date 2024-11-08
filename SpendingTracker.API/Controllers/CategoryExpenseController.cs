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
    public class CategoryExpenseController : ControllerBase
    {
        private readonly ICategoryExpenseService _categoryExpenseService;
        public CategoryExpenseController(ICategoryExpenseService categoryExpenseService)
        {
            _categoryExpenseService = categoryExpenseService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _categoryExpenseService.GetCategories(User);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryExpenseDto categoryExpenseDto)
        {
            var response = await _categoryExpenseService.CreateCategoryExpense(categoryExpenseDto, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CategoryExpenseDto categoryExpenseDto)
        {
            var response = await _categoryExpenseService.UpdateCategoryExpense(categoryExpenseDto, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid categoryId)
        {
            var response = await _categoryExpenseService.DeleteCategoryExpense(categoryId, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
