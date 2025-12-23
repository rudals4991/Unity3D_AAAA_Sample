using System;
using System.Collections;
using UnityEngine;

public class GameFlowManager : MonoBehaviour, IManagerBase
{
    public static event Action OnGameStarted;
    public static event Action OnGamePlayBegin;
    public static event Action<GameoverReason> OnGameOvered;
    public int Priority => 10;

    bool isGamePlaying = false;
    bool isLoading = false;
    bool hasPlayBegun = false;
    CountManager countManager;

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
        isGamePlaying = true;
        hasPlayBegun = false;
        OnGameStarted?.Invoke();
        countManager.StartFirstCountDown(3f);
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
        if (isGamePlaying || isLoading) return;
        isLoading = true;
        hasPlayBegun = false;
        MySceneManager.Instance.LoadScene(SceneList.GamePlay);
    }
    public void GameOver(GameoverReason reason)
    { 
        if (!isGamePlaying) return;
        isGamePlaying = false;
        isLoading = false;
        OnGameOvered?.Invoke(reason);
    }
}
