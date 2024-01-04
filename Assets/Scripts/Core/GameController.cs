using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts
{
    public class GameController : MonoBehaviour
    {
        private static GameController _instance;
        private event Action OnCheckGameFinished;

        [SerializeField] private List<ClickableGridItem> gridItems;
        [SerializeField] private PoolSystem pool;
        [SerializeField] private Transform gridTransform;
        [SerializeField] private GameObject grid;
        [SerializeField] private RectTransform popUp;
        [SerializeField] private Text winnerText;
        [SerializeField] private Button replayBtn;

        private WaitForSeconds _waitForSeconds;
        private ObjectType _currentType;
        private bool _hasGameStarted;

        private void OnDestroy()
        {
            OnCheckGameFinished = null;
            _instance = null;
            gridItems = null;
            pool = null;
            gridTransform = null;
            winnerText = null;
            replayBtn = null;
        }

        private void Start()
        {
            if (_instance != null) return;
            _instance = this;

            _waitForSeconds = new WaitForSeconds(0.6f);
            foreach (var btn in gridItems)
            {
                btn.OnClick += BtnClicked;
                btn.Initialise();
            }

            OnCheckGameFinished += IsGameFinished;
            replayBtn.onClick.AddListener(()=>
            {
                for (var i = 0; i < gridItems.Count; i++)
                {
                    var item = gridItems[i];
                    item.SetValue(string.Empty);
                }

                popUp.gameObject.SetActive(false);
                _hasGameStarted = false;
                grid.SetActive(true);
                SceneManager.LoadScene(GameConstants.IntroSceneName);
            });
        }

        private void IsGameFinished()
        {
            StartCoroutine(GameFinished());
            return;

            IEnumerator GameFinished()
            {
                // Check rows
                for (var i = 0; i < 3; i++)
                {
                    if (!AreAllEqual(gridItems[i * 3], gridItems[i * 3 + 1], gridItems[i * 3 + 2])) continue;
                    yield return _waitForSeconds;
                    grid.SetActive(false);
                    winnerText.text = gridItems[i * 3].ItemValue;
                    popUp.gameObject.SetActive(true);
                    yield break;
                }

                // Check columns
                for (var i = 0; i < 3; i++)
                {
                    if (!AreAllEqual(gridItems[i], gridItems[i + 3], gridItems[i + 6])) continue;
                    
                    yield return _waitForSeconds;
                    grid.SetActive(false);
                    winnerText.text = gridItems[i].ItemValue;
                    popUp.gameObject.SetActive(true);
                    yield break;
                }

                // Check diagonals
                if (AreAllEqual(gridItems[0], gridItems[4], gridItems[8]))
                {
                    yield return _waitForSeconds;
                    grid.SetActive(false);
                    winnerText.text = gridItems[0].ItemValue;
                    popUp.gameObject.SetActive(true);
                    yield break;
                }

                if (!AreAllEqual(gridItems[2], gridItems[4], gridItems[6])) yield break;
                
                yield return _waitForSeconds;
                grid.SetActive(false);
                winnerText.text = gridItems[2].ItemValue;
                popUp.gameObject.SetActive(true);
            }
        }

        private bool AreAllEqual(ClickableGridItem item1, ClickableGridItem item2, ClickableGridItem item3)
        {
            if (item1.ItemValue.IsNullorEmpty() || item2.ItemValue.IsNullorEmpty() || item3.ItemValue.IsNullorEmpty()) return false;
            
            return item1.ItemValue.Equals(item2.ItemValue) && item2.ItemValue.Equals(item3.ItemValue) &&
                   item1.ItemValue != string.Empty;
        }

        private void BtnClicked(ClickableGridItem gridItem)
        {
            gridItem.DisableButton();

            if (_hasGameStarted)
            {
                if (_currentType == ObjectType.X)
                {
                    _currentType = ObjectType.O;
                    AnimateItem(_currentType, gridItem.RectTransform.anchoredPosition);
                    gridItem.SetValue(_currentType.ToString());
                    OnCheckGameFinished?.Invoke();
                    return;
                }

                _currentType = ObjectType.X;
                AnimateItem(_currentType, gridItem.RectTransform.anchoredPosition);
                gridItem.SetValue(_currentType.ToString());
                OnCheckGameFinished?.Invoke();
                return;
            }

            _currentType = ObjectType.X;
            AnimateItem(_currentType, gridItem.RectTransform.anchoredPosition);
            gridItem.SetValue(_currentType.ToString());
            _hasGameStarted = true;
        }

        private void AnimateItem(ObjectType type, Vector3 pos)
        {
            var item = pool.SpawnItem(type);
            item.RectTransform.SetParent(gridTransform);
            item.RectTransform.anchoredPosition = Vector2.zero;
            var scale = item.RectTransform.localScale;
            item.RectTransform.localScale = new Vector2(scale.x * 2.8f, scale.y * 2.8f);
            item.RectTransform.DOScale(Vector3.one, 0.6f).Play();
            item.RectTransform.DOAnchorPos(pos, 0.25f).Play();
        }
        
    }
}