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
        CountManager.OnCountDownFin -= OnCountDownFin;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        countManager = DIContainer.Resolve<CountManager>();
        CountManager.OnCountDownFin -= OnCountDownFin;
        CountManager.OnCountDownFin += OnCountDownFin;
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
        if (second <= 0f)
        {
            Time.timeScale = 1f;
            State = PauseState.Playing;
            return;
        }
        State = PauseState.Resuming;
        Time.timeScale = 0f;
        countManager.StartResumeCountDown(second);
    }
    void OnCountDownFin(CountPurpose purpose)
    {
        if (purpose != CountPurpose.Resume) return;
        if (State != PauseState.Resuming) return;
        Time.timeScale = 1f;
        State = PauseState.Playing;
    }
}
