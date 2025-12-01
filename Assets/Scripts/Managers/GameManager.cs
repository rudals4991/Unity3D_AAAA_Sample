using System.Collections;
using UnityEngine;

//게임 시작 시, Manager 클래스들을 초기화시켜주는 최상위 Manager
public class GameManager : MonoBehaviour
{
    // GameManager에서 다른 모든 Manager 클래스들의 초기화를 담당합니다.
    // Manager 클래스들의 객체를 연결하려는 클래스들의 경우
    // DI를 통해 Resolve하는 방식으로 사용하는데 DI의 등록이 Manager.Initialize()에서 이루어지므로
    // isInitialized라는 플래그를 통해 DI 등록 전에 Manager 클래스들에게의 접근을 방지합니다.
    private bool isInitialized = false;
    public bool IsInitialized => isInitialized; //외부접근용

    public static GameManager Instance; //싱글톤

    //Manager 맴버
    #region ManagerClass
    private AudioManager audioManager;
    private CharacterManager characterManager;
    private LoadingManager loadingManager;
    private PoolManager poolManager;
    private StartManager startManager;
    private UIManager uiManager;
    private ViewModeManager viewModeManager;
    private StageManager stageManager;
    private SpeedManager speedManager;
    private CameraManager cameraManager;
    private InputManager inputManager;
    private GameStateManager gameStateManager;
    #endregion
    private void Awake()
    {
        if (Instance is not null) Destroy(gameObject);
        else
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GetAndAdd();
        }
    }
    private void Update()
    {
        if (!isInitialized) return; //아직 초기화가 완료되지 않았을 경우, 의미가 없으므로 return으로 방어
        // Tick()을 사용하는 이유: 스크립트들의 Update 호출 순서를 보장
        float dt = Time.deltaTime;
        characterManager.Tick(dt);
    }
    private void GetAndAdd()
    {
        // 별도의 GameObject를 만드는게 아닌 GameManager Object에 AddComponet를 통해 추가합니다.
        audioManager ??= GetComponent<AudioManager>() ?? gameObject.AddComponent<AudioManager>();
        characterManager ??= GetComponent<CharacterManager>() ?? gameObject.AddComponent<CharacterManager>();
        loadingManager ??= GetComponent<LoadingManager>() ?? gameObject.AddComponent<LoadingManager>();
        poolManager ??= GetComponent<PoolManager>() ?? gameObject.AddComponent<PoolManager>();
        startManager ??= GetComponent<StartManager>() ?? gameObject.AddComponent<StartManager>();
        uiManager ??= GetComponent<UIManager>() ?? gameObject.AddComponent<UIManager>();
        viewModeManager ??= GetComponent<ViewModeManager>() ?? gameObject.AddComponent<ViewModeManager>();
        stageManager ??= GetComponent<StageManager>() ?? gameObject.AddComponent<StageManager>();
        speedManager ??= GetComponent<SpeedManager>() ?? gameObject.AddComponent<SpeedManager>();
        cameraManager ??= GetComponent<CameraManager>() ?? gameObject.AddComponent<CameraManager>();
        inputManager ??= GetComponent<InputManager>() ?? gameObject.AddComponent<InputManager>();
        gameStateManager ??= GetComponent<GameStateManager>() ?? gameObject.AddComponent<GameStateManager>();

        StartCoroutine(StartInitialize());
    }
    private IEnumerator StartInitialize()
    {
        // 최상위 Manager인 GameManager에서 하위 Manager들의 초기화를 시작합니다.
        yield return StartCoroutine(ManagerInitializer.InitializeAll());
        isInitialized = true;
    }
    private void StartExit()
    { 
        //하위 Manager들의 종료를 시작합니다.
        ManagerInitializer.ExitAll();
    }
    private void OnApplicationQuit()
    {
        // 게임(어플)이 종료되었을 때 자동으로 호출됩니다.
        StartExit();
    }
}
