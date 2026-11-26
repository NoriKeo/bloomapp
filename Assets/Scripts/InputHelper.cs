using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public static class InputHelper
{
    public static bool TryGetPointerDownScreenPosition(out Vector2 screenPos, out int pointerId)
    {
        // 1. Touch (Handy / Tablet)
        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.phase.ReadValue() == TouchPhase.Began)
                {
                    screenPos = touch.position.ReadValue();
                    pointerId = touch.touchId.ReadValue();
                    return true;
                }
            }
        }

        // 2. Maus (Unity-Editor / Desktop)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            screenPos = Mouse.current.position.ReadValue();
            pointerId = -1;
            return true;
        }

        screenPos = Vector2.zero;
        pointerId = -1;
        return false;
    }

    public static bool IsPointerOverUI(int pointerId)
    {
        if (EventSystem.current == null) return false;

        return pointerId >= 0
            ? EventSystem.current.IsPointerOverGameObject(pointerId)
            : EventSystem.current.IsPointerOverGameObject();
    }

    public static bool HitCollider2D(Vector2 screenPos, Collider2D targetCollider)
    {
        if (Camera.main == null || targetCollider == null) return false;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
        Collider2D hit = Physics2D.OverlapPoint(worldPoint);
        return hit == targetCollider;
    }
}
