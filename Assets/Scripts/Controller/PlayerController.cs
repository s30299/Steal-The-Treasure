using UnityEngine;

public class PlayerController : Controller
{
    public override Vector2 RetrieveMoveInput() => InputLocked ? Vector2.zero : InputManager.ActionMove.ReadValue<Vector2>();
    public override bool RetrieveJumpInput() => InputLocked ? false : InputManager.ActionJump.IsPressed();
    public override bool RetrieveInteractInput() => InputLocked ? false : InputManager.ActionInteract.IsPressed();
    public override bool RetrieveAttackInput() => InputLocked ? false : InputManager.ActionAttack.triggered;
    public override bool RetrieveDashInput() => InputLocked ? false : InputManager.ActionDash.triggered;
}