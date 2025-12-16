using System.Collections;
using UnityEngine;

//게임 시작 시, Manager 클래스들을 초기화시켜주는 최상위 Manager
public class GameManager : MonoBehaviour
{
    private bool isInitialized = false; //초기화 보장용
    public bool IsInitialized => isInitialized; //외부접근용

    public static GameManager Instance; //싱글톤

    //Manager 맴버
    #region ManagerClass
    CountManager countManager;
    PauseManager pauseManager;
    CharacterManager characterManager;
    UIManager uiManager;
    PoolManager poolManager;
    MapManager mapManager;
    GameModeManager gameModeManager;
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

    //각 Manager 클래스들을 GameManager가 관리하기 위한 메서드
    void GetAndAdd()
    {
        // 별도의 GameObject를 만드는게 아닌 GameManager Object에 AddComponet를 통해 추가합니다.
        countManager ??= GetComponent<CountManager>() ?? gameObject.AddComponent<CountManager>();
        pauseManager ??= GetComponent<PauseManager>() ?? gameObject.AddComponent<PauseManager>();
        characterManager ??= GetComponent<CharacterManager>() ?? gameObject.AddComponent<CharacterManager>();
        uiManager ??= GetComponent<UIManager>() ?? gameObject.AddComponent<UIManager>();
        poolManager ??= GetComponent<PoolManager>() ?? gameObject.AddComponent<PoolManager>();
        mapManager ??= GetComponent<MapManager>() ?? gameObject.AddComponent<MapManager>();
        gameModeManager ??= GetComponent<GameModeManager>() ?? gameObject.AddComponent<GameModeManager>();

        StartCoroutine(StartInitialize());
    }

    //Manager 초기화 메서드
    IEnumerator StartInitialize()
    {
        yield return StartCoroutine(ManagerInitializer.InitializeAll());
        isInitialized = true;
    }

    //Manager 종료 메서드
    void StartExit()
    { 
        ManagerInitializer.ExitAll();
    }

    //게임 종료 시, 안전보장용
    void OnApplicationQuit()
    {
        StartExit();
    }
}
