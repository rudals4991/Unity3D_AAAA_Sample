using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TilePool : MonoBehaviour
{
    [System.Serializable]
    public class TilePrefabs
    {
        public TileType type;
        public GameObject prefab;
        public int ID;
    }
    [SerializeField] List<TilePrefabs> prefabList = new();

    Dictionary<TileType, Dictionary<int, Queue<GameObject>>> poolDic = new();
    Dictionary<TileType, List<TilePrefabs>> prefabDic = new();

    void Awake()
    {
        DIContainer.Register(this);

        foreach (TileType type in Enum.GetValues(typeof(TileType)))
        {
            prefabDic[type] = new List<TilePrefabs>();
            poolDic[type] = new Dictionary<int, Queue<GameObject>>();
        }
        foreach (var prefab in prefabList)
        {
            prefabDic[prefab.type].Add(prefab);
        }
    }
    public GameObject GetByRandom(TileType type, Vector3 pos, Quaternion rot)
    {
        if (!prefabDic.TryGetValue(type, out var candidate) || candidate.Count == 0) return null;
        TilePrefabs selected = candidate[UnityEngine.Random.Range(0, candidate.Count)];
        int id = selected.ID;
        if (!poolDic[type].TryGetValue(id, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            poolDic[type][id] = pool;
        }
        GameObject obj;
        if (pool.Count > 0) obj = pool.Dequeue();
        else
        {
            obj = Instantiate(selected.prefab, transform);
            if (!obj.TryGetComponent(out Tile tile))
            {
                Destroy(obj);
                return null;
            }
            tile.SetType(selected.type);
            tile.SetID(id);
        }
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj)
    {
        if (!obj.TryGetComponent(out Tile tile))
        { 
            Destroy(obj); 
            return;
        }
        if (!poolDic.TryGetValue(tile.MyType, out var idDic))
        {
            idDic = new Dictionary<int, Queue<GameObject>>();
            poolDic[tile.MyType] = idDic;
        }
        if (!idDic.TryGetValue(tile.MyID, out var pool))
        {
            pool = new Queue<GameObject>();
            idDic[tile.MyID] = pool;
        }
        pool.Enqueue(obj);
    }
    public void ReleaseAll()
    {
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            if (!obj.activeSelf) continue;
            if (obj.TryGetComponent(out Tile tile))
            {
                obj.SetActive(false);
                if (!poolDic.TryGetValue(tile.MyType, out var idDic))
                {
                    idDic = new Dictionary<int, Queue<GameObject>>();
                    poolDic[tile.MyType] = idDic;
                }
                if (!idDic.TryGetValue(tile.MyID, out var pool))
                {
                    pool = new Queue<GameObject>();
                    idDic[tile.MyID] = pool;
                }
                pool.Enqueue(obj);
            }
            else Destroy(obj);
        }
    }
}
