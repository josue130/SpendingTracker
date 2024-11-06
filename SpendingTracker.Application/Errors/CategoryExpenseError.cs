using SpendingTracker.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Errors
{
    public class CategoryExpenseError
    {
        public static readonly Error CategoryNotFound = new Error(
            "AccountNotFound", "The category does not exist");
    }
}
