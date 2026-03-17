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

    static Vector2 lastPos = Vector2.zero;

    public static bool swiping;
    public static bool swipeUp;
    public static bool swipeDown;
    public static bool swipeLeft;
    public static bool swipeRight;

    private static float swipeSensitivity = 5f;
    private static float swipeRatio;

    public float swipeSpeed = 4.0f;
    public float ratio = 6.0f;

    private void Start()
    {
        swipeSensitivity = swipeSpeed;
        swipeRatio = ratio;
    }

    public void Update()
    {
        if (Touchscreen.current == null) return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Began)
        {
            //print("Began");
            lastPos = touch.position.value;
        }

        if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            var diff = (touch.position.value - lastPos) * Time.deltaTime;
            //print("Diff: " + diff.ToString());
            swipeUp = diff.y > swipeSensitivity && Mathf.Abs(diff.y) > Mathf.Abs(diff.x) * swipeRatio;
            swipeDown = diff.y < -swipeSensitivity && Mathf.Abs(diff.y) > Mathf.Abs(diff.x) * swipeRatio;
            swipeLeft = diff.x < -swipeSensitivity && Mathf.Abs(diff.x) > Mathf.Abs(diff.y) * swipeRatio;
            swipeRight = diff.x > swipeSensitivity && Mathf.Abs(diff.x) > Mathf.Abs(diff.y) * swipeRatio;
            swiping = swipeUp || swipeDown || swipeLeft || swipeRight;

            print("S: " + swiping + "\nU: " + swipeUp + " D: " + swipeDown + " L: " + swipeLeft + " R: " + swipeRight);

            lastPos = touch.position.value;
        }

        if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            swipeUp = false;
            swipeDown = false;
            swipeLeft = false;
            swipeRight = false;
            swiping = false;
        }
    }
}
