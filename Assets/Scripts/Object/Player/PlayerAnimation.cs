using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Player player;
    public void Initialize(Player player)
    { 
        this.player = player;
        player.PlayerJump.OnJumpStarted -= SetJumpAnim;
        player.PlayerJump.OnJumpStarted += SetJumpAnim;
    }
    public void SetMoveAnim(bool isMoving)
    {
        player.Animator.SetBool("Move", isMoving);
    }
    public void SetMS(float value, float dampTime, float dt)
    {
        player.Animator.SetFloat("MS", value, dampTime, dt);
    }
    void SetJumpAnim()
    {
        player.Animator.SetTrigger("Jump");
    }
    public void SetDoubleJump()
    {
        player.Animator.SetTrigger("DoubleJump");
    }
}
