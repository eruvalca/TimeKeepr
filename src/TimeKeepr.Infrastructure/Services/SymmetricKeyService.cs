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
        private readonly IWebHostEnvironment _env;

        public SymmetricKeyService(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public SymmetricSecurityKey GetSymmetricKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TimeKeepr:SymmetricKey"]));
        }
    }
}
