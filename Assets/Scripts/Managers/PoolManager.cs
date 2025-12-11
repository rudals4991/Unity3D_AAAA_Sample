using System.Collections;
using UnityEngine;

public class PoolManager : MonoBehaviour, IManagerBase
{
    TilePool tilePool;
    PlatformPool platformPool;
    public TilePool TilePool => tilePool;
    public PlatformPool PlatformPool => platformPool;
    public int Priority => 5;

    public void Exit()
    {
        StageManager.OnStageStarted -= InitializePool;
        ReleaseAll();
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        tilePool = DIContainer.Resolve<TilePool>();
        platformPool = DIContainer.Resolve<PlatformPool>();
        // TODO: 스테이지 시작 이벤트 구독
        StageManager.OnStageStarted -= InitializePool;
        StageManager.OnStageStarted += InitializePool;
    }
    void InitializePool()
    {
        tilePool.Initialize();
        platformPool.Initialize();
    }
    public GameObject GetPlatform(PlatformType type, Vector3 pos, Quaternion rot)
    {
        return platformPool.Get(type, pos, rot);
    }
    public void ReleasePlatform(PlatformType type, GameObject obj)
    {
        platformPool.Release(type, obj);
    }
    public GameObject GetTile(TileType type, Vector3 pos, Quaternion rot)
    {
        return tilePool.Get(type, pos, rot);
    }
    public void ReleaseTile(TileType tpye, GameObject obj)
    { 
        tilePool.Release(tpye, obj);
    }
    public void ReleaseAll()
    {
        tilePool.ReleaseAll();
        platformPool.ReleaseAll();
    }
}
