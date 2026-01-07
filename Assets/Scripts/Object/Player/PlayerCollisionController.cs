using System.Collections;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    Player player;
    bool capsuleDisabled = false;
    Coroutine pendingRoutine;
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
        capsuleDisabled = false;
        player.Capsule.enabled = true;
    }
    void HandleJumpStart()
    {
        if (player.CurrentMode != GameMode.SideView_ToTop && player.CurrentMode != GameMode.SideView_ToDown)
            return;
        if (pendingRoutine != null) StopCoroutine(pendingRoutine);
        pendingRoutine = StartCoroutine(DisableCapsule());
    }
    void HandleJumpApex()
    {
        if (player.CurrentMode != GameMode.SideView_ToTop && player.CurrentMode != GameMode.SideView_ToDown)
            return;
        if (!capsuleDisabled) return;
        capsuleDisabled = false;
        player.Capsule.enabled = true;
    }
    IEnumerator DisableCapsule()
    {
        yield return new WaitForFixedUpdate();
        if (player == null || player.Rb == null || player.Capsule == null) yield break;
        if (player.Rb.linearVelocity.y > 0.05f && !capsuleDisabled)
        {
            capsuleDisabled = true;
            player.Capsule.enabled = false;
        }
    }
}
