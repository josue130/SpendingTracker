using SpendingTracker.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Errors
{
    public class IncomeError
    {
        public static readonly Error IncomeNotFound = new Error(
            "IncomeNotFound", "The income does not exist");
    }
}
