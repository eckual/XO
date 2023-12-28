using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    public enum ObjectType
    {
        X = 1,
        O = 2  
    }

    [Serializable] private struct PoolInfo
    {
        public ObjectType Type;
        public int Amount;
        public PoolableObject Prefab;
    }

    [SerializeField] private List<PoolInfo> poolInfos;
    private List<PoolableObject> _currentObjects;

    private void OnDestroy()
    {
        poolInfos = null;
        _currentObjects = null;
    }

    private void Awake()
    {
        _currentObjects = new List<PoolableObject>();
        
        for (var i = 0; i < poolInfos.Count; i++)
        {
            for (var j = 0; j < poolInfos[i].Amount; j++)
            {
                var item = Instantiate(poolInfos[i].Prefab, transform);
                item.Initialise(poolInfos[i].Type.ToString());
                _currentObjects.Add(item);
            }
        }
    }

    public PoolableObject SpawnItem(ObjectType inType)
    {
        var poolItem = _currentObjects.FirstOrDefault(item=> item.Type.ToString() == inType.ToString());
        if (poolItem == null) return null;
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
