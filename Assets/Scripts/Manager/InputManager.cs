using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-99)]
public class InputManager : MonoSingleton<InputManager>
{
    public static InputSystem_Actions InputActions;

    protected override void Awake()
       => EnableInput();

    private static void EnableInput()
    {
        InputActions = new();
        InputActions.Enable();
    }
    public static void DisableInput()
    {
        InputActions.Disable();
    }
    public static InputAction ActionMove => InputActions.Player.Move;
    public static InputAction ActionJump => InputActions.Player.Jump;
    public static InputAction ActionInteract => InputActions.Player.Interact;
    public static InputAction ActionAttack => InputActions.Player.Attack;
    public static InputAction ActionDash => InputActions.Player.Dash;
    public static InputAction ActionPause => InputActions.UI.Pause;
    public static InputAction LeftMouse => InputActions.Player.ClickLeft;
    public static InputAction RightMouse => InputActions.Player.ClickRight;

    public static string GetInteractButton()
    {
        return "E";
    }

    public static bool UsingController()
    {
        return true;
    }
    public static bool UsingKeyboard()
    {
        return true;
    }

    public static void CaptureCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
