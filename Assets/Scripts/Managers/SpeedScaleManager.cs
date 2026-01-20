using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedScaleManager : MonoBehaviour, IManagerBase
{
    [SerializeField] AudioClip sfx; 
    public static event Action<float> OnSpeedScaleChanged;
    float amount = 0.16f;
    float speedScale = 1f;

    bool sessionActive = false;
    bool hasLastMode = false;
    GameMode lastMode;

    SoundManager soundManager;
    GameModeManager gameModeManager;
    public int Priority => 8;

    public void Exit()
    {
        GameModeManager.OnGameModeChanged -= ModeChanged;
        GameFlowManager.OnGameStarted -= SetGameStarted;
        GameFlowManager.OnGamePlayBegin -= SetGamePlayBegin;
        GameFlowManager.OnGameOvered -= SetGameOvered;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        soundManager = DIContainer.Resolve<SoundManager>();
        gameModeManager = DIContainer.Resolve<GameModeManager>();
        GameModeManager.OnGameModeChanged -= ModeChanged;
        GameModeManager.OnGameModeChanged += ModeChanged;
        GameFlowManager.OnGameStarted -= SetGameStarted;
        GameFlowManager.OnGameStarted += SetGameStarted;
        GameFlowManager.OnGamePlayBegin -= SetGamePlayBegin; 
        GameFlowManager.OnGamePlayBegin += SetGamePlayBegin;
        GameFlowManager.OnGameOvered -= SetGameOvered;
        GameFlowManager.OnGameOvered += SetGameOvered;
        Broadcast(speedScale);
    }
    void SetGameStarted()
    {
        sessionActive = false;
        speedScale = 1f;
        Broadcast(speedScale);
        hasLastMode = true;
        lastMode = gameModeManager.CurrentMode;
    }
    void SetGamePlayBegin()
    {
        sessionActive = true;
        hasLastMode = true;
        lastMode = gameModeManager.CurrentMode;
    }
    void SetGameOvered(GameoverReason _)
    {
        sessionActive = false;
        hasLastMode = false;
    }
    void ModeChanged(GameMode mode)
    {
        if (!sessionActive)
        {
            hasLastMode = true;
            lastMode = mode;
            return;
        }
        if (hasLastMode && EqualityComparer<GameMode>.Default.Equals(lastMode, mode)) return;
        hasLastMode = true;
        lastMode = mode;
        speedScale += amount;
        if (speedScale > 1f) soundManager?.PlaySFX(sfx);
        Debug.Log($"Mode / {speedScale}");
        Broadcast(speedScale);
    }
    void Broadcast(float value)
    {
        OnSpeedScaleChanged?.Invoke(value);
    }
}
