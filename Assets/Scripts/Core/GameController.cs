using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private event Action<PoolSystem.ObjectType> OnCheckGameFinished;
    private static GameController _instance;
    
    [SerializeField] private List<ClickableGridItem> gridItems;
    [SerializeField] private PoolSystem pool;
    [SerializeField] private Transform gridTransform;
    [SerializeField] private Text gameFinishedMsg;
    
    private PoolSystem.ObjectType _currentType;
    private bool _hasGameStarted;
    
    private void OnDestroy()
    {
        OnCheckGameFinished = null;
        _instance = null;
        gridItems = null;
        pool = null;
        gridTransform = null;
        gameFinishedMsg = null;
    }

    private void Start()
    {
        if (_instance != null) return;
        _instance = this;
        
        gameFinishedMsg.gameObject.SetActive(false);
        foreach (var btn in gridItems)
        {
            btn.OnClick += BtnClicked;
            btn.Initialise();
        }

        OnCheckGameFinished += CheckGameFinish;
    }

    private void CheckGameFinish(PoolSystem.ObjectType type)
    {
        // Check rows
        for (var i = 0; i < 3; i++)
        {
            if (!AreAllEqual(gridItems[i * 3], gridItems[i * 3 + 1], gridItems[i * 3 + 2])) continue;
            gameFinishedMsg.gameObject.SetActive(true);
            gameFinishedMsg.text += $"  winner is : {type.ToString()}";
            return;
        }

        // Check columns
        for (var i = 0; i < 3; i++)
        {
            if (!AreAllEqual(gridItems[i], gridItems[i + 3], gridItems[i + 6])) continue;
            gameFinishedMsg.gameObject.SetActive(true);
            gameFinishedMsg.text += $"  winner is : {type.ToString()}";
            return;
        }

        // Check diagonals
        if (AreAllEqual(gridItems[0], gridItems[4], gridItems[8]))
        {
            gameFinishedMsg.gameObject.SetActive(true);
            gameFinishedMsg.text += $"  winner is : {type.ToString()}";
            return;
        }

        if (!AreAllEqual(gridItems[2], gridItems[4], gridItems[6])) return;
        gameFinishedMsg.gameObject.SetActive(true);
        gameFinishedMsg.text += $"  winner is : {type.ToString()}";
    }

    private bool AreAllEqual(ClickableGridItem item1, ClickableGridItem item2, ClickableGridItem item3)
    {
        return item1.ItemValue.Equals(item2.ItemValue) && item2.ItemValue.Equals(item3.ItemValue) && item1.ItemValue != string.Empty;
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
                gridItem.ItemValue = _currentType.ToString();
                OnCheckGameFinished?.Invoke(_currentType);
                return;
            }

            _currentType = PoolSystem.ObjectType.X;
            AnimateItem(_currentType, gridItem.RectTransform.anchoredPosition);
            gridItem.ItemValue = _currentType.ToString();
            OnCheckGameFinished?.Invoke(_currentType);
            return;
        }
        
        _currentType = PoolSystem.ObjectType.X;
        AnimateItem(_currentType, gridItem.RectTransform.anchoredPosition);
        gridItem.ItemValue = _currentType.ToString();
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
