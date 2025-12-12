using System;
using System.Collections;
using UnityEngine;

public class GameModeManager : MonoBehaviour, IManagerBase
{
    public static event Action<GameMode> OnGameModeChanged;
    public static event Action OnFirstStageStarted;
    public int Priority => 1;
    GameMode currentMode;
    bool isFirstMode = true;
    CharacterManager characterManager;
    MapManager mapManager;

    public void Exit()
    {
    }
    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        characterManager = DIContainer.Resolve<CharacterManager>();
        mapManager = DIContainer.Resolve<MapManager>();
    }
    public void SetMode(GameMode mode)
    {
        currentMode = mode;
        if (isFirstMode)
        {
            FirstMode(currentMode);
            isFirstMode = false;
        }
        else
        {
            ModeChange(currentMode);
        }
        OnGameModeChanged?.Invoke(currentMode);
    }
    void FirstMode(GameMode mode)
    {
        mapManager.PrepareForMode(mode);
        characterManager.CreatePlayer();
        characterManager.InitializePlayer(mode);
        OnFirstStageStarted?.Invoke();
    }
    void ModeChange(GameMode mode)
    {
        mapManager.PrepareForMode(mode);
        characterManager.SetMode(mode);
    }
}
