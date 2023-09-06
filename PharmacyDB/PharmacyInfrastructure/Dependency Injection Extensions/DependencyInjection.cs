using Microsoft.Extensions.DependencyInjection;
using PharmacyDB.Interfaces;
using PharmacyInfrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyInfrastructure.Dependency_Injection_Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            
            return services;
        }
    }
}
