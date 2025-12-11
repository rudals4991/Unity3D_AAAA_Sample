using UnityEngine;

public class Player : MonoBehaviour
{
    //Player 오브젝트 데이터 값을 SO로 관리
    [SerializeField] PlayerData data;

    // Player 오브젝트 컴포넌트
    public Rigidbody Rb { get; private set; }                  
    public CapsuleCollider Capsule { get; private set; }     

    // Player 오브젝트 기능 컴포넌트(외부 접근용)          
    public PlayerAutoMove PlayerAutoMove { get; private set; }
    public PlayerGyroMove PlayerGyroMove {get; private set;}
    public PlayerJump PlayerJump {get; private set;}
    public PlayerInput PlayerInput { get; private set; }
    public PlayerCollisionController CollisionController { get; private set; }

    // Player 오브젝트 데이터 값(외부 접근용)
    public float MoveSpeed => data.moveSpeed;                   //이동속도 초기세팅
    public float CurrentMoveSpeed { get; private set; }         //현재 이동속도 (변경용)
    public float GyroSpeedLeftRight => data.gyroSpeedLeftRight; //자이로(백뷰 모드) 이동속도
    public float GyroSpeedForward => data.gyroSpeedForward;     //자이로(y축 이동 모드) 이동속도
    public float DeadZone => data.deadZone;                     //떨림 보정용
    public float Sensitivity => data.sensitivity;               //기울기에 따른 이동속도 증가폭
    public float FallMultiplier => data.fallMultiplier;         //중력 강화용
    public float JumpForce => data.jumpForce;                   //점프 높이
    public int JumpCount_Back => data.maxJumpCount_BackView;    //BackView에서의 최대 점프 횟수
    public int JumpCount_Side => data.maxJumpCount_SideView;    //SideView에서의 최대 점프 횟수
    
    // 모드별 기능 분리를 위한 Bool
    bool canAutoMove;
    bool canGyroMove;
    bool canJump;

    //초기화 메서드 (Awake, Start 대체)
    public void Initialize(GameMode mode)
    {
        Rb = GetComponent<Rigidbody>();
        Capsule = GetComponent<CapsuleCollider>();

        PlayerAutoMove = GetComponent<PlayerAutoMove>();
        PlayerGyroMove = GetComponent<PlayerGyroMove>();
        PlayerJump = GetComponent<PlayerJump>();
        PlayerInput = GetComponent<PlayerInput>();
        CollisionController = GetComponent<PlayerCollisionController>();

        PlayerAutoMove.Initialize(this);
        PlayerGyroMove.Initialize(this);
        PlayerJump.Initialize(this);
        PlayerInput.Initialize(this);
        CollisionController.Initialize(this);

        CurrentMoveSpeed = MoveSpeed;
        ApplyGameMode(mode);
    }

    //Update 대체
    public void Tick(float dt)
    {
        if(canAutoMove) PlayerAutoMove.AutoMove(dt);
        if(canGyroMove) PlayerGyroMove.GyroMove(dt);
        if (canJump)
        {
            if (PlayerInput.GetJump()) PlayerJump.RequestJump();
            PlayerJump.Jump(dt);
        }
    }

    //게임 모드별 기능 적용 메서드
    public void ApplyGameMode(GameMode gameMode)
    {
        Debug.Log($"mode : {gameMode}");
        switch (gameMode)
        {
            case GameMode.BackView_ToForward:
                {
                    canAutoMove = true;
                    canGyroMove = true;
                    canJump = true;
                    PlayerGyroMove.SetGyroMode(GyroMode.LeftRight);
                    PlayerJump.SetJumpCountByMode(gameMode);
                    PlayerJump.ResetJumpState();
                } break;
            case  GameMode.SideView_ToRight:
                {
                    canAutoMove = true;
                    canGyroMove = false;
                    canJump = true;
                    PlayerJump.SetJumpCountByMode(gameMode);
                    PlayerJump.ResetJumpState();
                } break;
            case GameMode.SideView_ToTop:

            case GameMode.SideView_ToDown:
                {
                    canAutoMove = false;
                    canGyroMove = true;
                    canJump = false;
                    PlayerGyroMove.SetGyroMode(GyroMode.Forward);
                } break;
        }
    }
    // 속도 조절용 메서드
    public void SetMoveSpeed(float speed)
    { 
        CurrentMoveSpeed = speed;
    }
}
