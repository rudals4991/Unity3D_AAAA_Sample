using System;
using System.Collections;
using UnityEngine;

public class SpeedScaleManager : MonoBehaviour, IManagerBase
{
    [SerializeField] AudioClip sfx; 
    public static event Action<float> OnSpeedScaleChanged;
    float amount = 0.16f;
    float speedScale = 0.84f;
    SoundManager soundManager;
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
        soundManager = DIContainer.Resolve<SoundManager>();
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
        if(speedScale > 1f) soundManager?.PlaySFX(sfx);
        OnSpeedScaleChanged?.Invoke(speedScale);
    }
    void ResetScale(float scale = 1f)
    {
        speedScale = scale;
        OnSpeedScaleChanged?.Invoke(speedScale);
    }
}
