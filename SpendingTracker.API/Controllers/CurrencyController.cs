using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Common.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpendingTracker.API.Controllers
{
    [Route("api/currency")]
    [ApiController]
    [Authorize]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("currencies")]
        public IActionResult GetCurrencies()
        {
            var currencies = _currencyService.Currencies();
            return Ok(currencies);
        }

        [HttpGet("latest")]
        public IActionResult GetLatestRates([FromQuery] string baseCurrency, [FromQuery] string currencies)
        {
            var latestRates = _currencyService.LatestCurrency(baseCurrency, currencies);
            return Ok(latestRates);
        }
    }
}
