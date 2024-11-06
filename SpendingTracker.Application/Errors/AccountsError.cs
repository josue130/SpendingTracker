using SpendingTracker.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Errors
{
    public static class AccountsError
    {

        public static readonly Error InvalidAmount = new Error(
            "Validation", "Amount must be greater than 0");

        public static readonly Error AccountNotFound = new Error(
            "AccountNotFound", "The account does not exist");

    }
}
