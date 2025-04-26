using System;
using System.Collections.Generic;
using System.Linq;
using Example.Core.Services;
using Example.ResourcesManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Example.Visual.UI
{
    public interface IUIService
    {
        public void ShowView(ViewType view, Action onLoaded);
        void HideView(ViewType view);
        T GetOpenedView<T>() where T : BasicView;
    }
    
    /// <summary>
    /// Service for managing UI views
    /// </summary>
    public class UIService : MonoBehaviour, IUIService, IGameService, INeedResources
    {
        /// <summary>
        /// The root canvas for the UI
        /// </summary>
        [SerializeField] private Canvas _canvas;
        
        private ResourcesService _resourcesService;
        
        private Dictionary<ViewType, BasicView> _openedViews = new Dictionary<ViewType, BasicView>();
        private Dictionary<ViewType, BasicView> _cashedViews = new Dictionary<ViewType, BasicView>();
        private Dictionary<ViewType, Action> _onLoadedActions = new Dictionary<ViewType, Action>();
        
        public event Action<ViewType> OnViewShowed;

        public void Init(ResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }
        
        public void ShowView(ViewType view, Action onLoaded)
        {
            if(_openedViews.ContainsKey(view))
            {
                _openedViews[view].Show();
                _openedViews[view].gameObject.SetActive(true);
                onLoaded?.Invoke();
                return;
            }
            
            if(_cashedViews.ContainsKey(view))
            {
                _cashedViews[view].Show();
                _cashedViews[view].gameObject.SetActive(true);
                
                _openedViews.Add(view, _cashedViews[view]);
                onLoaded?.Invoke();
                return;
            }
            
            if(_onLoadedActions.ContainsKey(view))
            {
                _onLoadedActions[view] += onLoaded;
            }
            else
            {
                _onLoadedActions.Add(view, onLoaded);
            }

            _resourcesService.LoadAsync<GameObject>($"{view}", OnViewLoaded);
        }

        private void OnViewLoaded(GameObject viewPrefab)
        {
            GameObject viewInstance = Instantiate(viewPrefab, _canvas.transform);
            BasicView view = viewInstance.GetComponent<BasicView>();
            view.Show();
            _openedViews.Add(view.ViewType, view);
            _cashedViews.Add(view.ViewType, view);
            
            OnViewShowed?.Invoke(view.ViewType);
            
            if(_onLoadedActions.ContainsKey(view.ViewType))
            {
                _onLoadedActions[view.ViewType]?.Invoke();
                _onLoadedActions.Remove(view.ViewType);
            }
        }

        public void HideView(ViewType view)
        {
            _openedViews.TryGetValue(view, out BasicView viewToRemove);
            if (viewToRemove != null)
            {
                viewToRemove.Hide();
                _openedViews.Remove(view);
            }
        }
        
        public T GetOpenedView<T>() where T : BasicView
        {
            return (T)_openedViews.Values.FirstOrDefault(x => x is T);
        }

        /// <summary>
        /// Check if the click is handled by any of the opened views
        /// </summary>
        public bool IsClickHandled(Vector3 screenPosition)
        {
            if (_openedViews == null)
            {
                return false;
            }

            if (_openedViews.Count == 0)
            {
                return false;
            }

            GraphicRaycaster raycaster = _canvas.GetComponent<GraphicRaycaster>();
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);
            if (results.Count > 0)
            {
                return true;
            }


            return false;
        }

        public void SetResourcesService(ResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }
    }
}
