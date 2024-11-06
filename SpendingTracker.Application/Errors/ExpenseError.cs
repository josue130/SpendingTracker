using SpendingTracker.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Errors
{
    public class ExpenseError
    {
        public static readonly Error ExpenseNotFound = new Error(
            "ExpenseNotFound", "The expense does not exist");
    }
}
