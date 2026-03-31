using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-99)]
public class InputManager : MonoSingleton<InputManager>
{
    public static InputSystem_Actions InputActions;

    private void Awake()
       => EnableInput();

    private void EnableInput()
    {
        InputActions = new();
        InputActions.Enable();
    }
    public void DisableInput()
    {
        InputActions.Disable();
    }
    public static InputAction ActionMove => InputActions.Player.Move;
    public static InputAction ActionJump => InputActions.Player.Jump;
    public static InputAction ActionInteract => InputActions.Player.Interact;
    public static InputAction ActionAttack => InputActions.Player.Attack;
    public static InputAction ActionDash => InputActions.Player.Dash;
}
