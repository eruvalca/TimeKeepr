using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeepr.Application.Identity.Dtos
{
    public class IdentityRequestResult
    {
        internal IdentityRequestResult(bool succeeded, IEnumerable<string> errors, string token, string userId)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Token = token;
            UserId = userId;
        }

        public bool Succeeded { get; set; }
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public string[]? Errors { get; set; }

        public static IdentityRequestResult Success(string token, string userId)
        {
            return new IdentityRequestResult(true, Array.Empty<string>(), token, userId);
        }

        public static IdentityRequestResult Failure(IEnumerable<string> errors)
        {
            return new IdentityRequestResult(false, errors, string.Empty, string.Empty);
        }

        public IdentityRequestResult()
        {

        }
    }
}
