using UnityEngine;

public class FallDetect : MonoBehaviour
{
    float fallOffset = 5f;
    float viewportMargin = 0.05f;

    GameFlowManager gameFlowManager;
    CountManager countManager;
    Camera cam;
    Player player;
    GameMode currentMode;

    bool baseLine = false;
    float fallBaseY = 0;
    bool needBaseline = false;

    void Awake()
    {
        DIContainer.Register(this);
        gameFlowManager = DIContainer.Resolve<GameFlowManager>();
        countManager = DIContainer.Resolve<CountManager>();

        GameModeManager.OnGameModeChanged -= ModeChanged;
        GameModeManager.OnGameModeChanged += ModeChanged;
    }
    void ModeChanged(GameMode mode)
    { 
        currentMode = mode;
        if (mode == GameMode.BackView_ToForward || mode == GameMode.SideView_ToRight)
        {
            needBaseline = true;
            baseLine = false;
        }
        else
        {
            needBaseline = false;
            baseLine = false;
        }
    }
    public void Tick(float dt)
    {
        if (gameFlowManager != null && !gameFlowManager.CanGameplay) return;
        if (countManager != null && !countManager.IsGameActive) return;
        ResolvePlayer();
        if (currentMode == GameMode.BackView_ToForward || currentMode == GameMode.SideView_ToRight)
        {
            if (needBaseline) TryCaptureFallBaseline();
            if (!baseLine || player == null) return;
            float threshold = fallBaseY - fallOffset;
            if (player.transform.position.y < threshold) gameFlowManager.GameOver(GameoverReason.Fall);
            return;
        }
        if (currentMode == GameMode.SideView_ToTop || currentMode == GameMode.SideView_ToDown)
        {
            if (player == null) return;
            if (cam == null) cam = Camera.main;
            if (cam == null) return;

            Vector3 vp = cam.WorldToViewportPoint(player.transform.position);
            if (vp.z < 0f) return;
            bool outX = vp.x < -viewportMargin || vp.x > 1f + viewportMargin;
            bool outY = vp.y < -viewportMargin || vp.y > 1f + viewportMargin;
            if (outX || outY) gameFlowManager.GameOver(GameoverReason.OutOfScreen);
        }
    }
    void ResolvePlayer()
    {
        if (player != null) return;
        player = DIContainer.Resolve<Player>();
    }
    void TryCaptureFallBaseline()
    {
        if (player == null) return;

        fallBaseY = player.transform.position.y;
        baseLine = true;
        needBaseline = false;
    }
}
