using UnityEngine;
using UnityEngine.UI;

public class PoolableObject : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private PoolSystem.ObjectType type;

    private RectTransform _rt;

    public RectTransform RectTransform
    {
        get
        {
            if (_rt == null) _rt = GetComponent<RectTransform>();
            return _rt;
        }
    }
    
    public PoolSystem.ObjectType Type => type;

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
