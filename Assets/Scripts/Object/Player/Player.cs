using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData data;

    // Player Components
    public Rigidbody Rb { get; private set; }                  
    public CapsuleCollider Capsule { get; private set; }    
    public Animator Animator { get; private set; }
    public PlayerAutoMove PlayerAutoMove { get; private set; }
    public PlayerGyroMove PlayerGyroMove {get; private set;}
    public PlayerJump PlayerJump {get; private set;}
    public PlayerInput PlayerInput { get; private set; }
    public PlayerCollisionController CollisionController { get; private set; }
    public PlayerAnimation PlayerAnimation { get; private set; }

    // SO Setting
    public float MoveSpeed => data.moveSpeed;         
    public float GyroSpeedLeftRight => data.gyroSpeedLeftRight;
    public float GyroSpeedForward => data.gyroSpeedForward;     
    public float DeadZone => data.deadZone;                     
    public float Sensitivity => data.sensitivity;              
    public float FallMultiplier => data.fallMultiplier;     
    public float JumpForce => data.jumpForce;                   
    public int JumpCount_Back => data.maxJumpCount_BackView;   
    public int JumpCount_Side => data.maxJumpCount_SideView;   
    public GameMode CurrentMode { get; private set; }
    
    // ForState
    bool canAutoMove;
    bool canGyroMove;
    bool canJump;

    // Speed Setting
    public float SpeedScale { get; private set; } = 1f;
    public float CurrentGyroSpeed_LeftRight { get; private set; }
    public float CurrentGyroSpeed_Forward { get; private set; }
    public float CurrentMoveSpeed { get; private set; }
    public float CurrentGyroSensitivity { get; private set; }

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
        CurrentGyroSpeed_Forward = GyroSpeedForward;
        CurrentGyroSpeed_LeftRight = GyroSpeedLeftRight;
        CurrentGyroSensitivity = Mathf.Clamp(PlayerPrefs.GetFloat("GyroSen", Sensitivity), 0.6f, 1.8f);
        ApplyScale(1f);
        ApplyGameMode(mode);
        SpeedScaleManager.OnSpeedScaleChanged -= ApplyScale;
        SpeedScaleManager.OnSpeedScaleChanged += ApplyScale;
    }
    public void Tick(float dt)
    {
        if(canAutoMove) PlayerAutoMove.Tick(dt);
        if(canGyroMove) PlayerGyroMove.Tick(dt);
        if (canJump) if (PlayerInput.GetJump()) PlayerJump.RequestJump();
        Vector3 v = Rb != null ? Rb.linearVelocity : Vector3.zero;
        v.y = 0f;
        bool isMoving = v.sqrMagnitude > 0.00001f;
        PlayerAnimation.SetMoveAnim(isMoving);
    }
    public void FixedTick(float fdt)
    {
        if (canAutoMove) PlayerAutoMove.FixedTick(fdt);
        if (canGyroMove) PlayerGyroMove.FixedTick(fdt);
        PlayerJump.FixedTick();
    }

    public void ApplyGameMode(GameMode gameMode)
    {
        CurrentMode = gameMode;
        CollisionController.SetDefault();
        if (Rb != null)
        {
            Rb.angularVelocity = Vector3.zero;
            Rb.rotation = Quaternion.identity;
        }
        transform.rotation = Quaternion.identity;
        PlayerGyroMove.ResetCache();
        PlayerJump.ResetJumpState();
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
    void ApplyScale(float scale)
    { 
        SpeedScale = scale;
        CurrentMoveSpeed = MoveSpeed * SpeedScale;
        CurrentGyroSpeed_Forward = GyroSpeedForward * SpeedScale;
        CurrentGyroSpeed_LeftRight = GyroSpeedLeftRight * SpeedScale;
    }
    public void ApplySensitivity(float value)
    { 
        CurrentGyroSensitivity = value;
    }
}
