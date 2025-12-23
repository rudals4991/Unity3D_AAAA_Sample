using System;
using System.Collections;
using UnityEngine;

public class SpeedScaleManager : MonoBehaviour, IManagerBase
{
    public static event Action<float> OnSpeedScaleChanged;
    float amount = 0.2f;
    float speedScale = 1f;
    public int Priority => 8;

    public void Exit()
    {
        GameModeManager.OnGameModeChanged -= ModeChanged;
        GameFlowManager.OnGameStarted -= HandleGameStarted;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        GameModeManager.OnGameModeChanged -= ModeChanged;
        GameModeManager.OnGameModeChanged += ModeChanged;
        GameFlowManager.OnGameStarted -= HandleGameStarted;
        GameFlowManager.OnGameStarted += HandleGameStarted;
        OnSpeedScaleChanged?.Invoke(speedScale);
    }
    void HandleGameStarted()
    {
        ResetScale(1f);
    }
    void ModeChanged(GameMode mode)
    {
        speedScale += amount;
        OnSpeedScaleChanged?.Invoke(speedScale);
    }
    void ResetScale(float scale = 1f)
    {
        speedScale = scale;
        OnSpeedScaleChanged?.Invoke(speedScale);
    }
}
