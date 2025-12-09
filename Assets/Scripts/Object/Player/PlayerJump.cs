using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    int maxJumpCount;
    int jumpCount = 0;
    bool canJump = false;
    bool isGround = false;
    bool wasGround = false;

    Player player;

    public void Initialize(Player player)
    {
        this.player = player;
        ResetJumpState();
    }
    public void Jump(float dt)
    {
        CheckGround();
        if (canJump)
        {
            TryJump();
            canJump = false;
        }
        ApplyGravity(dt);
    }
    public void SetCanJump()
    { 
        canJump = true;
    }
    void CheckGround()
    {
        float dist = player.Capsule.bounds.extents.y + 0.1f;
        wasGround = isGround;
        isGround = Physics.Raycast(transform.position, Vector3.down, dist);
        if (!wasGround && isGround) jumpCount = 0;
    }
    public void JumpByPlatform(float customForce)
    {
        Rigidbody rb = player.Rb;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * customForce, ForceMode.Impulse);
        jumpCount = 1;
    }
    void TryJump()
    {
        if (jumpCount >= maxJumpCount) return;
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
        isGround = false;
        canJump = false;
    }
}
