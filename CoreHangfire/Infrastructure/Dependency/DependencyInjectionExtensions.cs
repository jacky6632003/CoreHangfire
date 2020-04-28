using CoreHangfire.Infrastructure.HangFireMisc;
using CoreHangfire.Infrastructure.HangFireMisc.Interface;
using CoreHangfire.Repository.Implement;
using CoreHangfire.Repository.Interface;
using CoreHangfire.Service.Implement;
using CoreHangfire.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHangfire.Infrastructure.Dependency
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDendencyInjection(this IServiceCollection services)
        {
            // HangFire
            services.AddScoped<IHangfireJobTrigger, HangfireJobTrigger>();
            services.AddScoped<IHangfireJobs, HangfireJobs>();

            // Repository
            services.AddScoped<ICustomerchoicebankRepository, CustomerchoicebankRepository>();

            // Service
            services.AddScoped<ICustomerchoicebankService, CustomerchoicebankService>();

            return services;
        }
    }
}