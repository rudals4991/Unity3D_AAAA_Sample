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
        List<GameObject> objs = new();
        Vector3 pos = startPos;
        for (int i = 0; i < linearTileCount; i++)
        {
            GameObject tile = poolManager.GetTile(currentLinearType, pos, Quaternion.identity);
            objs.Add(tile);
            pos += dir * tileLength;
        }
        GameObject trigger = poolManager.GetTile(TileType.Trigger, pos, Quaternion.identity);
        if (trigger.TryGetComponent(out TriggerTile tt)) tt.ResetFlag();
        objs.Add(trigger);
        Vector3 endPos = pos + dir * tileLength;
        return (objs, trigger.transform.position);
    }
}
