using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed { get; private set; } = 5f;
    public float GyroSpeedLeftRight { get; private set; } = 5f;
    public float GyroSpeedForward { get; private set; } = 5f;
    public float JumpForce { get; private set; } = 4f;
    public float DeadZone { get; private set; } = 0.1f;
    public float FallMultiplier { get; private set; } = 2f;

    public Rigidbody Rb { get; private set; }
    public CapsuleCollider Capsule { get; private set; }
    public GyroInput GyroInput { get; private set; }

    public PlayerAutoMove PlayerAutoMove { get; private set; }
    public PlayerGyroMove PlayerGyroMove {get; private set;}
    public PlayerJump PlayerJump {get; private set;}

    bool canAutoMove;
    bool canGyroMove;
    bool canJump;

    public void Initialize(GameMode mode)
    {
        Rb = GetComponent<Rigidbody>();
        Capsule = GetComponent<CapsuleCollider>();
        GyroInput = GetComponent<GyroInput>();

        PlayerAutoMove = GetComponent<PlayerAutoMove>();
        PlayerGyroMove = GetComponent<PlayerGyroMove>();
        PlayerJump = GetComponent<PlayerJump>();

        PlayerAutoMove.Initialize(this);
        PlayerGyroMove.Initialize(this);
        PlayerJump.Initialize(this);

        ApplyGameMode(mode);
    }
    public void Tick(float dt)
    {
        if(canAutoMove) PlayerAutoMove.AutoMove(dt);
        if(canGyroMove) PlayerGyroMove.GyroMove(dt);
        if(canJump) PlayerJump.Jump(dt);
    }
    void ApplyGameMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.BackView_ToForward:
                {
                    canAutoMove = true;
                    canGyroMove = true;
                    canJump = true;
                    PlayerGyroMove.SetGyroMode(GyroMode.LeftRight);
                } break;
            case  GameMode.SideView_ToRight:
                {
                    canAutoMove = true;
                    canGyroMove = false;
                    canJump = true;
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
}
