using UnityEngine;

public class GyroTest : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float deadZone = 0.1f;      // 자이로 허용 범위
    [SerializeField] private float sensitivity = 1.2f;   // 반응 감도 (1~2 추천)
    private void Start()
    {
#if UNITY_ANDROID
        Input.gyro.enabled = true;
#endif
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
        Quaternion q = Input.gyro.attitude;

        // Step1: Unity 좌표계 변환
        q = new Quaternion(q.x, q.y, -q.z, -q.w);

        // Step2: Portrait → Landscape Left 변환
        q = Quaternion.Euler(90, 0, -90) * q;

        // Step3: 좌우 기울기(roll) 계산
        float roll = Mathf.Atan2(2 * (q.w * q.z + q.x * q.y),
                                 1 - 2 * (q.y * q.y + q.z * q.z)) * Mathf.Rad2Deg;

        roll = Mathf.Clamp(roll, -45f, 45f);

        float tilt = roll / 45f;

        if (Mathf.Abs(tilt) < deadZone)
            tilt = 0f;

        return -tilt * sensitivity;
    }
    private float GetEditorTilt()
    {
        float t = 0f;

        if (Input.GetKey(KeyCode.A)) t -= 1f;
        if (Input.GetKey(KeyCode.D)) t += 1f;

        return t;
    }
}
