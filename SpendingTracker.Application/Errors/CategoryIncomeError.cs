using SpendingTracker.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Errors
{
    public class CategoryIncomeError
    {
        public static readonly Error CategorytNotFound = new Error(
            "AccountNotFound", "The category does not exist");
        public static readonly Error InvalidInputs = new Error(
            "Validation", "Inputs cannot be empty");
    }
}
