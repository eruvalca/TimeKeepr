using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TimeKeepr.Application.Holidays;
using TimeKeepr.Application.PtoEntries;

namespace TimeKeepr.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<HolidaysService>();
            services.AddScoped<PtoEntriesService>();

            return services;
        }
    }
}
