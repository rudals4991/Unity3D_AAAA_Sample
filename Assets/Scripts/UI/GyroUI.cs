using UnityEngine;

public class GyroUI : MonoBehaviour,IUIBase,ITickUI
{
    [SerializeField] RectTransform dot;
    [SerializeField] RectTransform line;
    float smooth = 10f;
    Player player;
    float halfWidth; 
    float currentX;

    public void Initialize()
    {
        RecalculateRange();
        currentX = 0f;
        SetDotX(0f);
    }
    private void OnEnable()
    {
        RecalculateRange();
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void Tick(float dt)
    {
        if (halfWidth <= 0f && line.rect.width > 0f) RecalculateRange();
        if (player == null)
        {
            if(!ResolvePlayer()) return;
        }
        float tilt = Mathf.Clamp(player.PlayerInput.GetTilt(), -1f, 1f);
        float targetX = tilt * halfWidth;
        if (smooth <= 0f) currentX = targetX;
        else
        {
            float t = 1f - Mathf.Exp(-smooth * dt);
            currentX = Mathf.Lerp(currentX, targetX, t);
        }
        SetDotX(currentX);
    }

    void RecalculateRange()
    {
        float railWidth = line.rect.width;
        float dotWidth = dot.rect.width;
        halfWidth = Mathf.Max(0f, (railWidth - dotWidth) * 0.5f);
    }
    void SetDotX(float x)
    {
        Vector2 pos = dot.anchoredPosition;
        pos.x = x;
        dot.anchoredPosition = pos;
    }
    bool ResolvePlayer()
    {
        try
        {
            player = DIContainer.Resolve<Player>();
            return player != null;
        }
        catch
        {
            return false;
        }
    }
}

