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
        OnCountDownFin();
        countManager.OnCountDown -= OnCountChanged;
        countManager.OnCountDownFin -= OnCountDownFin;
        countManager.OnCountDown += OnCountChanged;
        countManager.OnCountDownFin += OnCountDownFin;
    }
    private void OnDestroy()
    {
        if (countManager == null) return;
        countManager.OnCountDown -= OnCountChanged;
        countManager.OnCountDownFin -= OnCountDownFin;
    }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    void OnCountChanged(int second)
    {
        if (second <= 0)
        {
            OnCountDownFin();
            return;
        }
        text.gameObject.SetActive(true);
        text.text = second.ToString();
    }
    void OnCountDownFin()
    {
        text.text = "";
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
