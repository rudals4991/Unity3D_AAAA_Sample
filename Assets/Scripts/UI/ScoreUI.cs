using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour, IUIBase, IVisibleUI
{
    [SerializeField] SceneList[] visibleScenes;
    [SerializeField] TMP_Text bestScoreText;
    [SerializeField] TMP_Text currentScoreText;
    ScoreManager scoreManager;

    public void Initialize()
    {
        scoreManager ??= DIContainer.Resolve<ScoreManager>();
        ApplyBestScore(scoreManager.BestScore);
        ApplyCurrentScore(scoreManager.CurrentScore);
        ScoreManager.OnBestScoreChanged -= SetBestScoreUI;
        ScoreManager.OnBestScoreChanged += SetBestScoreUI;
        ScoreManager.OnCurrentScoreChanged -= SetCurrentScoreUI;
        ScoreManager.OnCurrentScoreChanged += SetCurrentScoreUI;
    }
    private void OnDestroy()
    {
        ScoreManager.OnBestScoreChanged -= SetBestScoreUI;
        ScoreManager.OnCurrentScoreChanged -= SetCurrentScoreUI;
    }
    void SetBestScoreUI(int value) => ApplyBestScore(value);
    void SetCurrentScoreUI(int value) => ApplyCurrentScore(value);
    void ApplyBestScore(int value)
    {
        if (bestScoreText == null) return;
        bestScoreText.text = value.ToString("N0");
    }
    void ApplyCurrentScore(int value)
    {
        if (currentScoreText == null) return;
        currentScoreText.text = value.ToString("N0");
    }
    public bool IsVisible(SceneList scene)
    {
        if (visibleScenes == null || visibleScenes.Length == 0) return true;
        for (int i = 0; i < visibleScenes.Length; i++)
        {
            if (visibleScenes[i] == scene) return true;
        }
        return false;
    }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
