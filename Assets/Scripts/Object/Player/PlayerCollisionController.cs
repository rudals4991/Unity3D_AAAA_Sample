
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
    public void SetDefault()
    {
        player.Capsule.enabled = true;
    }
    void HandleJumpStart()
    {
        if(player.CurrentMode == GameMode.SideView_ToTop || player.CurrentMode == GameMode.SideView_ToDown)
            player.Capsule.enabled = false;  
    }

    void HandleJumpApex()
    {
        if (player.CurrentMode == GameMode.SideView_ToTop || player.CurrentMode == GameMode.SideView_ToDown)
            player.Capsule.enabled = true;  
    }
}
