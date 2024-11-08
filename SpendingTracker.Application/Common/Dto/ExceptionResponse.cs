using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Common.Dto
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description);
}
