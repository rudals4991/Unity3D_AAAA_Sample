using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("Spawn Setting")]
    [SerializeField] float tileLength = 50f;
    [SerializeField] float linearTileCount = 3;

    PoolManager poolManager;
    TileType currentLinearType;

    void Awake()
    {
        DIContainer.Register(this);
    }
    public void Initialize(PoolManager pool)
    {
        poolManager = pool;
    }
    public void SetLinearType(TileType type)
    {
        currentLinearType = type;
    }
    public (List<GameObject> objects, Vector3 endPos) Generate(Vector3 startPos, Vector3 dir)
    {
        if (poolManager == null)
        {
            // 여기 로그가 뜨면 “Initialize가 안 됐다”가 확정입니다.
            Debug.LogError("[TileGenerator] poolManager is null. Did you call TileGenerator.Initialize(pool) after GamePlay scene loaded?");
            return (new List<GameObject>(), startPos);
        }
        List<GameObject> objs = new();
        Vector3 pos = startPos;
        for (int i = 0; i < linearTileCount; i++)
        {
            GameObject tile = poolManager.GetTile(currentLinearType, pos, Quaternion.identity);
            if (tile == null)
            {
                Debug.LogError($"[TileGenerator] poolManager.GetTile returned null. type={currentLinearType}");
                continue;
            }
            objs.Add(tile);
            pos += dir * tileLength;
        }
        GameObject trigger = poolManager.GetTile(TileType.Trigger, pos, Quaternion.identity);
        if (trigger != null)
        {
            objs.Add(trigger);
        }
        else
        {
            Debug.LogError("[TileGenerator] Trigger tile spawn returned null.");
        }
        Vector3 endPos = pos + dir * tileLength;
        return (objs, endPos);
    }
}
