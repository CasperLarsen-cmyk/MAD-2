using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static bool IsPressing(out Vector2 position)
    {
        position = Vector2.zero;

        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return true;
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            position = Camera.main.ScreenToWorldPoint(Touchscreen.current.position.ReadValue());
            return true;
        }

        if (Keyboard.current.aKey.isPressed ^ Keyboard.current.dKey.isPressed)
        {

            Vector2 dir = new ((-Keyboard.current.aKey.value + Keyboard.current.dKey.value) / 2 + 0.5f, 0);
            position = Camera.main.ViewportToWorldPoint(dir);
            return true;
        }

        return false;
    }
    static Vector2 lPos = Vector2.zero;

    public static float swipeSensitivity = 5f;
    public static bool swiping;
    public static bool swipeUp;
    public static bool swipeDown;
    public static bool swipeLeft;
    public static bool swipeRight;
    
    public void Update()
    {
        if (Touchscreen.current == null) return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Began)
        {
            //print("Began");
            lPos = touch.position.value;
        }

        if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            var diff = (touch.position.value - lPos) * Time.deltaTime;
            //print("Diff: " + diff.ToString());
            swiping = diff.magnitude > swipeSensitivity;
            swipeUp = swiping && diff.y > swipeSensitivity;
            swipeDown = swiping && diff.y < -swipeSensitivity;
            swipeLeft = swiping && diff.x < -swipeSensitivity;
            swipeRight = swiping && diff.x > swipeSensitivity;

            lPos = touch.position.value;

            if(swiping) print(swipeUp + "/" + swipeDown + "/" + swipeLeft + "/" + swipeRight);
        }
    }
}
