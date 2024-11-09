using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SpendingTracker.Application.Services;
using SpendingTracker.Application.Services.IServices;

namespace SpendingTracker.API.Controllers
{
    [Route("api/monthly-balances")]
    [ApiController]
    [Authorize]
    public class MonthlyBalancesController : ControllerBase
    {
        private readonly IMonthlyBalancesService _monthlyBalancesService;

        public MonthlyBalancesController(IMonthlyBalancesService monthlyBalancesService)
        {
            _monthlyBalancesService = monthlyBalancesService;
        }
        [HttpGet("{accountId:guid},{year:int},{month:int}")]
        public async Task<IActionResult> Get(Guid accountId, int year, int month)
        {
            var response = await _monthlyBalancesService.GetMonthlyBalance(accountId,year,month, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
