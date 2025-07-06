using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApplication.Persistance
{
    public static class AddPersistanceService
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
