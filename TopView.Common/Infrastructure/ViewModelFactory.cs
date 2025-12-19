
using Microsoft.Extensions.DependencyInjection;

namespace TopView.Common.Infrastructure
{
    public class ViewModelFactory(IServiceProvider serviceProvider) : IViewModelFactory
    {
        public T Create<T>() where T : class 
            => ActivatorUtilities.GetServiceOrCreateInstance<T>(serviceProvider);

        public T Create<T>(params object[] parameters) where T : class
        {
            return ActivatorUtilities.CreateInstance<T>(serviceProvider, parameters);
        }
    }
}
