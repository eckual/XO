using Scripts;
using UnityEngine;
using UnityEngine.UI;

public class PoolableObject : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private ObjectType type;

    private RectTransform _rt;
    public ObjectType Type => type;
    
    public RectTransform RectTransform
    {
        get
        {
            if (_rt == null) _rt = GetComponent<RectTransform>();
            return _rt;
        }
    }

    private void OnDestroy()
    {
        _rt = null;
        text = null;
    }

    public void Initialise(string inText)
    {
        text.text = inText;
        gameObject.SetActive(false);
    }
    
}
