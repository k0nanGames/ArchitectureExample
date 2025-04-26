using System.Collections.Generic;
using System.Linq;
using Example.Core.Services;
using Example.ResourcesManagement;
using UnityEngine;

namespace Example.Core
{
    public interface IGameChain
    {
        BaseChainsData Handle(BaseChainsData data);
        IGameChain SetNext(IGameChain next);
    }
    
    public class ServicesInitChain : BaseChain
    {
        private const string RESOURCES_SERVICE_NAME = "ResourcesService";
        private const string AUDIO_SERVICE_NAME = "AudioService";
        private const string INPUT_SERVICE_NAME = "InputService";
        private const string UI_SERVICE_NAME = "UIService";
       
        private Dictionary<string, IGameService> _services = new Dictionary<string, IGameService>();
        private ResourcesService _resources;
        private CoreChainsData _data;
        
        public override BaseChainsData Handle(BaseChainsData data)
        {
            if (_services.Count > 0)
            {
                Debug.LogError("Services have already been initialized.");
                return null;
            }

            _resources = new ResourcesService();
            _services.Add(RESOURCES_SERVICE_NAME, _resources);
            
            if (data is not CoreChainsData coreChainsData)
            {
                Debug.LogError("Invalid data type. Expected CoreChainsData.");
                return null;
            }
            
            LoadService(AUDIO_SERVICE_NAME);
            LoadService(INPUT_SERVICE_NAME);
            LoadService(UI_SERVICE_NAME);
            
            _data = coreChainsData;
            _data.Services = _services.Values.ToList();
            return HandleNext(_data);
        }

        private void LoadService(string serviceName)
        {
            GameObject prefab = _resources.Load<GameObject>(serviceName);
            if (prefab == null)
            {
                Debug.LogError($"Service prefab not found: {serviceName}");
                return;
            }
            
            GameObject serviceInstance = Object.Instantiate(prefab, null, true);
            IGameService service = serviceInstance.GetComponent<IGameService>();
            if (service == null)
            {
                Debug.LogError($"Service component not found in prefab: {serviceName}");
                return;
            }
            
            CheckRelations(service);
            _services.Add(serviceName, service);
        }

        private void CheckRelations<T>(T service) where T : IGameService
        {
            if(service is INeedResources resourcesRequired)
            {
                resourcesRequired.SetResourcesService(_resources);
            }
        }
    }
}