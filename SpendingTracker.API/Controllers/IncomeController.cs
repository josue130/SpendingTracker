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
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet("{accountId:guid},{year:int},{month:int}")]
        public async Task<IActionResult> Get(Guid accountId,  int year, int month)
        {
            var response = await _incomeService.GetIncome(accountId,month,year);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IncomeDto incomeDto)
        {
            var response = await _incomeService.AddIncome(incomeDto, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] IncomeDto incomeDto)
        {
            var response = await _incomeService.UpdateIncome(incomeDto, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid incomeId)
        {
            var response = await _incomeService.DeleteIncome(incomeId, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
