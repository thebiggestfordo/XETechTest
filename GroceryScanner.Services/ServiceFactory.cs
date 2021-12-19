using GroceryScanner.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryScanner.Services
{
    public class ServiceFactory
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPointOfSaleTerminalService, PointOfSaleTerminalService>();
        }
    }
}
