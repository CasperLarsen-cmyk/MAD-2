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
    
    //bool swiping;
    public static bool IsSwipingUp()
    {
        if (Touchscreen.current == null) return false;
        
        var touch = Touchscreen.current.primaryTouch;

        if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Began)
        {
            //print("Began");
            lPos = touch.position.value;
            return false;
        }
        
        if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            var diff = (touch.position.value - lPos) * Time.deltaTime;
            //print("Diff: " + diff.ToString());
            if (diff.y > 5)
            {
                return true;
            }
            lPos = touch.position.value;
            return false;
        }

        /*if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            print("Ended");
        }*/
        return false;
    }
}
