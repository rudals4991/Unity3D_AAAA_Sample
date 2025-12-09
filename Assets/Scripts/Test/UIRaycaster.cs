using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;

public static class UIRaycaster
{
    public static bool IsPointerOverUI(int fingerID)
    { 
        if(EventSystem.current == null) return false;
        if (fingerID < 0 || fingerID >= Input.touchCount) return false;

        UnityEngine.Touch touch = Input.GetTouch(fingerID);

        PointerEventData eventData = new(EventSystem.current)
        {
            position = touch.position
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
