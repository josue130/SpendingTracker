using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Common.Interface
{
    public interface ICurrencyService
    {
        string Currencies();

        string LatestCurrency(string baseCurrency,string currencies);
    }
}
