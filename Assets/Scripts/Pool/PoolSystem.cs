using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public enum ObjectType
    {
        X = 1,
        O = 2
    }

    public class PoolSystem : MonoBehaviour
    {
        [Serializable]
        private struct PoolInfo
        {
            public ObjectType type;
            public int amount;
            public PoolableObject prefab;
        }

        private static PoolSystem _instance;

        [SerializeField] private List<PoolInfo> poolInfos;
        private List<PoolableObject> _currentObjects;

        private void OnDestroy()
        {
            _instance = null;
            poolInfos = null;
            _currentObjects = null;
        }

        private void Awake()
        {
            if (_instance != null) return;
            _instance = this;

            _currentObjects = new List<PoolableObject>();

            for (var i = 0; i < poolInfos.Count; i++)
            {
                var poolInfo = poolInfos[i];
                for (var j = 0; j < poolInfo.amount; j++)
                {
                    var item = Instantiate(poolInfo.prefab, transform);
                    item.Initialise(poolInfo.type.ToString());
                    _currentObjects.Add(item);
                }
            }
        }

        public PoolableObject SpawnItem(ObjectType inType)
        {
            var poolItem = _currentObjects.FirstOrDefault(item => item.Type == inType);
            if (poolItem == null)
            {
                throw new NullReferenceException($"pool item type of {inType} is null");
            }

            _currentObjects.Remove(poolItem);
            poolItem.gameObject.SetActive(true);
            return poolItem;
        }

        public void DespawnItem(PoolableObject item)
        {
            item.RectTransform.SetParent(transform);
            item.gameObject.SetActive(false);
            _currentObjects.Add(item);
        }
    }
}