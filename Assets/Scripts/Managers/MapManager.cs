using System.Collections;
using UnityEngine;

public class MapManager : MonoBehaviour,IManagerBase
{
    PlatformGenerator platform;
    TileGenerator tile;
    PoolManager pool;
    public int Priority => 6;

    public void Exit()
    {
        GameModeManager.OnGameModeChanged -= ApplyMapSetting;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        tile = DIContainer.Resolve<TileGenerator>();
        platform = DIContainer.Resolve<PlatformGenerator>();
        pool = DIContainer.Resolve<PoolManager>();
        GameModeManager.OnGameModeChanged -= ApplyMapSetting;
        GameModeManager.OnGameModeChanged += ApplyMapSetting;
    }
    void ApplyMapSetting(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.BackView_ToForward:
            case GameMode.SideView_ToRight: tile.CreateTileByMode(mode,pool.TilePool); break;

            case GameMode.SideView_ToTop: 
            case GameMode.SideView_ToDown: platform.CreatePlatformByMode(mode,pool.PlatformPool); break;
        }
    }
}
