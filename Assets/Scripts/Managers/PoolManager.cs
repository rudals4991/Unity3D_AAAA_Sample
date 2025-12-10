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
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
    }
}
