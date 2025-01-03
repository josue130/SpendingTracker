using SpendingTracker.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Errors
{
    public static class AuthErrors
    {
        public static readonly Error UserNameAlreadyExits = new Error(
        "Conflict", "Username already exits");

        public static readonly Error InvalidEmailFormat = new Error(
            "Validation", "Invalid email format");

        public static readonly Error UserNameNotExist = new Error(
            "Credentials", "The user does not exist");

        public static readonly Error InvalidCredentials = new Error(
            "Credentials", "Invalid Credentials");
    }
}
