using System;
using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour, IManagerBase
{
    public int Priority => 3;
    public static event Action OnStageStarted;
    CharacterManager characterManager;
    bool isLoaded;

    public void Exit()
    {
        GameModeManager.OnGameModeChanged -= OnGameModeChanged;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        characterManager = DIContainer.Resolve<CharacterManager>();
        GameModeManager.OnGameModeChanged -= OnGameModeChanged;
        GameModeManager.OnGameModeChanged += OnGameModeChanged;
    }
    void OnGameModeChanged(GameMode mode)
    {
        if (!isLoaded)
        { 
            isLoaded = true;
            characterManager.CreatePlayer();
            characterManager.InitializePlayer(mode);
            StageStart();
        }
        characterManager.SetMode(mode);
    }
    void StageStart()
    {
        OnStageStarted.Invoke();
    }
}
