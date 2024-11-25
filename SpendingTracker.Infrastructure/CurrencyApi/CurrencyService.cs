using freecurrencyapi;
using SpendingTracker.Application.Common.Interface;

namespace SpendingTracker.Infrastructure.CurrencyApi
{
    public class CurrencyService : ICurrencyService
    {
        private readonly Freecurrencyapi _currencyService;
        public CurrencyService()
        {
            _currencyService = new("YOUR_API_KEY");
        }
        public string Currencies()
        {
            return _currencyService.Currencies();
        }

        public  string LatestCurrency(string baseCurrency, string currencies)
        {
            return _currencyService.Latest(baseCurrency, currencies);
        }
    }
}
