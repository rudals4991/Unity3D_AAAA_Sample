using System;
using System.Collections;
using UnityEngine;

public class CountManager : MonoBehaviour, IManagerBase
{
    public int Priority => 2;
    public bool IsGameActive { get; private set; } = false;
    public bool IsCounting { get; private set; } = false;
    public static event Action<int> OnCountDown;
    public static event Action<CountPurpose> OnCountDownFin;

    float remain;
    CountPurpose currentPurpose;

    public void Exit()
    {
        GameFlowManager.OnGameStarted -= OnGameStarted;
        GameFlowManager.OnGameOvered -= OnGameOver;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        GameFlowManager.OnGameStarted -= OnGameStarted;
        GameFlowManager.OnGameStarted += OnGameStarted;

        GameFlowManager.OnGameOvered -= OnGameOver;
        GameFlowManager.OnGameOvered += OnGameOver;
    }
    void OnGameStarted()
    {
        IsGameActive = false;
        IsCounting = false;
        remain = 0;
    }
    void OnGameOver(GameoverReason _)
    {
        IsGameActive = false;
        IsCounting = false;
        remain = 0f;
    }
    public void StartFirstCountDown(float second = 3f)
    {
        StartCountDown(second, CountPurpose.FirstStart);
    }
    public void StartResumeCountDown(float second = 3f)
    {
        StartCountDown(second, CountPurpose.Resume);
    }
    void StartCountDown(float second, CountPurpose purpose)
    {
        if (IsCounting) return;
        currentPurpose = purpose;
        remain = Mathf.Max(0.1f, second);
        IsGameActive = false;
        IsCounting = true;
        OnCountDown?.Invoke(Mathf.CeilToInt(remain));
    }
    public void Tick(float unscaledDt)
    {
        if (!IsCounting) return;
        if (unscaledDt <= 0f) return;
        float prev = remain;
        remain -= unscaledDt;
        int prevInt = Mathf.CeilToInt(prev);
        int curInt = Mathf.CeilToInt(Mathf.Max(remain, 0f));
        if (curInt != prevInt) OnCountDown?.Invoke(curInt);
        if (remain <= 0f)
        {
            IsCounting = false;
            IsGameActive = true;
            OnCountDownFin?.Invoke(currentPurpose);
        }
    }
}
