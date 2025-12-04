using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{
    Player player;

    public void Initialize(Player player)
    { 
        this.player = player;
    }
    public void AutoMove(float dt)
    {
        transform.position += transform.forward * player.MoveSpeed * dt;
    }
}
