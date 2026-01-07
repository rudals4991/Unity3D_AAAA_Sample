using UnityEngine;

public class CameraViewController : MonoBehaviour
{
    Transform target;
    [Header("Offsets")]
    [SerializeField] Vector3 sideOffset = new Vector3(6, 2, 6);        // Side Left->Right
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

    float cameraBaseSpeed = 1f;
    float currentSpeed = 1f;
    bool useInitialYCalibration = true;
    float expectedGroundY = 0f;

    bool hasInitialized = false;
    bool isTransitioning = false;
    float transitionElapsed = 0f;

    Vector3 fromOffset, toOffset;
    Quaternion fromRot, toRot;

    float sideRightBaseY;
    bool sideRightBaseYValid = false;
    float initialYBias = 0f;
    bool initialYBiasReady = false;
    bool IsScrollMode(GameMode m) => m == GameMode.SideView_ToTop || m == GameMode.SideView_ToDown;

    void Awake()
    {
        DIContainer.Register(this);
        GameModeManager.OnGameModeChanged -= SetCameraMode;
        GameModeManager.OnGameModeChanged += SetCameraMode;
        SpeedScaleManager.OnSpeedScaleChanged -= ApplyScale;
        SpeedScaleManager.OnSpeedScaleChanged += ApplyScale;
        ApplyScale(1f);
    }
    void ApplyScale(float scale)
    {
        currentSpeed = cameraBaseSpeed * scale;
    }
    public void SetTarget(Transform t)
    {
        target = t;
        hasInitialized = false;
        sideRightBaseYValid = false;
        if (useInitialYCalibration && target != null)
        {
            initialYBias = target.position.y - expectedGroundY;
            initialYBiasReady = true;
        }
        else
        {
            initialYBias = 0f;
            initialYBiasReady = false;
        }
    }
    public void SetCameraMode(GameMode mode)
    {
        gameMode = mode;
        if (target == null) return;
        Vector3 nextOffset = GetOffset(mode);
        Quaternion nextRot = GetRotation(mode);
        if (IsScrollMode(mode))
        {
            isTransitioning = false;
            transitionElapsed = 0f;
            targetPos = target.position + nextOffset;
            if (useInitialYCalibration && initialYBiasReady) targetPos.y -= initialYBias;
            targetRot = nextRot;
            transform.position = targetPos; 
            transform.rotation = targetRot;
            hasInitialized = true;
            return;
        }
        if (mode == GameMode.SideView_ToRight)
        {
            float y = target.position.y + sideOffset.y;
            if (useInitialYCalibration && initialYBiasReady && !hasInitialized) y -= initialYBias;
            sideRightBaseY = y;
            sideRightBaseYValid = true;
        }
        else sideRightBaseYValid = false;
        if (!hasInitialized)
        {
            targetPos = target.position + nextOffset;
            if (useInitialYCalibration && initialYBiasReady) targetPos.y -= initialYBias;
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
        if (IsScrollMode(gameMode))
        {
            targetRot = GetRotation(gameMode);
            float dt = Time.deltaTime;
            float rotT = 1f - Mathf.Exp(-(rotFollowSpeed * currentSpeed) * dt);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotT);
            return;
        }
        if (isTransitioning)
        {
            transitionElapsed += Time.deltaTime * currentSpeed;
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
        float dt = Time.deltaTime;

        // 프레임레이트 독립 보간 계수
        float rotT = 1f - Mathf.Exp(-(rotFollowSpeed * currentSpeed) * dt);
        float posT = 1f - Mathf.Exp(-(posFollowSpeed * currentSpeed) * dt);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotT);
        Vector3 pos = Vector3.Lerp(transform.position, targetPos, posT);

        if (applyConstraints)
        {
            // 아래 보정들도 동일한 방식으로 보간 계수를 사용하면 일관성이 좋아집니다.
            float xT = 1f - Mathf.Exp(-(2f * currentSpeed) * dt);
            float yT = 1f - Mathf.Exp(-(7f * currentSpeed) * dt);

            if (gameMode == GameMode.BackView_ToForward)
                pos.x = Mathf.Lerp(transform.position.x, targetPos.x, xT);

            if (gameMode == GameMode.SideView_ToRight)
            {
                if (!sideRightBaseYValid)
                {
                    sideRightBaseY = target.position.y + sideOffset.y;
                    if (useInitialYCalibration && initialYBiasReady && !hasInitialized)
                        sideRightBaseY -= initialYBias;
                    sideRightBaseYValid = true;
                }
                pos.y = sideRightBaseY;
            }

            if (gameMode == GameMode.SideView_ToTop)
            {
                pos.x = Mathf.Lerp(transform.position.x, targetPos.x, xT);
                pos.y = Mathf.Lerp(transform.position.y, target.position.y + topOffset.y, yT);
            }

            if (gameMode == GameMode.SideView_ToDown)
            {
                pos.x = Mathf.Lerp(transform.position.x, targetPos.x, xT);
                pos.y = Mathf.Lerp(transform.position.y, target.position.y + downOffset.y, yT);
            }
        }

        transform.position = pos;
    }
}
