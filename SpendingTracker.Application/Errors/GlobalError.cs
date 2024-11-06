using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Errors
{
    public class GlobalError
    {
        public static readonly Error InvalidInputs = new Error(
            "Validation", "Inputs cannot be empty");
    }
}
