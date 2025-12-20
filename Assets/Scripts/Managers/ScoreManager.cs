using System;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IManagerBase
{
    public static Action<int> OnCurrentScoreChanged;
    public static Action<int> OnBestScoreChanged;
    public int Priority => 9;
    const int MAXSCORE = 9_999_999;
    const string BESTSCORE = "BestScore";

    [Header("Score Setting")]
    [SerializeField] int scroePerSecond = 10;
    bool useUnscaledTime = false;

    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }

    bool isRunning = false;
    float scoreAccumulator = 0f;

    CountManager countManager;
    public void Exit()
    {
        countManager.OnCountDownFin -= StartScoring;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        BestScore = PlayerPrefs.GetInt(BESTSCORE, 0);
        yield return null;
        countManager = DIContainer.Resolve<CountManager>();
        countManager.OnCountDownFin -= StartScoring;
        countManager.OnCountDownFin += StartScoring;
    }
    public void StartScoring()
    { 
        isRunning = true;
    }
    public void StopScoring()
    {
        isRunning = false;
    }
    public void ResetScore(bool keepBest = true)
    {
        CurrentScore = 0;
        scoreAccumulator = 0f;
        OnCurrentScoreChanged?.Invoke(CurrentScore);

        if (!keepBest)
        {
            BestScore = 0;
            PlayerPrefs.SetInt(BESTSCORE, BestScore);
            PlayerPrefs.Save();
            OnBestScoreChanged?.Invoke(BestScore);
        }
    }
    public void Tick(float dt, float udt)
    {
        if (!isRunning) return;
        float t = useUnscaledTime ? udt : dt;
        if (t <= 0) return;
        scoreAccumulator += scroePerSecond * t;
        int add = Mathf.FloorToInt(scoreAccumulator);
        if (add <= 0) return;
        scoreAccumulator -= add;
        AddScore(add);
    }
    void AddScore(int add)
    {
        if (add <= 0) return;
        int newScore = CurrentScore + add;
        if(newScore > MAXSCORE) newScore = MAXSCORE;
        if (newScore == CurrentScore) return;
        CurrentScore = newScore;
        OnCurrentScoreChanged?.Invoke(CurrentScore);
        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
            PlayerPrefs.SetInt(BESTSCORE, BestScore);
            PlayerPrefs.Save();
            OnBestScoreChanged?.Invoke(BestScore);
        }
    }
}
