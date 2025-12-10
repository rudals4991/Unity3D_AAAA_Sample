
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    Player player;
    public void Initialize(Player player)
    { 
        this.player = player;
        player.PlayerJump.OnJumpStarted -= HandleJumpStart;
        player.PlayerJump.OnJumpApex -= HandleJumpApex;

        player.PlayerJump.OnJumpStarted += HandleJumpStart;
        player.PlayerJump.OnJumpApex += HandleJumpApex;
    }
    void HandleJumpStart()
    {
        player.Capsule.enabled = false;  
    }

    void HandleJumpApex()
    {
        player.Capsule.enabled = true;  
    }
}
