using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance { get; private set; }
    public static event Action<SceneList> OnSceneChanged;
    public SceneList CurrentScene { get; private set; } = SceneList.Title;
    SceneList currentContent = SceneList.Title;
    bool isLoading = false;
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    IEnumerator Start()
    {
        yield return StartCoroutine(UISceneLoaded());
        yield return StartCoroutine(ChangeContentScene(SceneList.Title));
    }
    public void LoadScene(SceneList scene)
    {
        if (isLoading) return;
        StartCoroutine(ChangeContentScene(scene));
    }
    IEnumerator UISceneLoaded()
    {
        string uiName = ConvertSceneList(SceneList.UI);
        if (string.IsNullOrEmpty(uiName)) yield break;

        var uiScene = SceneManager.GetSceneByName(uiName);
        if (!uiScene.isLoaded)
        {
            yield return SceneManager.LoadSceneAsync(uiName, LoadSceneMode.Additive);
        }
    }
    IEnumerator ChangeContentScene(SceneList next)
    {
        if (isLoading) yield break;
        isLoading = true;
        if (next == SceneList.UI)
        {
            isLoading = false;
            yield break;
        }
        if (currentContent != next)
        {
            string curName = ConvertSceneList(currentContent);
            if (!string.IsNullOrEmpty(curName))
            {
                var curScene = SceneManager.GetSceneByName(curName);
                if (curScene.isLoaded)
                {
                    yield return SceneManager.UnloadSceneAsync(curName);
                }
            }
        }
        string nextName = ConvertSceneList(next);
        if (string.IsNullOrEmpty(nextName))
        {
            isLoading = false;
            yield break;
        }
        var nextScene = SceneManager.GetSceneByName(nextName);
        if (!nextScene.isLoaded)
        {
            yield return SceneManager.LoadSceneAsync(nextName, LoadSceneMode.Additive);
            nextScene = SceneManager.GetSceneByName(nextName);
        }
        if (nextScene.IsValid() && nextScene.isLoaded) SceneManager.SetActiveScene(nextScene);
        currentContent = next;
        CurrentScene = next;
        OnSceneChanged?.Invoke(next);
        if (next == SceneList.GamePlay)
            DIContainer.Resolve<GameModeManager>().StartCycle(GameMode.SideView_ToRight);
        isLoading = false;
    }
    public string UISceneName => ConvertSceneList(SceneList.UI);
    string ConvertSceneList(SceneList scene)
    {
        switch (scene)
        {
            case SceneList.Title: return "Title";
            case SceneList.GamePlay: return "GamePlay";
            case SceneList.GameOver: return "GameOver";
            case SceneList.UI: return "UI";
            default: return null;
        }
    }
}
