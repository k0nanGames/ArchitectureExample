using System.Collections.Generic;
using Example.Core.Services;

namespace Example.Core
{
    public class CoreChainsData : BaseChainsData
    {
        public bool IsDebugMode { get; set; }
        public List<IGameService> Services { get; set; }

        public T GetService<T>() where T : IGameService
        {
            foreach (var service in Services)
            {
                if (service is T typedService)
                {
                    return typedService;
                }
            }

            return default;
        }
    }
}