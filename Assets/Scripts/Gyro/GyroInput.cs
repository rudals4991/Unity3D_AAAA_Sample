using UnityEngine;

public class GyroInput : MonoBehaviour
{
    float sensitivity = 1.2f;

    void Start()
    {
#if UNITY_ANDROID
        Input.gyro.enabled = true;
#endif
    }
    public float GetTilt()
    {
#if UNITY_ANDROID
        return Mathf.Clamp(Input.gyro.gravity.x * sensitivity, -1f, 1f);

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
