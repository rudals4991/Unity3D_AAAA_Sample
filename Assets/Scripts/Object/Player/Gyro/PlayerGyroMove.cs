using UnityEngine;

public class PlayerGyroMove : MonoBehaviour
{
    GyroMode gyroMode;
    Player player;

    public void Initialize(Player player)
    {
        this.player = player;
    }
    public void SetGyroMode(GyroMode mode)
    { 
        gyroMode = mode;
    }
    public void GyroMove(float dt)
    {
        float tilt = player.GyroInput.GetTilt();
        if (Mathf.Abs(tilt) < player.DeadZone) return;
        switch (gyroMode)
        {
            case GyroMode.LeftRight: MoveLeftRight(tilt,dt); break;
            case GyroMode.Forward: MoveForward(tilt,dt); break;
        }
    }
    void MoveLeftRight(float tilt, float dt)
    {
        float speed = tilt * player.GyroSpeedLeftRight;
        transform.position += Vector3.right * speed * dt;
    }
    void MoveForward(float tilt, float dt)
    {
        if (tilt > 0) transform.rotation = Quaternion.Euler(0, 90, 0);
        else transform.rotation = Quaternion.Euler(0,-90,0);
        float speed = Mathf.Abs(tilt) * player.GyroSpeedForward;
        transform.position += transform.forward * speed * dt;
    }
}
