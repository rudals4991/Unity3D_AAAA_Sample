using System.Collections;
using UnityEngine;

public enum PauseState
{ 
    Playing, Paused, Resuming
}

public class PauseManager : MonoBehaviour, IManagerBase
{
    public int Priority => 2;
    public PauseState State { get; private set; } = PauseState.Playing;
    public bool IsHardPaused => State == PauseState.Paused; // 완전 정지(카운트도 멈춤)
    public bool BlockGameplayTick => State != PauseState.Playing; // 플레이어/맵 Tick 차단
    CountManager countManager;

    public void Exit()
    {
        countManager.OnCountDownFin -= OnCountDownFin;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        countManager = DIContainer.Resolve<CountManager>();
        countManager.OnCountDownFin -= OnCountDownFin;
        countManager.OnCountDownFin += OnCountDownFin;
    }
    public void Pause()
    {
        if (State != PauseState.Playing) return;
        State = PauseState.Paused;
        Time.timeScale = 0f;
    }
    public void Resume(float second = 3f)
    {
        if (State != PauseState.Paused) return;
        State = PauseState.Resuming;
        Time.timeScale = 0;
        countManager.StartResumeCountDown(second);
    }
    void OnCountDownFin()
    {
        if (State != PauseState.Resuming) return;
        Time.timeScale = 1f;
        State = PauseState.Playing;
    }
}
