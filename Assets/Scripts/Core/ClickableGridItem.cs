using System;
using UnityEngine;
using UnityEngine.UI;

public class ClickableGridItem : MonoBehaviour
{
    public event Action<ClickableGridItem> OnClick;
    [SerializeField] private Button button;

    private RectTransform _rt;

    public RectTransform RectTransform
    {
        get
        {
            if (_rt == null) _rt = GetComponent<RectTransform>();
            return _rt;
        }
    }
    public void Initialise()
    {
        button.onClick.AddListener(()=> OnClick?.Invoke(this));
    }
    

    private void OnDestroy()
    {
        OnClick = null;
    }

    public void DisableButton()
    {
        button.interactable = false;
    }
}
