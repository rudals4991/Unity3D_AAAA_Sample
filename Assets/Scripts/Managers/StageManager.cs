using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour, IManagerBase
{
    public int Priority => 3;
    CharacterManager characterManager;

    public void Exit()
    {
        GameModeManager.OnGameModeChanged -= OnGameModeChanged;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        characterManager = DIContainer.Resolve<CharacterManager>();
        GameModeManager.OnGameModeChanged -= OnGameModeChanged;
        GameModeManager.OnGameModeChanged += OnGameModeChanged;
    }
    void OnGameModeChanged(GameMode mode)
    {
        characterManager.CreatePlayer();
        characterManager.InitializePlayer(mode);
    }
}
