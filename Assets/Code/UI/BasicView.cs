using System.Collections;
using System.Collections.Generic;
using Example.Visual.UI;
using UnityEngine;

namespace Example.Visual.UI
{
    public class BasicView : MonoBehaviour
    {
        [SerializeField] private ViewType _viewType;

        public ViewType ViewType => _viewType;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public virtual void SetData(UIData data)
        {
            
        }
    }
    
    public class UIData
    {
        
    }
}