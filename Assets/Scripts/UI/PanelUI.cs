using UnityEngine;

public class PanelUI : MonoBehaviour, IUIBase, IVisibleUI
{
    [SerializeField] SceneList[] visibleScenes;
    public void Initialize()
    {
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
}
