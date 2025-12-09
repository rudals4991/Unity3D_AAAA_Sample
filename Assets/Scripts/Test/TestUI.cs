using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour,IUIBase
{
    GameModeManager gameModeManager;
    public void Back()
    {
        if(gameModeManager == null) gameModeManager = DIContainer.Resolve<GameModeManager>();
        gameModeManager.SetMode(GameMode.BackView_ToForward);
    }
    public void Right()
    {
        if (gameModeManager == null) gameModeManager = DIContainer.Resolve<GameModeManager>();
        gameModeManager.SetMode(GameMode.SideView_ToRight);
    }
    public void Top()
    {
        if (gameModeManager == null) gameModeManager = DIContainer.Resolve<GameModeManager>();
        gameModeManager.SetMode(GameMode.SideView_ToTop);
    }
    public void Down()
    {
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
