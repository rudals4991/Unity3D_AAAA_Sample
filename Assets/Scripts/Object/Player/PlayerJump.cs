using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    int maxJumpCount;
    int _jumpCount;
    bool wasAscending = false;
    public event Action OnJumpStarted;
    public event Action OnJumpApex;
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

    public LayerMask groundLayer;

    public void Initialize(Player player)
    {
        this.player = player;
        ResetJumpState();
    }
    public void FixedTick()
    {
        float vy = player.Rb.linearVelocity.y;
        bool isAscending = vy > 0;
        if (wasAscending && vy <= 0) OnJumpApex?.Invoke();
        wasAscending = isAscending;
        isGround = CheckGround();
        if (!wasGround && isGround)
        {
            wasGround = isGround;
            ResetJumpState();
        }
        if (jumpRequested) TryJump();
        ApplyGravity();
    }
    void Jump(float force, bool consumeCount)
    {
        Rigidbody rb = player.Rb;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        if (consumeCount) jumpCount++;
        wasAscending = true;
        if (consumeCount && jumpCount == 1) OnJumpStarted?.Invoke();
        else if (consumeCount && jumpCount == 2) player.PlayerAnimation.SetDoubleJump();
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
        Vector3 direction = Vector3.down; Ray ray = new Ray(origin, direction); 
        return Physics.Raycast(ray, out RaycastHit hitInfo, dist, groundLayer, QueryTriggerInteraction.Ignore);
    }
    bool CanJump()
    {
        return jumpCount < maxJumpCount;
    }
    void TryJump()
    {
        if (!CanJump()) return;

        jumpRequested = false;
        Jump(player.JumpForce, consumeCount: true);
    }
    void ApplyGravity()
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
    }
    public void JumpByPlatform(float customForce)
    {
        jumpRequested = false;
        Jump(customForce, consumeCount: true);
    }
}
