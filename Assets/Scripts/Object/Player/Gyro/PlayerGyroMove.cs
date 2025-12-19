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
        switch (gyroMode)
        {
            case GyroMode.LeftRight: MoveLeftRight(fdt); break;
            case GyroMode.Forward: MoveForward(fdt); break;
        }
    }
    void MoveLeftRight(float fdt)
    {
        float speed = cachedTilt * player.GyroSpeedLeftRight;
        Vector3 delta = Vector3.right * speed * fdt;
        player.Rb.MovePosition(player.Rb.position + delta);
    }
    void MoveForward(float fdt)
    {
        bool toRight = cachedTilt > 0f;
        Quaternion rot = Quaternion.Euler(0f, toRight ? 90f : -90f, 0f);
        player.Rb.MoveRotation(rot);
        float speed = Mathf.Abs(cachedTilt) * player.GyroSpeedForward;
        Vector3 dir = toRight ? Vector3.right : Vector3.left;
        Vector3 delta = dir * speed * fdt;
        player.Rb.MovePosition(player.Rb.position + delta);
    }
}
