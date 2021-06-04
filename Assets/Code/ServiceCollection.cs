using System.Collections.Generic;
using MiniRPG.Common;

namespace MiniRPG
{
    public interface IServiceCollection
    {
        void AddService<T>(T service);
        T GetService<T>();
    }

    public class ServiceCollection : IServiceCollection
    {
        private IDictionary<string, object> _services;
        private Common.ILogger _logger;

        public ServiceCollection(Common.ILogger logger)
        {
            _services = new Dictionary<string, object>();
            _logger = logger;
        }

        private static string GetServiceName<T>()
        {
            return typeof(T).Name;
        }

        public void AddService<T>(T service)
        {
            var serviceName = GetServiceName<T>();
            if(_services.ContainsKey(serviceName))
            {
                _logger.LogError($"Cannot add service {serviceName} to the service collection. A service with same name already exists.");
                return;
            }

            _services[serviceName] = service;
        }

        public T GetService<T>()
        {
            var serviceName = GetServiceName<T>();
            if(_services.TryGetValue(serviceName, out var service))
            {
                return (T)service;
            }

            _logger.LogError($"No service found with name : {serviceName}");
            return default;
        }
    }
}
