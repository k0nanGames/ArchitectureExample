using System;
using UnityEngine;

namespace Example.Core
{
    public class CoreChainsInitializer : MonoBehaviour
    {
        public void Awake()
        {
            ServicesInitChain initChain = new ServicesInitChain();
            MainMenuChain mainMenuChain = new MainMenuChain();
            
            CoreChainsData data = new CoreChainsData()
            {
                IsDebugMode = false
            };

            initChain.SetNext(mainMenuChain);

            initChain.Handle(data);
        }
    }
}