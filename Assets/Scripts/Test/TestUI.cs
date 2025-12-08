using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour,IUIBase
{
    GameModeManager gameModeManager;
    GameMode currentMode;
    public void Back()
    {
        currentMode = GameMode.BackView_ToForward;
        Debug.Log($"mode : {currentMode}");
        if(gameModeManager == null) gameModeManager = DIContainer.Resolve<GameModeManager>();
        gameModeManager.SetMode(GameMode.BackView_ToForward);
    }
    public void Right()
    {
        currentMode = GameMode.SideView_ToRight;
        Debug.Log($"mode : {currentMode}");
        if (gameModeManager == null) gameModeManager = DIContainer.Resolve<GameModeManager>();
        gameModeManager.SetMode(GameMode.SideView_ToRight);
    }
    public void Top()
    {
        currentMode = GameMode.SideView_ToTop;
        Debug.Log($"mode : {currentMode}");
        if (gameModeManager == null) gameModeManager = DIContainer.Resolve<GameModeManager>();
        gameModeManager.SetMode(GameMode.SideView_ToTop);
    }
    public void Down()
    {
        currentMode = GameMode.SideView_ToDown;
        Debug.Log($"mode : {currentMode}");
        if (gameModeManager == null) gameModeManager = DIContainer.Resolve<GameModeManager>();
        gameModeManager.SetMode(GameMode.SideView_ToDown);
    }

    public void Initialize()
    {
        gameModeManager = DIContainer.Resolve<GameModeManager>();
    }

    public void SetActiveFalse()
    {

    }
}
