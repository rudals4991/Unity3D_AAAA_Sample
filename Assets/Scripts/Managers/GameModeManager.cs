using System;
using System.Collections;
using UnityEngine;

public class GameModeManager : MonoBehaviour, IManagerBase
{
    public static event Action<GameMode> OnGameModeChanged;
    public int Priority => 1;
    GameMode currentMode;

    public void Exit()
    {
    }
    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
    }
    public void SetMode(GameMode mode)
    {
        currentMode = mode;
        OnGameModeChanged?.Invoke(currentMode);
    }
}
