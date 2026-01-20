using System;
using System.Collections;
using UnityEngine;

public class GameModeManager : MonoBehaviour, IManagerBase
{
    public static event Action<GameMode> OnGameModeChanged;
    public int Priority => 7;
    readonly GameMode[] cycle =
    {
        GameMode.SideView_ToRight,
        GameMode.BackView_ToForward,
        GameMode.SideView_ToTop,
        GameMode.SideView_ToRight,
        GameMode.BackView_ToForward,
        GameMode.SideView_ToDown,
    };
    int index = 0;
    public GameMode CurrentMode { get; private set; }
    bool isFirstMode = true;
    CharacterManager characterManager;
    MapManager mapManager;

    public void Exit()
    {
        GameFlowManager.OnGameStarted -= HandleGameStarted;
    }
    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        characterManager = DIContainer.Resolve<CharacterManager>();
        mapManager = DIContainer.Resolve<MapManager>();
        GameFlowManager.OnGameStarted -= HandleGameStarted;
        GameFlowManager.OnGameStarted += HandleGameStarted;
    }
    void HandleGameStarted()
    {
        StartCycle(GameMode.SideView_ToRight);
    }
    public void StartCycle(GameMode? start = null)
    {
        if (start.HasValue)
        {
            int found = Array.IndexOf(cycle, start.Value);
            index = found >= 0 ? found : 0;
        }
        else index = 0;
        isFirstMode = true;
        SetMode(cycle[index]);
    }
    public void AdvanceMode()
    {
        index = (index + 1) % cycle.Length;
        SetMode(cycle[index]);
    }
    public void SetMode(GameMode mode)
    {
        CurrentMode = mode;
        if (isFirstMode)
        {
            if (characterManager == null) Debug.Log("Null");
            characterManager.CreatePlayer();
            characterManager.InitializePlayer(mode);
            mapManager.PrepareForMode(mode);
            isFirstMode = false;
        }
        else
        {
            characterManager.SetMode(mode);
            mapManager.PrepareForMode(mode);
        }
        OnGameModeChanged?.Invoke(CurrentMode);
    }
}
