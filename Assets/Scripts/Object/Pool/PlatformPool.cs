using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{
    [System.Serializable]
    public class PlatformPrefabs
    {
        public PlatformType type;
        public GameObject prefab;
    }
    [SerializeField] List<PlatformPrefabs> prefabList = new();

    Dictionary<PlatformType, Queue<GameObject>> poolDic = new();
    Dictionary<PlatformType, List<GameObject>> prefabDic = new();

    void Awake()
    {
        DIContainer.Register(this);

        foreach (PlatformType type in System.Enum.GetValues(typeof(PlatformType)))
        {
            prefabDic[type] = new List<GameObject>();
            poolDic[type] = new Queue<GameObject>();
        }
        foreach (var p in prefabList)
        {
            prefabDic[p.type].Add(p.prefab);
        }
    }
    public GameObject GetByRandom(PlatformType type, Vector3 pos, Quaternion rot)
    {
        if (prefabDic[type].Count == 0) return null;
        GameObject selected = prefabDic[type][Random.Range(0, prefabDic[type].Count)];
        Queue<GameObject> q = poolDic[type];
        GameObject obj;
        if (q.Count > 0) obj = q.Dequeue();
        else
        {
            obj = Instantiate(selected, transform);
            obj.GetComponent<Platform>().SetType(type);
        }
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        return obj;
    }
    public void Release(GameObject obj)
    {
        if (!obj.TryGetComponent(out Platform plat))
        {
            Destroy(obj);
            return;
        }
        obj.SetActive(false);
        poolDic[plat.MyType].Enqueue(obj);
    }
    public void ReleaseAll()
    {
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            if (obj.activeSelf && child.TryGetComponent(out Platform platform))
            {
                obj.SetActive(false);
                poolDic[platform.MyType].Enqueue(obj);
            }
        }
    }
}
