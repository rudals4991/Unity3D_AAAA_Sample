using UnityEngine;

public class GyroInput : MonoBehaviour
{
    Player player;
    public void Initialize(Player player)
    { 
        this.player = player;
#if UNITY_ANDROID
        Input.gyro.enabled = true;
#endif
    }
    public float GetTilt()
    {
#if UNITY_ANDROID
        return Mathf.Clamp(Input.gyro.gravity.x * player.Sensitivity, -1f, 1f);

#else
        return GetEditorTilt();
#endif
    }

    private float GetEditorTilt()
    {
        float t = 0f;
        if (Input.GetKey(KeyCode.A)) t -= 1f;
        if (Input.GetKey(KeyCode.D)) t += 1f;
        return t;
    }
}
