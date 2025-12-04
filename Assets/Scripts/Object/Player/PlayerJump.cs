using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    int maxJumpCount = 2;
    int jumpCount = 0;
    bool isGround = false;
    Player player;

    public void Initialize(Player player)
    {
        this.player = player;
    }
    public void Jump(float dt)
    {
        CheckGround();
        if (Input.GetMouseButtonDown(0))TryJump();
        ApplyGravity(dt);
    }
    void CheckGround()
    {
        float dist = player.Capsule.bounds.extents.y + 0.1f;
        if (Physics.Raycast(transform.position, Vector3.down, dist))
        { 
            isGround = true;
            jumpCount = 0;
        }
        else isGround = false;
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
}
