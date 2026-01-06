using System;
using System.Collections;
using UnityEngine;

public class GameFlowManager : MonoBehaviour, IManagerBase
{
    public static event Action OnGameStarted;
    public static event Action OnGamePlayBegin;
    public static event Action<GameoverReason> OnGameOvered;
    public int Priority => 9;

    bool isGamePlaying = false;
    bool isLoading = false;
    bool hasPlayBegun = false;
    CountManager countManager;
    PauseManager pauseManager;
    MySceneManager sceneManager;

    public bool IsInRun => isGamePlaying;               
    public bool CanGameplay => isGamePlaying && hasPlayBegun && !pauseManager.BlockGameplayTick;
    public void Exit()
    {
        MySceneManager.OnSceneLoaded -= SceneLoaded;
        CountManager.OnCountDownFin -= CountDownFin;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        countManager = DIContainer.Resolve<CountManager>();
        pauseManager = DIContainer.Resolve<PauseManager>();
        sceneManager = MySceneManager.Instance;

        MySceneManager.OnSceneLoaded -= SceneLoaded;
        MySceneManager.OnSceneLoaded += SceneLoaded;
        CountManager.OnCountDownFin -= CountDownFin;
        CountManager.OnCountDownFin += CountDownFin;
    }
    void SceneLoaded(SceneList scene)
    {
        if (!isLoading) return;
        if (scene != SceneList.GamePlay) return;
        isLoading = false;
        StartRun();
    }
    void CountDownFin(CountPurpose purpose)
    {
        if (!isGamePlaying) return;
        if (purpose != CountPurpose.FirstStart) return;
        if (hasPlayBegun) return;

        hasPlayBegun = true;
        OnGamePlayBegin?.Invoke();
    }
    public void GameStart()
    {
        if (isLoading) return;

        if (sceneManager.CurrentScene == SceneList.GamePlay)
        {
            StartRun();
            return;
        }
        if (isGamePlaying) return;
        isLoading = true;
        hasPlayBegun = false;
        sceneManager.LoadScene(SceneList.GamePlay);
    }
    public void GameRestart()
    {
        if (isLoading) return;
        if (sceneManager.CurrentScene != SceneList.GamePlay) return;
        StartRun();
    }
    public void GameOver(GameoverReason reason)
    {
        if (!isGamePlaying) return;
        isGamePlaying = false;
        isLoading = false;
        hasPlayBegun = false;
        OnGameOvered?.Invoke(reason);
    }
    public void GamePause()
    {
        if (!isGamePlaying) return;
        pauseManager.Pause();
    }
    public void GameResume(float second = 3f)
    {
        if (!isGamePlaying) return;
        pauseManager.Resume(second);
    }
    void StartRun()
    {
        isLoading = false;
        isGamePlaying = true;
        hasPlayBegun = false;
        OnGameStarted?.Invoke();
        countManager.StartFirstCountDown(3f);
    }
}
