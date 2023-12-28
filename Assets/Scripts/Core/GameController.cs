using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    
    [SerializeField] private List<ClickableGridItem> gridItems;
    [SerializeField] private PoolSystem pool;
    [SerializeField] private Transform gridTransform;
    
    private PoolSystem.ObjectType _currentType;
    private bool _hasGameStarted;
    private void OnDestroy()
    {
        _instance = null;
        gridItems = null;
        pool = null;
        gridTransform = null;
    }

    private void Start()
    {
        if (_instance != null) return;
        _instance = this;
        
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
        {
            if (_currentType == PoolSystem.ObjectType.X)
            {
                _currentType = PoolSystem.ObjectType.O;
                AnimateItem(_currentType, gridItem.RectTransform.anchoredPosition);
                return;
            }

            _currentType = PoolSystem.ObjectType.X;
            AnimateItem(_currentType, gridItem.RectTransform.anchoredPosition);
            return;
        }
        
        _currentType = PoolSystem.ObjectType.X;
        AnimateItem(_currentType, gridItem.RectTransform.anchoredPosition);
        _hasGameStarted = true;
    }

    private void AnimateItem(PoolSystem.ObjectType type, Vector3 pos)
    {
        var item = pool.SpawnItem(type);
        item.RectTransform.SetParent(gridTransform);
        item.RectTransform.anchoredPosition = Vector2.zero;
        var scale = item.RectTransform.localScale;
        item.RectTransform.localScale = new Vector2(scale.x * 2, scale.y * 2);
        item.RectTransform.DOScale(Vector3.one, 1f).Play();
        item.RectTransform.DOAnchorPos(pos, 0.5f).Play();
    }
}
