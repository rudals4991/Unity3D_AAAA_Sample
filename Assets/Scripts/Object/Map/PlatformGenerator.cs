using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    PoolManager poolManager;

    [Header("Spawn Settings")]
    [SerializeField] int platformCount = 30;
    [SerializeField] Vector3 boxSize = new Vector3(4f, 2f, 4f);
    [SerializeField] float verticalOffset = 2f;
    void Awake()
    {
        DIContainer.Register(this);
    }
    public void Initialize(PoolManager pool)
    { 
        poolManager = pool;
    }
    public (List<GameObject> objects, Vector3 endPos) Generate(Vector3 startPos, Vector3 dir)
    {
        List<GameObject> objs = new();
        PlatformType type = ResolvePlatformType(dir);
        GameObject first = poolManager.GetPlatform(type, startPos, Quaternion.identity);
        objs.Add(first);
        Vector3 lastPos = startPos;
        for (int i = 1; i < platformCount; i++)
        {
            Vector3 newPos = GetRandomPosition(type, lastPos);
            GameObject platform = poolManager.GetPlatform(type, newPos, Quaternion.identity);
            objs.Add(platform);
            lastPos = newPos;
        }
        GameObject trigger = poolManager.GetPlatform(PlatformType.Trigger, lastPos, Quaternion.identity);
        if (trigger.TryGetComponent(out TriggerPlatform tp)) tp.ResetFlag();
        objs.Add(trigger);
        return (objs, trigger.transform.position);
    }
    PlatformType ResolvePlatformType(Vector3 dir)
    {
        if (dir == Vector3.up) return PlatformType.ForJump;
        if (dir == Vector3.down) return PlatformType.Normal;
        return PlatformType.Normal;
    }
    Vector3 GetRandomPosition(PlatformType type, Vector3 basePos)
    {
        Vector3 half = boxSize / 2f;
        float x = Random.Range(-half.x, half.x);
        float z = Random.Range(-half.z, half.z);
        float y = Random.Range(-half.y, half.y);
        float centerY = type == PlatformType.ForJump ? verticalOffset : -verticalOffset;
        return basePos + new Vector3(x, centerY + y, z);
    }
}
