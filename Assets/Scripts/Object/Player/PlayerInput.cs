using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

//Player의 입력을 처리하는 클래스입니다.
public class PlayerInput : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    Player player;
    InputSystem_Actions actions;

    bool jumpPressed;
    public void Initialize(Player player)
    { 
        this.player = player;
        actions = new InputSystem_Actions();
        actions.Player.SetCallbacks(this);
        actions.Enable();

#if UNITY_ANDROID
        Input.gyro.enabled = true;
#endif
    }

    public float GetTilt()
    {
#if UNITY_ANDROID
        float tilt = Input.gyro.gravity.x * player.Sensitivity;
        tilt = Mathf.Clamp(tilt, -1f, 1f);
        if (Mathf.Abs(tilt) < player.DeadZone) tilt = 0f;
        return tilt;

#else
        float t = 0f;
        if (Keyboard.current.aKey.isPressed) t -= 1f;
        if (Keyboard.current.dKey.isPressed) t += 1f;
        return t;
#endif
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
#if UNITY_EDITOR
        // 에디터에서는 반드시 PC UI 체크
        if (IsPointerOverUI_PC())
            return;

#elif UNITY_ANDROID
    // 모바일 진짜 디바이스에서 돌 때만 모바일 UI 체크
    for (int i = 0; i < Input.touchCount; i++)
    {
        if (UIRaycaster.IsPointerOverUI(i)) return;
    }

#else
    if (IsPointerOverUI_PC()) return;

#endif
        jumpPressed = true;
    }
    public bool GetJump()
    {
        if (!jumpPressed) return false;
        jumpPressed = false; 
        return true;
    }
    bool IsPointerOverUI_PC()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}
