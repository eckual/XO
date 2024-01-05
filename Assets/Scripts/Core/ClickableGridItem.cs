using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ClickableGridItem : MonoBehaviour
    {
        public event Action<ClickableGridItem> OnClick;
        [SerializeField] private Button button;

        private RectTransform _rt;
        public string ItemValue { get; private set; }

        private void OnDestroy()
        {
            _rt = null;
            button = null;
            ItemValue = null;
            OnClick = null;
        }

        public RectTransform RectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }

        public void Initialise() => button.onClick.AddListener(() => OnClick?.Invoke(this));
        
        public void DisableButton() => button.interactable = false;
        
        public void SetValue(string inValue) => ItemValue = inValue;

    }
}
