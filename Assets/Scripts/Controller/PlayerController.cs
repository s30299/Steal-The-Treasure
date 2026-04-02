using UnityEngine;

public class PlayerController : Controller, IInputRetriever
{
    private double lastOnDeathReceived=0;

    public AudioClip dashSound;
    public AudioClip JumpSound;
    public AudioClip walkSound;

    private UIManager ui;
    public void Start()
    {
        ui = FindAnyObjectByType<UIManager>();
        UnlockInputs();
    }
    public void Update()
    {
        if (InputManager.ActionPause.WasPerformedThisFrame()){
            ui.SwitchPause();
        }
    }
    public override Vector2 RetrieveMoveInput() => InputLocked ? Vector2.zero : InputManager.ActionMove.ReadValue<Vector2>();
    public override bool RetrieveJumpInput() => InputLocked ? false : InputManager.ActionJump.IsPressed();
    public override bool RetrieveInteractInput() => InputLocked ? false : InputManager.ActionInteract.IsPressed();
    public override bool RetrieveAttackInput() => InputLocked ? false : InputManager.ActionAttack.triggered;
    public override bool RetrieveDashInput() => InputLocked ? false : InputManager.ActionDash.triggered;
    public override bool RetrieveLeftMouseInput() => InputLocked ? false : InputManager.LeftMouse.triggered;
    public override bool RetrieveRightMouseInput() => InputLocked ? false : InputManager.RightMouse.triggered;

    public void OnDeath(){
        if (Time.timeAsDouble - lastOnDeathReceived >= 1)
        {
            LockInputs();
            lastOnDeathReceived = Time.timeAsDouble;
            ui.EnableGameOverUI();
            Invoke(nameof(StopTime), 0.5f);
        }
    }
    private void StopTime()
    {
        Time.timeScale = 0;
    }
}