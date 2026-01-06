using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour, IUIBase,IVisibleUI
{
    GameFlowManager gameFlowManager;
    ScoreManager scoreManager;
    [SerializeField] TMP_Text currentScore;
    [SerializeField] SceneList[] visibleScenes;

    public void Initialize()
    {
        gameFlowManager = DIContainer.Resolve<GameFlowManager>();
        scoreManager = DIContainer.Resolve<ScoreManager>();
        GameFlowManager.OnGameOvered -= GameOver;
        GameFlowManager.OnGameOvered += GameOver;
        GameFlowManager.OnGameStarted -= GameStart;
        GameFlowManager.OnGameStarted += GameStart;
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void GameReStart()
    {
        if (gameFlowManager is null) gameFlowManager = DIContainer.Resolve<GameFlowManager>();
        gameFlowManager.GameRestart();
    }
    void GameOver(GameoverReason reason)
    {
        gameObject.SetActive(true);
        if (scoreManager is null) scoreManager = DIContainer.Resolve<ScoreManager>();
        currentScore.text = scoreManager.CurrentScore.ToString();
    }
    void GameStart()
    {
        SetActiveFalse();
    }

    public bool IsVisible(SceneList scene)
    {
        if (visibleScenes == null) return false;
        for (int i = 0; i < visibleScenes.Length; i++)
        {
            if (visibleScenes[i] == scene) return true;
        }
        return false;
    }
}
