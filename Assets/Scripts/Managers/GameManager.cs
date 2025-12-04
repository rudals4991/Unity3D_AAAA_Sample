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
    GameModeManager gameModeManager;
    CharacterManager characterManager;
    StageManager stageManager;
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
        float dt = Time.deltaTime;
        characterManager.Tick(dt);
    }
    private void GetAndAdd()
    {
        // 별도의 GameObject를 만드는게 아닌 GameManager Object에 AddComponet를 통해 추가합니다.
        gameModeManager ??= GetComponent<GameModeManager>() ?? gameObject.AddComponent<GameModeManager>();
        characterManager ??= GetComponent<CharacterManager>() ?? gameObject.AddComponent<CharacterManager>();
        stageManager ??= GetComponent<StageManager>() ?? gameObject.AddComponent<StageManager>();
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
