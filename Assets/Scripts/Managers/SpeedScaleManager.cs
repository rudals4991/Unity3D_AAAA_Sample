using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SpeedScaleManager : MonoBehaviour, IManagerBase
{
    public static event Action<float> OnSpeedScaleChanged;
    float amount = 0.2f;
    float speedScale = 1f;
    public int Priority => 8;
    bool isFirst = true;

    public void Exit()
    {
        GameModeManager.OnGameModeChanged -= ModeChanged;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        GameModeManager.OnGameModeChanged -= ModeChanged;
        GameModeManager.OnGameModeChanged += ModeChanged;

        OnSpeedScaleChanged?.Invoke(speedScale);
    }
    void ModeChanged(GameMode mode)
    {
        Debug.Log($"isFirst = {isFirst}");
        if (isFirst)
        {
            isFirst = false;
            return;
        }
        speedScale += amount;
        OnSpeedScaleChanged?.Invoke(speedScale);
    }
    public void ResetScale(float scale = 1f)
    {
        speedScale = scale;
        OnSpeedScaleChanged?.Invoke(speedScale);
    }
}
