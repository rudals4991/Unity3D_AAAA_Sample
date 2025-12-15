using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance { get; private set; }
    public static event Action<SceneList> OnSceneChanged;
    
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
    void Start()
    {
        LoadSingleScene(SceneList.Title);
    }
    public void LoadSingleScene(SceneList scene)
    {
        StartCoroutine(StartSceneRoutine(scene));
    }
    IEnumerator LoadAdditiveScene()
    {
        string UIScene = ConvertSceneList(SceneList.UI);
        if (UIScene == null) yield break;
        if (!SceneManager.GetSceneByName(UIScene).isLoaded)
            yield return SceneManager.LoadSceneAsync(UIScene, LoadSceneMode.Additive);
    }
    IEnumerator StartSceneRoutine(SceneList scene)
    {
        string targetScene = ConvertSceneList(scene);
        if(targetScene == null) yield break;
        yield return SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
        OnSceneChanged?.Invoke(scene);
        yield return StartCoroutine(LoadAdditiveScene());
        if (scene == SceneList.GamePlay) DIContainer.Resolve<GameModeManager>().StartCycle(GameMode.SideView_ToRight);
    }
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
