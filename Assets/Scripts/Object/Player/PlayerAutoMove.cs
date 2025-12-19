using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{
    Player player;

    public void Initialize(Player player)
    { 
        this.player = player;
    }
    public void Tick(float dt)
    { 
        
    }
    public void FixedTick(float fdt)
    {
        Vector3 v = player.Rb.linearVelocity;
        Vector3 forward = transform.forward.normalized;
        Vector3 want = forward * player.CurrentMoveSpeed;
        player.Rb.linearVelocity = new Vector3(want.x, v.y, want.z);
    }
}
