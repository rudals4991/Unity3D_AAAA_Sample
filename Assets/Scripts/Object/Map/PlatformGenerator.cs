using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    PoolManager poolManager;
    [Header("Counts")]
    [SerializeField] int platformCount = 30;

    [Header("Platform")]
    [SerializeField] Vector3 platformBoxSize = new Vector3(4f, 2f, 4f); 
    float overlapPadding = 0.2f;                        
    int maxAttemptsPerPlatform = 40;                     

    [Header("Background")]
    [SerializeField] GameObject backgroundPrefab;
    [SerializeField] Vector3 backgroundEuler = new Vector3(90f, 180f, 0f);
    [SerializeField] Vector3 backgroundScale = new Vector3(4f, 1f, 10f); 
    Vector3 fallbackBackgroundSize = new Vector3(40f, 100f, 1f); 

    float edgePadding = 2f;
    bool reuseBackground = true;
    float yJitter = 0.5f;                            
    float xJitter = 1.5f;                           
    float maxHalfWidth = 12f;
    float playerStartYOffset = -2f;
    float backgroundGap = 0.05f;  
    bool useFixedPlaneBounds = true; 
    bool lockZ = true;                          
    GameObject backgroundInstance;
    void Awake()
    {
        DIContainer.Register(this);
    }
    public void Initialize(PoolManager pool)
    { 
        poolManager = pool;
    }
    public void ClearBackground()
    {
        if (backgroundInstance != null)
        {
            Destroy(backgroundInstance);
            backgroundInstance = null;
        }
    }
    public (List<GameObject> objects, Vector3 endPos) Generate(Vector3 startPos, Vector3 dir)
    {
        if (dir == Vector3.up || dir == Vector3.down) return GenerateVertical(startPos, dir);
        return GenerateLegacyRandom(startPos, dir);
    }
    (List<GameObject> objects, Vector3 endPos) GenerateVertical(Vector3 anchorPos, Vector3 dir)
    {
        List<GameObject> objs = new();
        List<Bounds> placed = new();
        Bounds bgBounds = EnsureBackgroundAndGetBounds(anchorPos, dir);

        float minX = bgBounds.min.x + edgePadding;
        float maxX = bgBounds.max.x - edgePadding;
        float minY = bgBounds.min.y + edgePadding;
        float maxY = bgBounds.max.y - edgePadding;

        float centerX = bgBounds.center.x;
        float halfWidth = Mathf.Min((maxX - minX) * 0.5f, maxHalfWidth);
        minX = centerX - halfWidth;
        maxX = centerX + halfWidth;

        float z = lockZ ? anchorPos.z : Mathf.Clamp(anchorPos.z, bgBounds.min.z + edgePadding, bgBounds.max.z - edgePadding);

        Vector3 firstPos = new Vector3(
            Mathf.Clamp(anchorPos.x, minX, maxX),
            Mathf.Clamp(anchorPos.y + playerStartYOffset, minY, maxY),
            z
        );

        PlatformType type = ResolvePlatformType(dir);

        GameObject first = poolManager.GetPlatform(type, firstPos, Quaternion.identity);
        objs.Add(first);
        placed.Add(MakeBounds(firstPos, platformBoxSize, overlapPadding));

        float endY = (dir == Vector3.up) ? maxY : minY;
        int remainCount = Mathf.Max(0, platformCount - 1);
        float startY = firstPos.y;
        float step = (remainCount <= 1) ? 0f : (endY - startY) / remainCount;

        for (int i = 1; i <= remainCount; i++)
        {
            float baseY = startY + step * i;
            baseY = Mathf.Clamp(baseY, minY, maxY);

            Vector3 pos = FindNonOverlappingPosition(baseY, minX, maxX, z, placed);
            GameObject p = poolManager.GetPlatform(type, pos, Quaternion.identity);
            objs.Add(p);
            placed.Add(MakeBounds(pos, platformBoxSize, overlapPadding));
        }
        Vector3 endPos = objs[objs.Count - 1].transform.position;
        return (objs, endPos);
    }

    Vector3 FindNonOverlappingPosition(float baseY, float minX, float maxX, float z, List<Bounds> placed)
    {
        for (int t = 0; t < maxAttemptsPerPlatform; t++)
        {
            float x = Mathf.Lerp(minX, maxX, Random.value) + Random.Range(-xJitter, xJitter);
            float y = baseY + Random.Range(-yJitter, yJitter);
            Vector3 candidate = new Vector3(x, y, z);
            Bounds b = MakeBounds(candidate, platformBoxSize, overlapPadding);
            if (!IntersectsAny(b, placed)) return candidate;
        }
        return new Vector3((minX + maxX) * 0.5f, baseY, z);
    }

    bool IntersectsAny(Bounds b, List<Bounds> placed)
    {
        for (int i = 0; i < placed.Count; i++)
        {
            if (placed[i].Intersects(b)) return true;
        }
        return false;
    }

    Bounds MakeBounds(Vector3 center, Vector3 size, float pad)
    {
        Vector3 paddedSize = size + Vector3.one * pad;
        return new Bounds(center, paddedSize);
    }

    Bounds EnsureBackgroundAndGetBounds(Vector3 anchorPos, Vector3 dir)
    {
        Vector3 spawnCenter = new Vector3(anchorPos.x, anchorPos.y, anchorPos.z);
        if (backgroundPrefab != null)
        {
            if (!reuseBackground || backgroundInstance == null)
            {
                if (!reuseBackground) ClearBackground();
                if (backgroundInstance == null)
                    backgroundInstance = Instantiate(backgroundPrefab, spawnCenter, Quaternion.identity);
            }
            backgroundInstance.transform.rotation = Quaternion.Euler(backgroundEuler);
            backgroundInstance.transform.localScale = backgroundScale;
            float push = (platformBoxSize.z * 0.5f) + backgroundGap;
            Camera cam = Camera.main;
            if (cam != null)
            {
                Vector3 camPos = cam.transform.position;
                Vector3 viewDir = (spawnCenter - camPos).normalized;
                Vector3 bgPos = spawnCenter + viewDir * push;
                backgroundInstance.transform.position = bgPos;
            }
            else
            {
                backgroundInstance.transform.position = spawnCenter - backgroundInstance.transform.forward * push;
            }
            if (useFixedPlaneBounds)
            {
                float width = 10f * backgroundInstance.transform.lossyScale.x; 
                float height = 10f * backgroundInstance.transform.lossyScale.z; 
                float depth = 1f;
                return new Bounds(spawnCenter, new Vector3(width, height, depth));
            }
            if (backgroundInstance.TryGetComponent<BoxCollider>(out var box)) return box.bounds;
            var r = backgroundInstance.GetComponentInChildren<Renderer>();
            if (r != null) return r.bounds;
        }
        backgroundInstance = null;
        return new Bounds(spawnCenter, fallbackBackgroundSize);
    }

    PlatformType ResolvePlatformType(Vector3 dir)
    {
        if (dir == Vector3.up) return PlatformType.ForJump;
        if (dir == Vector3.down) return PlatformType.Normal;
        return PlatformType.Normal;
    }

    (List<GameObject> objects, Vector3 endPos) GenerateLegacyRandom(Vector3 startPos, Vector3 dir)
    {
        List<GameObject> objs = new();
        PlatformType type = ResolvePlatformType(dir);

        GameObject first = poolManager.GetPlatform(type, startPos, Quaternion.identity);
        objs.Add(first);

        Vector3 lastPos = startPos;
        Vector3 boxSize = platformBoxSize;
        Vector3 half = boxSize / 2f;

        for (int i = 1; i < platformCount; i++)
        {
            float x = Random.Range(-half.x, half.x);
            float z = Random.Range(-half.z, half.z);
            float y = Random.Range(-half.y, half.y);

            float centerY = (type == PlatformType.ForJump) ? 2f : -2f;
            Vector3 newPos = lastPos + new Vector3(x, centerY + y, z);

            GameObject p = poolManager.GetPlatform(type, newPos, Quaternion.identity);
            objs.Add(p);
            lastPos = newPos;
        }
        return (objs, lastPos);
    }
}
