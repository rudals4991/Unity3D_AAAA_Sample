using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    int maxJumpCount;
    private int _jumpCount;
    int jumpCount 
    {
        get { return _jumpCount; } 
        set {
            _jumpCount = value; 
        }
    }
    bool jumpRequested = false;
    bool isGround = false;
    bool wasGround = false;

    Player player;

    // 땅 감지를 위한 땅 레이어
    public LayerMask groundLayer;

    public void Initialize(Player player)
    {
        this.player = player;
        ResetJumpState();
    }
    public void Jump(float dt)
    {

        // 땅 감지 상태 갱신
        isGround = CheckGround();

        // 땅을 감지하지 못한 경우
        //if (!isGround)
        //{
        //    return;
        //}

        // 착지했을 경우
        if (!wasGround && isGround)
        {
            wasGround = isGround;
            ResetJumpState();
        }

        // 점프 처리
        // 점프 요청이 존재하는 경우
        if (jumpRequested)
        {
            // 점프 시도
            TryJump();
        }
        ApplyGravity(dt);
    }
    public void RequestJump()
    {
        if (!CanJump()) return;
        jumpRequested = true;
    }
    bool CheckGround()
    {
        float dist = 0.2f;
        wasGround = isGround;

        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Vector3 direction = Vector3.down;

        Ray ray = new Ray(origin, direction);

        return Physics.Raycast(ray, out RaycastHit hitInfo, dist, groundLayer, QueryTriggerInteraction.Ignore);


        //if (!wasGround && isGround) jumpCount = 0;
    }
    public void JumpByPlatform(float customForce)
    {
        Rigidbody rb = player.Rb;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * customForce, ForceMode.Impulse);
        jumpCount = 1;
    }

    bool CanJump()
    {
        return jumpCount < maxJumpCount;
    }

    void TryJump()
    {
        if (!CanJump()) return;

        jumpRequested = false;
        Debug.Log($"jumpCount({jumpCount}), maxJumpCount({maxJumpCount})");
        //Debug.Log($"now maxJump = {maxJumpCount}");
        Rigidbody rb = player.Rb;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * player.JumpForce, ForceMode.Impulse);
        jumpCount++;
    }
    void ApplyGravity(float dt)
    {
        if (player.Rb.linearVelocity.y < 0)
        {
            player.Rb.AddForce(Vector3.up * Physics.gravity.y * (player.FallMultiplier - 1f),
                ForceMode.Acceleration);
        }
    }
    public void SetJumpCountByMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.BackView_ToForward: maxJumpCount = player.JumpCount_Back; break;
            case GameMode.SideView_ToRight: maxJumpCount = player.JumpCount_Side; break;
            default:maxJumpCount = 0; break;
        }
    }
    public void ResetJumpState()
    {
        jumpCount = 0;
        wasGround = false;
        //isGround = false;
        //canJump = false;
    }
}
