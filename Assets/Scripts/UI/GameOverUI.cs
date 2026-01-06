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
        Debug.Log("11");
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
        Debug.Log("GameOver");
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
        foreach (SceneList s in visibleScenes)
        {
            if (s == scene) return true;
        }
        return false;
    }
}
