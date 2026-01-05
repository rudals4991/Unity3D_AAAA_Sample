using UnityEngine;

public class CameraScrollController : MonoBehaviour
{
    [SerializeField] float baseScrollSpeed = 2.0f;
    [SerializeField] Vector3 upDir = Vector3.up;
    [SerializeField] Vector3 downDir = Vector3.down;
    GameFlowManager flow;
    Vector3 scrollDir;
    bool modeActive;
    float speedScale = 1f;
    void Awake()
    {
        DIContainer.Register(this);

        flow = DIContainer.Resolve<GameFlowManager>();

        GameModeManager.OnGameModeChanged -= ApplyGameMode;
        GameModeManager.OnGameModeChanged += ApplyGameMode;

        SpeedScaleManager.OnSpeedScaleChanged -= ApplySpeedScale;
        SpeedScaleManager.OnSpeedScaleChanged += ApplySpeedScale;

        GameFlowManager.OnGameOvered -= OnGameOver;
        GameFlowManager.OnGameOvered += OnGameOver;
    }
    void ApplySpeedScale(float scale)
    {
        speedScale = scale;
    }

    void ApplyGameMode(GameMode mode)
    {
        modeActive = false;

        switch (mode)
        {
            case GameMode.SideView_ToTop:
                scrollDir = upDir;
                modeActive = true;
                break;

            case GameMode.SideView_ToDown:
                scrollDir = downDir;
                modeActive = true;
                break;
        }
    }
    void OnGameOver(GameoverReason _)
    {
        modeActive = false;
    }
    void LateUpdate()
    {
        if (!modeActive) return;
        if (flow != null && !flow.CanGameplay) return;
        float speed = baseScrollSpeed * speedScale;
        transform.position += scrollDir * speed * Time.deltaTime;
    }
    public void SetBaseSpeed(float speed) => baseScrollSpeed = speed;
    public void Stop() => modeActive = false;
}
