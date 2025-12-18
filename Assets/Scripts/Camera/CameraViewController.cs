using UnityEngine;

public class CameraViewController : MonoBehaviour
{
    Transform target;
    [Header("Offsets")]
    [SerializeField] Vector3 sideOffset = new Vector3(6, 0, 6);        // Side Left->Right
    [SerializeField] Vector3 topOffset = new Vector3(0, 0.2f, -11);   // Down->Up
    [SerializeField] Vector3 downOffset = new Vector3(0, 0.2f, -11);   // Up->Down
    [SerializeField] Vector3 backOffset = new Vector3(0, 3.5f, -4);    // BackView

    [Header("Follow")]
    [SerializeField] float rotFollowSpeed = 8f;
    [SerializeField] float posFollowSpeed = 5f;

    [Header("Mode Transition")]
    [SerializeField] float modeBlendDuration = 1.0f;
    AnimationCurve modeBlendCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 targetPos;
    Quaternion targetRot;
    GameMode gameMode;

    float speedScale = 0.2f;
    public float CameraSpeedScale { get; private set; } = 1f;

    bool hasInitialized = false;
    bool isTransitioning = false;
    float transitionElapsed = 0f;

    Vector3 fromOffset, toOffset;
    Quaternion fromRot, toRot;

    void Awake()
    {
        DIContainer.Register(this);
        GameModeManager.OnGameModeChanged -= SetCameraMode;
        GameModeManager.OnGameModeChanged += SetCameraMode;
    }


    public void SetCameraSpeedScale(float scale)
    {
        CameraSpeedScale = scale;
    }
    public void IncreaseCameraSpeedStep()
    {
        CameraSpeedScale = CameraSpeedScale + speedScale;
    }
    public void ResetCameraSpeedScale(float scale = 1f)
    {
        CameraSpeedScale = scale;
    }
    public void SetTarget(Transform t)
    {
        target = t;
        hasInitialized = false;
    }
    public void SetCameraMode(GameMode mode)
    {
        gameMode = mode;
        if (target == null) return;
        Vector3 nextOffset = GetOffset(mode);
        Quaternion nextRot = GetRotation(mode);
        if (!hasInitialized)
        {
            targetPos = target.position + nextOffset;
            targetRot = nextRot;
            transform.position = targetPos;
            transform.rotation = targetRot;
            hasInitialized = true;
            isTransitioning = false;
            return;
        }
        BeginTransition(nextOffset, nextRot);
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (isTransitioning)
        {
            transitionElapsed += Time.deltaTime;
            float n = Mathf.Clamp01(transitionElapsed / modeBlendDuration);
            float k = modeBlendCurve.Evaluate(n);
            Vector3 blendedOffset = Vector3.LerpUnclamped(fromOffset, toOffset, k);
            Quaternion blendedRot = Quaternion.Slerp(fromRot, toRot, k);
            targetPos = target.position + blendedOffset;
            targetRot = blendedRot;
            ApplyCamera(applyConstraints: false);
            if (n >= 1f) isTransitioning = false;
            return;
        }
        targetPos = target.position + GetOffset(gameMode);
        targetRot = GetRotation(gameMode);
        ApplyCamera(applyConstraints: true);
    }
    void BeginTransition(Vector3 nextOffset, Quaternion nextRot)
    {
        fromOffset = transform.position - target.position;
        fromRot = transform.rotation;
        toOffset = nextOffset;
        toRot = nextRot;
        transitionElapsed = 0f;
        isTransitioning = true;
    }

    Vector3 GetOffset(GameMode mode)
    {
        return mode switch
        {
            GameMode.BackView_ToForward => backOffset,
            GameMode.SideView_ToRight => sideOffset,
            GameMode.SideView_ToTop => topOffset,
            GameMode.SideView_ToDown => downOffset,
            _ => backOffset,
        };
    }

    Quaternion GetRotation(GameMode mode)
    {
        return mode switch
        {
            GameMode.BackView_ToForward => Quaternion.Euler(20f, 0f, 0f),
            GameMode.SideView_ToTop => Quaternion.Euler(15f, 0f, 0f),
            GameMode.SideView_ToDown => Quaternion.Euler(15f, 0f, 0f),
            GameMode.SideView_ToRight => Quaternion.Euler(0f, -90f, 0f),
            _ => Quaternion.Euler(20f, 0f, 0f),
        };
    }

    void ApplyCamera(bool applyConstraints)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotFollowSpeed);
        Vector3 pos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * posFollowSpeed);

        if (applyConstraints)
        {
            if (gameMode == GameMode.BackView_ToForward)
                pos.x = Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * 2f);
            if (gameMode == GameMode.SideView_ToRight)
                pos.y = transform.position.y;
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
        }
        transform.position = pos;
    }
}
