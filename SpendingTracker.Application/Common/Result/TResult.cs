using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Common.Result
{
    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        protected internal Result(TValue value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("No value for failure result");
    }
}
