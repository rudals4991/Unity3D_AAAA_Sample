using UnityEngine;

public class GyroTest : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float deadZone = 0.1f;      // 자이로 허용 범위
    [SerializeField] private float sensitivity = 1.2f;   // 반응 감도

    [Header("Speed Setting")]
    [SerializeField] private float maxSpeed;

    private void Awake()
    {
        DIContainer.Register(this);
    }

    private void Start()
    {
#if UNITY_ANDROID
        Input.gyro.enabled = true;
#endif
    }
    public float GetHorizontalSpeed()
    {
        float tilt = GetHorizontalTilt();
        return tilt * maxSpeed ;
    }
    public float GetHorizontalTilt()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    return GetMobileTilt();     // 실제 안드로이드 기기
#else
        return GetEditorTilt();     // PC 테스트
#endif
    }
    private float GetMobileTilt()
    {
        Vector3 g = Input.gyro.gravity;
        float tilt = Mathf.Clamp(g.x, -1f, 1f);
        if (Mathf.Abs(tilt) < deadZone) tilt = 0f;
        return tilt * sensitivity;
    }
    private float GetEditorTilt()
    {
        float t = 0f;

        if (Input.GetKey(KeyCode.A)) t -= 1f;
        if (Input.GetKey(KeyCode.D)) t += 1f;

        return t;
    }
}
