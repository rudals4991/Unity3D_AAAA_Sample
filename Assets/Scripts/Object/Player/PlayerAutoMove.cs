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
        Vector3 delta = player.CurrentMoveSpeed * transform.forward * fdt;
        player.Rb.MovePosition(player.Rb.position + delta);
    }
}
