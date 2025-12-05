using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour, IManagerBase
{
    public static event Action<GameMode> OnGameModeChanged;
    public int Priority => 1;
    GameMode currentMode;

    public void Exit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        //TODO: Scene 전환 구조 완성되면 거기에 연결해서 씬 변경 감지
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        yield return null;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentMode = ConvertSceneToMode(scene.name);
        OnGameModeChanged?.Invoke(currentMode);
    }
    GameMode ConvertSceneToMode(string sceneName)
    {
        switch (sceneName)
        {
            case "BackView_Forward": return GameMode.BackView_ToForward;
            case "SideView_ToRight": return GameMode.SideView_ToRight;
            case "SideView_ToDown": return GameMode.SideView_ToDown;
            case "SideView_ToTop": return GameMode.SideView_ToTop;
            default: return GameMode.SideView_ToRight;
        }
    }
}
