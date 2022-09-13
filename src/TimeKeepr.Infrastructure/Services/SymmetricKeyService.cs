using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeepr.Infrastructure.Services
{
    public class SymmetricKeyService
    {
        private readonly IConfiguration _config;

        public SymmetricKeyService(IConfiguration config)
        {
            _config = config;
        }

        public SymmetricSecurityKey GetSymmetricKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TimeKeepr:SymmetricKey"]));
        }
    }
}
