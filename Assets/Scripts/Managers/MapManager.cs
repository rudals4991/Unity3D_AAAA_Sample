using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour,IManagerBase
{
    PlatformGenerator platform;
    TileGenerator tile;
    PoolManager pool;
    public int Priority => 6;

    Vector3 nextSpawnPos = Vector3.zero;
    Vector3 currentDirection = Vector3.forward; 
    List<MapSegment> activeSegments = new();
    bool isInitialized;

    public void Exit()
    {
        ClearAll();
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        pool = DIContainer.Resolve<PoolManager>();
        platform = DIContainer.Resolve<PlatformGenerator>();
        tile = DIContainer.Resolve<TileGenerator>();
    }

    public void PrepareForMode(GameMode mode)
    {
        currentDirection = ResolveDirection(mode);

        if (!isInitialized)
            InitializeGenerators();

        GenerateNextSegment(mode);
    }
    Vector3 ResolveDirection(GameMode mode)
    {
        return mode switch
        {
            GameMode.BackView_ToForward => Vector3.forward,
            GameMode.SideView_ToRight => Vector3.forward,
            GameMode.SideView_ToTop => Vector3.up,
            GameMode.SideView_ToDown => Vector3.down,
            _ => Vector3.forward
        };
    }
    void InitializeGenerators()
    {
        if (platform == null) platform = DIContainer.Resolve<PlatformGenerator>();
        if(tile == null) tile = DIContainer.Resolve<TileGenerator>();
        platform.Initialize(pool);
        tile.Initialize(pool);
        isInitialized = true;
    }
    public void GenerateNextSegment(GameMode mode)
    {
        List<GameObject> objs;
        float startAxis = GetAxisValue(nextSpawnPos);
        float endAxis = startAxis;
        if (currentDirection == Vector3.forward)
        {
            if (mode == GameMode.BackView_ToForward) tile.SetLinearType(TileType.Linear_ToForward);
            else tile.SetLinearType(TileType.Linear_ToRight);
            var result = tile.Generate(nextSpawnPos, currentDirection);
            objs = result.objects;
            nextSpawnPos = result.endPos;
            endAxis = GetAxisValue(result.endPos);
        }
        else
        {
            var result = platform.Generate(nextSpawnPos, currentDirection);
            objs = result.objects;
            nextSpawnPos = result.endPos;
            endAxis = GetAxisValue(result.endPos);
        }
        activeSegments.Add(new MapSegment(objs, startAxis, endAxis));
        CleanupOldSegments();
    }
    void CleanupOldSegments()
    {
        float playerAxis = GetPlayerAxis();
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            if (playerAxis - activeSegments[i].endAxis > 80f) 
            {
                ReleaseSegment(activeSegments[i]);
                activeSegments.RemoveAt(i);
            }
        }
    }

    float GetPlayerAxis()
    {
        Player player = DIContainer.Resolve<Player>();
        if (player == null) return 0;
        return currentDirection == Vector3.forward ?
            player.transform.position.z :
            player.transform.position.y;
    }

    float GetAxisValue(Vector3 pos)
    {
        return currentDirection == Vector3.forward ? pos.z : pos.y;
    }

    void ReleaseSegment(MapSegment seg)
    {
        foreach (var obj in seg.objects)
        {
            if (obj.TryGetComponent<Tile>(out _)) pool.ReleaseTile(obj);
            else pool.ReleasePlatform(obj);
        }
    }
    public void ClearAll()
    {
        foreach (var seg in activeSegments)
        {
            ReleaseSegment(seg);
        }
        activeSegments.Clear();
        nextSpawnPos = Vector3.zero;
    }
    public void OnTriggerReached()
    {
        // GameModeManager가 SetMode를 다시 호출할 것이므로 여기선 아무것도 안함
    }
}
