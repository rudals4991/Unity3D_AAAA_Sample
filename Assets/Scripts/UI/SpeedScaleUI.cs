using TMPro;
using UnityEngine;

public class SpeedScaleUI : MonoBehaviour, IUIBase,IVisibleUI
{
    [SerializeField] TMP_Text text;
    [SerializeField] SceneList[] visibleScenes;
    float baseSpeedScale = 1f;
    float currentSpeedScale = 1f;
    public void Initialize()
    {
        SpeedScaleManager.OnSpeedScaleChanged -= UpdateUI;
        SpeedScaleManager.OnSpeedScaleChanged += UpdateUI;
        UpdateUI(baseSpeedScale);
    }

    public bool IsVisible(SceneList scene)
    {
        if(visibleScenes == null) return true;
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
    void UpdateUI(float scale)
    { 
        currentSpeedScale = baseSpeedScale * scale;
        text.text = currentSpeedScale.ToString();
    }
}
