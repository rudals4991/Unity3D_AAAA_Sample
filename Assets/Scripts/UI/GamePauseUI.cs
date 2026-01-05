using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour,IUIBase,IVisibleUI
{
    [SerializeField] SceneList[] visibleScenes;
    GameFlowManager gameFlowManager;
    bool isPaused = false;
    public void Initialize()
    {
        gameFlowManager = DIContainer.Resolve<GameFlowManager>();
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void PauseAndResume()
    {
        if(gameFlowManager == null) gameFlowManager = DIContainer.Resolve<GameFlowManager>();
        if (!isPaused)
        {
            isPaused = true;
            gameFlowManager.GamePause();
        }
        else
        {
            isPaused = false;
            gameFlowManager.GameResume(3f);
        }
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
}
