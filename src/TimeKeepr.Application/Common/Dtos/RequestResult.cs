using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeepr.Application.Common.Dtos
{
    public class RequestResult<T>
    {
        internal RequestResult(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        internal RequestResult(bool succeeded, T data)
        {
            Succeeded = succeeded;
            Data = data;
        }

        public bool Succeeded { get; set; }
        public string[]? Errors { get; set; }
        public T? Data { get; set; }

        public static RequestResult<T> Success(T data)
        {
            return new RequestResult<T>(true, data);
        }

        public static RequestResult<T> Failure(IEnumerable<string> errors)
        {
            return new RequestResult<T>(false, errors);
        }

        public RequestResult()
        {

        }
    }
}
