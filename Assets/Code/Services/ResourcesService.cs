using System;
using Example.Core.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Example.ResourcesManagement
{
    public class ResourcesService : IGameService
    {
        public void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            handle.Completed += res =>
            {
                callback?.Invoke(handle.Result);
            };
        }
        
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
        }

        public Sprite GetSprite(string itemName)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprites/{itemName}");
            
            if (sprite == null)
            {
                Debug.LogError($"Sprite not found: {itemName}");
            }
            
            return sprite;
        }
    }
}
