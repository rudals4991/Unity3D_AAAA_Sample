using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isInitialized = false; 
    public bool IsInitialized => isInitialized; 

    public static GameManager Instance; 

    #region ManagerClass
    CountManager countManager;
    PauseManager pauseManager;
    CharacterManager characterManager;
    UIManager uiManager;
    PoolManager poolManager;
    MapManager mapManager;
    GameModeManager gameModeManager;
    SpeedScaleManager speedScaleManager;
    #endregion
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        GetAndAdd();
    }
    void Update()
    {
        if (!isInitialized) return;
        if (pauseManager.IsHardPaused) return;
        float dtUnscaled = Time.unscaledDeltaTime;
        countManager.Tick(dtUnscaled);
        if (pauseManager.BlockGameplayTick) return;
        if (!countManager.IsGameActive) return;
        float dt = Time.deltaTime;
        characterManager.Tick(dt);
    }
    void FixedUpdate()
    {
        if (!isInitialized) return;
        if (pauseManager.IsHardPaused) return;
        if(pauseManager.BlockGameplayTick) return;
        if (!countManager.IsGameActive) return;
        float fdt = Time.fixedDeltaTime;
        characterManager.FixedTick(fdt);
    }

    void GetAndAdd()
    {
        countManager ??= GetComponent<CountManager>() ?? gameObject.AddComponent<CountManager>();
        pauseManager ??= GetComponent<PauseManager>() ?? gameObject.AddComponent<PauseManager>();
        characterManager ??= GetComponent<CharacterManager>() ?? gameObject.AddComponent<CharacterManager>();
        uiManager ??= GetComponent<UIManager>() ?? gameObject.AddComponent<UIManager>();
        poolManager ??= GetComponent<PoolManager>() ?? gameObject.AddComponent<PoolManager>();
        mapManager ??= GetComponent<MapManager>() ?? gameObject.AddComponent<MapManager>();
        gameModeManager ??= GetComponent<GameModeManager>() ?? gameObject.AddComponent<GameModeManager>();
        speedScaleManager ??= GetComponent<SpeedScaleManager>() ?? gameObject.AddComponent<SpeedScaleManager>();

        StartCoroutine(StartInitialize());
    }
    IEnumerator StartInitialize()
    {
        yield return StartCoroutine(ManagerInitializer.InitializeAll());
        isInitialized = true;
    }

    void StartExit()
    { 
        ManagerInitializer.ExitAll();
    }
    void OnApplicationQuit()
    {
        StartExit();
    }
}
