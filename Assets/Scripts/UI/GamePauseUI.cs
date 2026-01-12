using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour,IUIBase,IVisibleUI
{
    [SerializeField] SceneList[] visibleScenes;
    [SerializeField] GameObject pausePanel;
    GameFlowManager gameFlowManager;
    bool isPaused = false;
    public void Initialize()
    {
        gameFlowManager = DIContainer.Resolve<GameFlowManager>();
        isPaused = false;
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public bool IsVisible(SceneList scene)
    {
        if (visibleScenes == null) return true;
        foreach (SceneList s in visibleScenes)
        { 
            if(s == scene) return true;
        }
        return false;
    }
    public void Pause()
    {
        if (gameFlowManager == null) gameFlowManager = DIContainer.Resolve<GameFlowManager>();
        if (isPaused) return;
        SetBool(true);
        gameFlowManager.GamePause();
    }
    public void Resume()
    {
        if (!isPaused)
        { 
            pausePanel.SetActive(false);
            return;
        }
        SetBool(false);
        gameFlowManager.GameResume(3f);
    }
    public void Restart()
    {
        SetBool(false);
        gameFlowManager.GameResume(0);
        gameFlowManager.GameRestart();
    }
    public void GoTitle()
    {
        SetBool(false);
        gameFlowManager.GameResume(0);
        gameFlowManager.GoTitle();
    }
    void SetBool(bool b)
    {
        isPaused = b;
        pausePanel.SetActive(b);
    }
}
