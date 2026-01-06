using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour, IManagerBase
{
    PlatformGenerator platform;
    TileGenerator tile;
    PoolManager pool;
    public int Priority => 5;

    Vector3 currentDirection = Vector3.forward;
    List<GameObject> activeObjects = new();
    bool isInitialized;
    bool isFirst = true;

    public void Exit()
    {
        ClearAll();
    }
    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        pool = DIContainer.Resolve<PoolManager>();
    }
    public void PrepareForMode(GameMode mode)
    {
        currentDirection = ResolveDirection(mode);

        if (!isInitialized) InitializeGenerators();
        if (currentDirection == Vector3.forward) platform.ClearBackground();
        ClearAll();
        Vector3 anchor = GetAnchorPos(mode);
        GenerateOneSegment(mode, anchor);
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
        if (tile == null) tile = DIContainer.Resolve<TileGenerator>();
        platform.Initialize(pool);
        tile.Initialize(pool);
        isInitialized = true;
    }
    void GenerateOneSegment(GameMode mode, Vector3 startPos)
    {
        if (currentDirection == Vector3.forward)
        {
            tile.SetLinearType(mode == GameMode.BackView_ToForward ? TileType.Linear_ToForward : TileType.Linear_ToRight);

            var result = tile.Generate(startPos, currentDirection);
            activeObjects = result.objects;
        }
        else
        {
            var result = platform.Generate(startPos, currentDirection);
            activeObjects = result.objects;
        }
    }
    Vector3 GetAnchorPos(GameMode mode)
    {
        if (isFirst && mode == GameMode.SideView_ToRight)
        {
            isFirst = false;
            return Vector3.zero;
        }
        Player player = DIContainer.Resolve<Player>();
        if (player == null) return Vector3.zero;
        Vector3 p = new();
        if (mode == GameMode.SideView_ToRight || mode == GameMode.BackView_ToForward)
            p = player.transform.position + new Vector3(0, 0, 24.5f);
        else p = player.transform.position;

        //if (ResolveDirection(mode) == Vector3.forward) return new Vector3(pla, 0f, p.z);
        return p;
    }
    public void ClearAll()
    {
        if (activeObjects == null || activeObjects.Count == 0) return;
        foreach (var obj in activeObjects)
        {
            if (obj == null) continue;
            if (obj.TryGetComponent<Tile>(out _)) pool.ReleaseTile(obj);
            else pool.ReleasePlatform(obj);
        }
        activeObjects.Clear();
    }
}
