
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TopView.Common.Infrastructure
{
    public static class ServiceHelper
    {
        public static IServiceProvider _serviceProvider { get; private set; }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static TService GetService<TService>() where TService : notnull
            => ActivatorUtilities.GetServiceOrCreateInstance<TService>(_serviceProvider);
    }
}