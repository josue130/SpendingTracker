using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SpendingTracker.Application.Services;
using SpendingTracker.Application.Services.IServices;

namespace SpendingTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyBalancesController : ControllerBase
    {
        private readonly IMonthlyBalancesService _monthlyBalancesService;

        public MonthlyBalancesController(IMonthlyBalancesService monthlyBalancesService)
        {
            _monthlyBalancesService = monthlyBalancesService;
        }
        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> Get(Guid accountId)
        {
            var response = await _monthlyBalancesService.GetMonthlyBalance(accountId, User);
            if (response.IsFailure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
