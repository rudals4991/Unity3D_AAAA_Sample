using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData data;

    public Rigidbody Rb { get; private set; }                  
    public CapsuleCollider Capsule { get; private set; }    
    public Animator Animator { get; private set; }
        
    public PlayerAutoMove PlayerAutoMove { get; private set; }
    public PlayerGyroMove PlayerGyroMove {get; private set;}
    public PlayerJump PlayerJump {get; private set;}
    public PlayerInput PlayerInput { get; private set; }
    public PlayerCollisionController CollisionController { get; private set; }
    public PlayerAnimation PlayerAnimation { get; private set; }

    public float MoveSpeed => data.moveSpeed;                  
    public float CurrentMoveSpeed { get; private set; }        
    public float GyroSpeedLeftRight => data.gyroSpeedLeftRight;
    public float GyroSpeedForward => data.gyroSpeedForward;     
    public float DeadZone => data.deadZone;                     
    public float Sensitivity => data.sensitivity;              
    public float FallMultiplier => data.fallMultiplier;     
    public float JumpForce => data.jumpForce;                   
    public int JumpCount_Back => data.maxJumpCount_BackView;   
    public int JumpCount_Side => data.maxJumpCount_SideView;   
    public GameMode CurrentMode { get; private set; }
    
    bool canAutoMove;
    bool canGyroMove;
    bool canJump;

    Vector3 prevPos;

    public void Initialize(GameMode mode)
    {
        DIContainer.Register(this);
        Rb = GetComponent<Rigidbody>();
        Capsule = GetComponent<CapsuleCollider>();
        Animator = GetComponent<Animator>();

        PlayerAutoMove = GetComponent<PlayerAutoMove>();
        PlayerGyroMove = GetComponent<PlayerGyroMove>();
        PlayerJump = GetComponent<PlayerJump>();
        PlayerInput = GetComponent<PlayerInput>();
        CollisionController = GetComponent<PlayerCollisionController>();
        PlayerAnimation = GetComponent<PlayerAnimation>();

        PlayerAutoMove.Initialize(this);
        PlayerGyroMove.Initialize(this);
        PlayerJump.Initialize(this);
        PlayerInput.Initialize(this);
        CollisionController.Initialize(this);
        PlayerAnimation.Initialize(this);

        CurrentMoveSpeed = MoveSpeed;
        ApplyGameMode(mode);
        prevPos = transform.position;
    }

    public void Tick(float dt)
    {
        if(canAutoMove) PlayerAutoMove.AutoMove(dt);
        if(canGyroMove) PlayerGyroMove.GyroMove(dt);
        if (canJump)
        {
            if (PlayerInput.GetJump()) PlayerJump.RequestJump();
            PlayerJump.Jump(dt);
        }
        Vector3 delta = transform.position - prevPos;
        delta.y = 0f;
        bool isMoving = delta.sqrMagnitude > 0.00001f;
        PlayerAnimation.SetMoveAnim(isMoving);
        prevPos = transform.position;
    }

    public void ApplyGameMode(GameMode gameMode)
    {
        CurrentMode = gameMode;
        CollisionController.SetDefault();
        transform.rotation = Quaternion.identity;
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
        prevPos = transform.position;
    }
    public void SetMoveSpeed(float speed)
    { 
        CurrentMoveSpeed = speed;
    }
}
