using SpendingTracker.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Errors
{
    public class JWTError
    {
        public static readonly Error JwtTokenInvalid = new Error(
            "Token", "Jwt token invalid");
    }
}
