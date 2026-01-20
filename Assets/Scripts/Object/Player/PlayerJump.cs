using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    int maxJumpCount;
    int _jumpCount;

    public event Action OnJumpStarted;
    public event Action OnJumpApex;

    bool jumpRequested = false;
    //bool isGround = false;
    public bool isGround { get; private set; } = false;
    //bool wasGround = false;

    Player player;
    public LayerMask groundLayer;

    bool wasAscending = false;

    bool platformJumpPending = false;
    float pendingPlatformForce = 0f;
    float groundLockTimer = 0f;

    int jumpCount
    {
        get => _jumpCount;
        set => _jumpCount = value;
    }

    public void Initialize(Player player)
    {
        this.player = player;
        ResetJumpState();
    }
    public void FixedTick()
    {
        if (groundLockTimer > 0f) groundLockTimer -= Time.fixedDeltaTime;
        if (platformJumpPending)
        {
            platformJumpPending = false;
            Jump(pendingPlatformForce, consumeCount: true);
        }
        if (jumpRequested) TryJump();
        float vy = player.Rb.linearVelocity.y;
        bool isAscending = vy > 0f;
        if (wasAscending && !isAscending) OnJumpApex?.Invoke();
        wasAscending = isAscending;
        bool prevGround = isGround;
        isGround = (groundLockTimer > 0f) ? false : CheckGround();
        if (!prevGround && isGround) ResetJumpState();
        ApplyGravity();
    }
    void Jump(float force, bool consumeCount)
    {
        Rigidbody rb = player.Rb;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);
        if (consumeCount) jumpCount++;
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
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, dist, groundLayer, QueryTriggerInteraction.Ignore);
    }
    bool CanJump()
    {
        return jumpCount < maxJumpCount;
    }
    void TryJump()
    {
        if (!CanJump()) return;
        jumpRequested = false;
        groundLockTimer = 0.12f;
        isGround = false;
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
        wasAscending = false;
        platformJumpPending = false;
        pendingPlatformForce = 0f;
        jumpRequested = false;
    }
    public void RequestPlatformJump(float force)
    {
        platformJumpPending = true;
        pendingPlatformForce = force;
    }
}
