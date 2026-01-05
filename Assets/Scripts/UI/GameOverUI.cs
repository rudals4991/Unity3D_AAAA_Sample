using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour, IUIBase, IVisibleUI
{
    GameFlowManager gameFlowManager;
    ScoreManager scoreManager;
    [SerializeField] TMP_Text currentScore;
    [SerializeField] TMP_Text bestScore;
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

    public bool IsVisible(SceneList scene)
    {
        if (visibleScenes == null) return true;
        foreach (SceneList s in visibleScenes)
        {
            if (s == scene) return true;
        }
        return false;
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void GameReStart()
    {
        gameFlowManager.GameRestart();
    }
    void GameOver(GameoverReason reason)
    {
        gameObject.SetActive(true);
        currentScore.text = scoreManager.CurrentScore.ToString();
        bestScore.text = scoreManager.BestScore.ToString();
    }
    void GameStart()
    {
        SetActiveFalse();
    }
}
