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
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("{accountId:guid},{year:int},{month:int}")]
        public async Task<IActionResult> Get(Guid accountId, int year, int month)
        {
            var response = await _expenseService.GetExpense(accountId, month, year);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenseDto expenseDto)
        {
            var response = await _expenseService.AddExpense(expenseDto, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ExpenseDto expenseDto)
        {
            var response = await _expenseService.UpdateExpense(expenseDto, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid expenseId)
        {
            var response = await _expenseService.DeleteExpense(expenseId, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
