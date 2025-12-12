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
        ReleaseAll();
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        tilePool = DIContainer.Resolve<TilePool>();
        platformPool = DIContainer.Resolve<PlatformPool>();
    }
    public GameObject GetTile(TileType type, Vector3 pos, Quaternion rot)
    {
        return tilePool.GetByRandom(type, pos, rot);
    }
    public GameObject GetPlatform(PlatformType type, Vector3 pos, Quaternion rot)
    {
        return platformPool.GetByRandom(type, pos, rot);
    }
    public void ReleaseTile(GameObject obj)
    { 
        tilePool.Release(obj);
    }
    public void ReleasePlatform(GameObject obj)
    { 
        platformPool.Release(obj);
    }
    public void ReleaseAll()
    {
        tilePool.ReleaseAll();
        platformPool.ReleaseAll();
    }
}
