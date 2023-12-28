using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<ClickableGridItem> gridItems;
    [SerializeField] private PoolSystem pool;
    [SerializeField] private Transform gridTransform;
    
    private PoolSystem.ObjectType _currentType;
    private bool _hasGameStarted;
    private void OnDestroy()
    {
        gridItems = null;
        pool = null;
        gridTransform = null;
    }

    private void Awake()
    {
        foreach (var btn in gridItems)
        {
            btn.OnClick += BtnClicked;
            btn.Initialise();
        }
    }

    private void BtnClicked(ClickableGridItem gridItem)
    {
        gridItem.DisableButton();

        if (_hasGameStarted)
            _currentType = (_currentType != PoolSystem.ObjectType.X) ? PoolSystem.ObjectType.X : PoolSystem.ObjectType.O;

        else
        {
            _currentType = PoolSystem.ObjectType.X;
            _hasGameStarted = true;
        }

        var item = pool.SpawnItem(_currentType);
        item.RectTransform.SetParent(gridTransform);
        item.RectTransform.anchoredPosition = Vector2.zero;

        var itemScale = item.RectTransform.localScale;
        item.RectTransform.localScale = new Vector2(itemScale.x * 2, itemScale.y * 2);
        item.RectTransform.DOScale(Vector3.one, 1f).Play();
        item.RectTransform.DOAnchorPos(gridItem.RectTransform.anchoredPosition, 0.5f).Play();
    }
    
}
