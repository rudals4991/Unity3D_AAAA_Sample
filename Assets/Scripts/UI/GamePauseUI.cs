using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour,IUIBase,IVisibleUI
{
    [SerializeField] SceneList[] visibleScenes;
    PauseManager pauseManager;
    bool isPaused = false;
    public void Initialize()
    {
        pauseManager = DIContainer.Resolve<PauseManager>();
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void PauseAndResume()
    {
        if(pauseManager == null) pauseManager = DIContainer.Resolve<PauseManager>();
        if (isPaused)
        {
            isPaused = false;
            pauseManager.Resume();
        }
        else
        {
            isPaused = true;
            pauseManager.Pause();
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
