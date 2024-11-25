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
        private readonly IRedisCacheService _redisCacheService;
        public CurrencyController(ICurrencyService currencyService, IRedisCacheService redisCacheService)
        {
            _currencyService = currencyService;
            _redisCacheService = redisCacheService;
        }

        [HttpGet("currencies")]
        public IActionResult GetCurrencies()
        {
            var cacheData = _redisCacheService.GetData<string>("currencies");
            if (cacheData is not null)
            {
                return Ok(cacheData);
            }
            var currencies = _currencyService.Currencies();
            _redisCacheService.SetData("currencies",currencies);
            return Ok(currencies);
        }

        [HttpGet("latest")]
        public IActionResult GetLatestRates([FromQuery] string baseCurrency, [FromQuery] string currencies)
        {
            string key = baseCurrency + currencies;
            var cacheData = _redisCacheService.GetData<string>(key);
            if (cacheData is not null)
            {
                return Ok(cacheData);
            }
            var latestRates = _currencyService.LatestCurrency(baseCurrency, currencies);
            _redisCacheService.SetData(key, latestRates);
            return Ok(latestRates);
        }
    }
}
