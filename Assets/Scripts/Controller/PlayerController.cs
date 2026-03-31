using UnityEngine;

public class PlayerController : Controller
{
    private double lastOnDeathReceived=0;
    public override Vector2 RetrieveMoveInput() => InputLocked ? Vector2.zero : InputManager.ActionMove.ReadValue<Vector2>();
    public override bool RetrieveJumpInput() => InputLocked ? false : InputManager.ActionJump.IsPressed();
    public override bool RetrieveInteractInput() => InputLocked ? false : InputManager.ActionInteract.IsPressed();
    public override bool RetrieveAttackInput() => InputLocked ? false : InputManager.ActionAttack.triggered;
    public override bool RetrieveDashInput() => InputLocked ? false : InputManager.ActionDash.triggered;

    public void OnDeath(){
        if (Time.timeAsDouble - lastOnDeathReceived >= 1)
        {
            lastOnDeathReceived = Time.timeAsDouble;
            FindAnyObjectByType<UIManager>().EnableGameOverUI();
            Invoke(nameof(Pause), 0.5f);
        }
    }
    private void Pause()
    {
        Time.timeScale = 0;
    }
}