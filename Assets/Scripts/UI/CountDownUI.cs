using TMPro;
using UnityEngine;

public class CountDownUI : MonoBehaviour, IUIBase, IVisibleUI
{
    [SerializeField] TMP_Text text;
    [SerializeField] SceneList[] visibleScenes;
    CountManager countManager;
    public void Initialize()
    {
        countManager = DIContainer.Resolve<CountManager>();
        HideText();
        CountManager.OnCountDown -= OnCountChanged;
        CountManager.OnCountDown += OnCountChanged;
        CountManager.OnCountDownFin -= OnCountDownFin;
        CountManager.OnCountDownFin += OnCountDownFin;
    }
    void OnDestroy()
    {
        CountManager.OnCountDown -= OnCountChanged;
        CountManager.OnCountDownFin -= OnCountDownFin;
    }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    void OnCountChanged(int second)
    {
        if (second <= 0)
        {
            HideText();
            return;
        }
        text.gameObject.SetActive(true);
        text.text = second.ToString();
    }
    void OnCountDownFin(CountPurpose _)
    {
        HideText();
    }
    void HideText()
    {
        if (text == null) return;
        text.text = string.Empty;
        text.gameObject.SetActive(false);
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
}
