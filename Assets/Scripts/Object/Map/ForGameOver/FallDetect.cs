using UnityEngine;

public class FallDetect : MonoBehaviour
{
    GameoverReason reason = GameoverReason.Fall;
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

    bool waitFirstVisible = false;
    bool seenVisibleOnce = false;

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
            waitFirstVisible = false;
            seenVisibleOnce = false;
        }
        else
        {
            needBaseline = false;
            baseLine = false;
            waitFirstVisible = true;
            seenVisibleOnce = false;
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
            if (player.transform.position.y < threshold)
            {
                player.PlaySFX(reason);
                gameFlowManager.GameOver(GameoverReason.Fall);
            }
            return;
        }
        if (currentMode == GameMode.SideView_ToTop || currentMode == GameMode.SideView_ToDown)
        {
            if (player == null) return;
            if (cam == null) cam = Camera.main;
            if (cam == null) return;
            Vector3 vp = cam.WorldToViewportPoint(player.transform.position);
            if (vp.z < 0f) return;
            bool inX = vp.x >= 0f && vp.x <= 1f;
            bool inY = vp.y >= 0f && vp.y <= 1f;
            bool inScreen = inX && inY;
            if (waitFirstVisible && !seenVisibleOnce)
            {
                if (inScreen) seenVisibleOnce = true; return;
            }
            bool outX = vp.x < -viewportMargin || vp.x > 1f + viewportMargin;
            bool outY = vp.y < -viewportMargin || vp.y > 1f + viewportMargin;
            if (outX || outY)
            {
                player.PlaySFX(reason);
                gameFlowManager.GameOver(GameoverReason.OutOfScreen);
            }
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
