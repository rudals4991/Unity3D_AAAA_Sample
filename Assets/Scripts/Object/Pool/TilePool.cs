using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    [System.Serializable]
    public class TilePrefabPair
    {
        public TileType type;
        public GameObject prefab;
        public int poolSize = 10;
    }
    [SerializeField] List<TilePrefabPair> prefabList = new();
    Dictionary<TileType, Queue<GameObject>> poolDic = new();
    Dictionary<TileType, GameObject> prefabDic = new();
    void Awake()
    {
        DIContainer.Register(this as TilePool);
    }
    public void Initialize()
    {
        foreach (var prefab in prefabList)
        {
            prefabDic[prefab.type] = prefab.prefab;
            Queue<GameObject> q = new();
            poolDic[prefab.type] = q;
            for (int i = 0; i < prefab.poolSize; i++)
            {
                GameObject obj = Instantiate(prefab.prefab, transform);
                obj.SetActive(false);
                q.Enqueue(obj);
            }
        }
    }
    public GameObject Get(TileType type, Vector3 pos, Quaternion rot)
    {
        if (!poolDic.ContainsKey(type)) return null;
        Queue<GameObject> queue = poolDic[type];
        GameObject obj = queue.Count > 0 ? queue.Dequeue() : Instantiate(prefabDic[type], transform);
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        return obj;
    }

    public void Release(TileType type, GameObject obj)
    {
        obj.SetActive(false);
        if (!poolDic.ContainsKey(type))
        {
            Destroy(obj);
            return;
        }
        poolDic[type].Enqueue(obj);
    }
    public void ReleaseAll()
    {
        foreach (var pool in poolDic)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                    pool.Value.Enqueue(child.gameObject);
                }
            }
        }
    }
}
