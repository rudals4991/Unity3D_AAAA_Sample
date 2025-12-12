using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    [System.Serializable]
    public class TilePrefabs
    {
        public TileType type;
        public GameObject prefab;
    }
    [SerializeField] List<TilePrefabs> prefabList = new();

    Dictionary<TileType, Queue<GameObject>> poolDic = new();
    Dictionary<TileType, List<GameObject>> prefabDic = new();

    void Awake()
    {
        DIContainer.Register(this);

        foreach (TileType type in System.Enum.GetValues(typeof(TileType)))
        {
            prefabDic[type] = new List<GameObject>();
            poolDic[type] = new Queue<GameObject>();
        }
        foreach (var prefab in prefabList)
        {
            prefabDic[prefab.type].Add(prefab.prefab);
        }
    }
    public GameObject GetByRandom(TileType type, Vector3 pos, Quaternion rot)
    {
        if (prefabDic[type].Count == 0) return null;
        GameObject selected = prefabDic[type][Random.Range(0, prefabDic[type].Count)];
        Queue<GameObject> q = poolDic[type];
        GameObject obj = null;
        if (q.Count > 0) obj = q.Dequeue();
        else
        {
            obj = Instantiate(selected, transform);
            obj.GetComponent<Tile>().SetType(type);
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
        obj.SetActive(false);
        poolDic[tile.MyType].Enqueue(obj);
    }
    public void ReleaseAll()
    {
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            
            if (obj.activeSelf && child.TryGetComponent(out Tile tile))
            {
                obj.SetActive(false);
                poolDic[tile.MyType].Enqueue(obj);
            }
        }
    }
}
