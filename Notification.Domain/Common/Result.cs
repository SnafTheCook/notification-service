using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None) throw new InvalidOperationException();
            if (!isSuccess && error == Error.None) throw new InvalidOperationException();
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;
        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException();

        protected Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public static Result<TValue> Success(TValue value) => new(value, true, Error.None);
        public static new Result<TValue> Failure(Error error) => new(default, false, error);
    }
}
