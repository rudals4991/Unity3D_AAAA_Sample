using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offsets")]
    [SerializeField] Vector3 sideOffset = new Vector3(-5, 2, 0);   // Side Left->Right
    [SerializeField] Vector3 topOffset = new Vector3(0, 5, -5);    // Down->Up
    [SerializeField] Vector3 downOffset = new Vector3(0, -5, -5);  // Up->Down
    [SerializeField] Vector3 backOffset = new Vector3(0, 3, -6);   // BackView

    private Vector3 targetPos;
    private Quaternion targetRot;
    private bool first = true;
    private GameMode currentMode;

    public void SetCameraMode(GameMode mode)
    {
        currentMode = mode;
        first = true;
    }
    private void LateUpdate()
    {
        if (target == null) return;
        UpdateCameraMode(currentMode);
        ApplyCamera();
    }
    private void UpdateCameraMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.SideView_ToRight:
                targetPos = target.position + sideOffset;
                targetRot = Quaternion.Euler(0f, -90f, 0f);
                break;

            case GameMode.BackView_ToForward:
                targetPos = target.position + backOffset;
                targetRot = Quaternion.Euler(20f, 0f, 0f);
                break;

            case GameMode.SideView_ToTop:
                targetPos = target.position + topOffset;
                targetRot = Quaternion.Euler(90f, 0f, 0f);
                break;

            case GameMode.SideView_ToDown:
                targetPos = target.position + downOffset;
                targetRot = Quaternion.Euler(-90f, 0f, 0f);
                break;
        }
    }
    private void ApplyCamera()
    {
        // 첫 프레임은 즉시 위치/회전 적용
        if (first)
        {
            transform.position = targetPos;
            transform.rotation = targetRot;
            first = false;
            return;
        }

        // 회전은 부드럽게
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * 8f
        );

        // 위치도 부드럽게 이동
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * 5f
        );

        // 백뷰 모드의 좌우 흔들림 제어
        if (currentMode == GameMode.BackView_ToForward)
        {
            float smoothedX = Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * 2f);
            transform.position = new Vector3(smoothedX, transform.position.y, transform.position.z);
        }
    }

}
