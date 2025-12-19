using UnityEngine;

public class PlayerGyroMove : MonoBehaviour
{
    GyroMode gyroMode;
    Player player;
    float cachedTilt;
    bool hasTilt;

    public void Initialize(Player player)
    {
        this.player = player;
    }
    public void SetGyroMode(GyroMode mode)
    { 
        gyroMode = mode;
        ResetCache();
    }
    public void ResetCache()
    {
        cachedTilt = 0f;
        hasTilt = false;
    }
    public void Tick(float dt)
    {
        float tilt = player.PlayerInput.GetTilt();

        if (Mathf.Abs(tilt) < player.DeadZone)
        {
            hasTilt = false;
            return;
        }
        cachedTilt = tilt;
        hasTilt = true;
    }
    public void FixedTick(float fdt)
    {
        if (!hasTilt) return;
        Vector3 v = player.Rb.linearVelocity;
        switch (gyroMode)
        {
            case GyroMode.LeftRight:
                {
                    float speed = cachedTilt * player.CurrentGyroSpeed_LeftRight;
                    player.Rb.linearVelocity = new Vector3(speed, v.y, v.z);
                    break;
                }
            case GyroMode.Forward:
                {
                    bool toRight = cachedTilt > 0f;
                    player.Rb.MoveRotation(Quaternion.Euler(0f, toRight ? 90f : -90f, 0f));
                    float speed = Mathf.Abs(cachedTilt) * player.CurrentGyroSpeed_Forward;
                    float x = toRight ? speed : -speed;
                    player.Rb.linearVelocity = new Vector3(x, v.y, 0f);
                    break;
                }
        }
    }
}
