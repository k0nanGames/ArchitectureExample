using Example.ResourcesManagement;

namespace Example.Core.Services
{
    public interface IGameService
    {
        
    }
    
    public interface INeedResources
    {
        void SetResourcesService(ResourcesService resourcesService);
    }
}