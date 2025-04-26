using System.Linq;
using Example.Visual.UI;
using UnityEngine;

namespace Example.Core
{
    public class MainMenuChain : BaseChain
    {
        private IGameChain _nextChain;
        private CoreChainsData _data;
        private UIService _uiService;
        public override BaseChainsData Handle(BaseChainsData data)
        {
            if (data is CoreChainsData coreData)
            {
                _data = coreData;
                _uiService = _data.GetService<UIService>();
                _uiService.ShowView(ViewType.MainMenuView, OnMenuLoaded);
            }
            else
            {
                Debug.LogError("Invalid data type passed to MainMenuChain.");
            }
            
            return _data;
        }

        private void OnMenuLoaded()
        {
            Debug.Log("Main menu loaded.");
            // Perform any additional actions after the main menu is loaded
        }
    }
}