﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Common.Result
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }
        public static Result Success() => new(true, Error.None);
        public static Result<TValue> Success<TValue>(TValue value) => new(value,true, Error.None);
        public static Result Failure(Error error) => new(false, error);
        public static Result<TValue> Failure<TValue>(Error error) => new (default, false, error);


    }

   
}
