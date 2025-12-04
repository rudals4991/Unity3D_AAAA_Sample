using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offsets")]
    Vector3 sideOffset = new Vector3(6, 1, 5);   // Side Left->Right
    Vector3 topOffset = new Vector3(6, -2, 0);    // Down->Up
    Vector3 downOffset = new Vector3(10, 2, 0);  // Up->Down
    Vector3 backOffset = new Vector3(0, 3, -6);   // BackView

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
                targetRot = Quaternion.Euler(0f, -90f, 0f);
                break;

            case GameMode.SideView_ToDown:
                targetPos = target.position + downOffset;
                targetRot = Quaternion.Euler(0f, -90f, 0f);
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
        Vector3 pos = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * 5f
        );
        switch (currentMode)
        {
            case GameMode.BackView_ToForward:
                float smoothedX = Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * 2f);
                pos.x = smoothedX; break;
            case GameMode.SideView_ToRight:
                pos.y = transform.position.y; break;
            case GameMode.SideView_ToTop:
            case GameMode.SideView_ToDown:
                pos.x = transform.position.x; break;
        }

        transform.position = pos;
    }

}
