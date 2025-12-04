using UnityEngine;

public class CameraViewController : MonoBehaviour
{
    Transform target;

    Vector3 sideOffset = new Vector3(6, 1, 5);   // Side Left->Right
    Vector3 topOffset = new Vector3(6, -2, 0);    // Down->Up
    Vector3 downOffset = new Vector3(6, 2, 0);  // Up->Down
    Vector3 backOffset = new Vector3(0, 3, -6);   // BackView

    Vector3 targetPos;
    Quaternion targetRot;
    bool first = true;
    GameMode gameMode;

    public void SetTarget(Transform t)
    { 
        target = t;
        first = true;
    }
    public void SetCameraMode(GameMode mode)
    {
        gameMode = mode;
        first = true;
        UpdateCameraMode();
    }
    void LateUpdate()
    {
        if (target == null) return;
        switch (gameMode)
        {
            case GameMode.BackView_ToForward: targetPos = target.position + backOffset; break;
            case GameMode.SideView_ToRight: targetPos = target.position + sideOffset; break;
            case GameMode.SideView_ToTop: targetPos = target.position + topOffset; break;
            case GameMode.SideView_ToDown: targetPos = target.position + downOffset; break;
        }
        ApplyCamera();
    }
    
    void UpdateCameraMode()
    {
        switch (gameMode)
        {
            case GameMode.BackView_ToForward: targetRot = Quaternion.Euler(20f, 0f, 0f); break;

            case GameMode.SideView_ToRight:
            case GameMode.SideView_ToTop:
            case GameMode.SideView_ToDown:targetRot = Quaternion.Euler(0f, -90f, 0f); break;
        }
    }
    void ApplyCamera()
    {
        if (first)
        {
            transform.position = targetPos;
            transform.rotation = targetRot;
            first = false;
            return;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 8f);
        Vector3 pos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);

        if (gameMode == GameMode.BackView_ToForward) pos.x = Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * 2f);
        if (gameMode == GameMode.SideView_ToRight) pos.y = transform.position.y; 

        if (gameMode == GameMode.SideView_ToTop)
        {
            pos.x = Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * 2f);
            pos.y = Mathf.Lerp(transform.position.y, target.position.y + topOffset.y, Time.deltaTime * 7f);
        }
        if (gameMode == GameMode.SideView_ToDown)
        {
            pos.x = Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * 2f);
            pos.y = Mathf.Lerp(transform.position.y, target.position.y + downOffset.y, Time.deltaTime * 7f);
        }

        transform.position = pos;
    }
}
