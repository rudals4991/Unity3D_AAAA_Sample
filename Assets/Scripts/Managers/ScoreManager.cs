using System;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IManagerBase
{
    public static Action<int> OnCurrentScoreChanged;
    public static Action<int> OnBestScoreChanged;
    public int Priority => 9;
    const int MAXSCORE = int.MaxValue;

    [Header("Score Setting")]
    [SerializeField] float scoreTick = 0.1f;
    [SerializeField] int scorePerTick = 10;
    bool useUnscaledTime = false;

    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }

    bool isRunning = false;
    float scoreAccumulator = 0f;
    float scoreScale = 1f;

    CountManager countManager;
    public void Exit()
    {
        countManager.OnCountDownFin -= StartScoring;
        SpeedScaleManager.OnSpeedScaleChanged -= ApplySpeedScale;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
        yield return null;
        countManager = DIContainer.Resolve<CountManager>();
        countManager.OnCountDownFin -= StartScoring;
        countManager.OnCountDownFin += StartScoring;
        SpeedScaleManager.OnSpeedScaleChanged -= ApplySpeedScale;
        SpeedScaleManager.OnSpeedScaleChanged += ApplySpeedScale;
    }
    void StartScoring()
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
        isRunning = false;
        OnCurrentScoreChanged?.Invoke(CurrentScore);

        if (!keepBest)
        {
            BestScore = 0;
            PlayerPrefs.SetInt("BestScore", BestScore);
            PlayerPrefs.Save();
            OnBestScoreChanged?.Invoke(BestScore);
        }
    }
    void ApplySpeedScale(float scale)
    { 
        scoreScale = scale;
    }
    public void Tick(float dt, float udt)
    {
        if (!isRunning) return;
        float t = useUnscaledTime ? udt : dt;
        if (t <= 0) return;
        scoreAccumulator += t;
        while (scoreAccumulator >= scoreTick)
        {
            scoreAccumulator -= scoreTick;
            int add = Mathf.RoundToInt(scorePerTick * scoreScale);
            if (add <= 0) continue;
            AddScore(add);
            if (CurrentScore >= MAXSCORE) break;
        }
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
            PlayerPrefs.SetInt("BestScore", BestScore);
            PlayerPrefs.Save();
            OnBestScoreChanged?.Invoke(BestScore);
        }
    }
}
