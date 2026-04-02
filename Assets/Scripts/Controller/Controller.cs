using UnityEngine;
using System.Collections;

public enum MovementDirection
{
    Left, Right, Standing
}
public abstract class Controller : MonoBehaviour, IInputRetriever
{
    [field: Header("Base Controller")]
    [field: SerializeField] public Transform Anchor { get; private set; }
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
    [field: SerializeField] public GroundDetector Ground { get; private set; }
    public Vector2 Facing { get; set; } = Vector2.right;
    public Vector2 LastHorizontalFacing { get; set; } = Vector2.right;
    private int _inputLockCount;
    public bool InputLocked => _inputLockCount > 0;
    private static MovementDirection movementDirection=MovementDirection.Standing;
    private static bool isDashing;
    
    public void LockInputsFor(float seconds)
    {
        if (seconds <= 0f)
            return;

        _inputLockCount++;
        StartCoroutine(InputLockCoroutine(seconds));
    }

    public void LockInputs()
    {
        _inputLockCount++;
    }

    public void UnlockInputs()
    {
        if (_inputLockCount > 0)
            _inputLockCount--;
    }

    private IEnumerator InputLockCoroutine(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        if (_inputLockCount > 0)
            _inputLockCount--;
    }

    public abstract Vector2 RetrieveMoveInput();
    public abstract bool RetrieveJumpInput();
    public abstract bool RetrieveInteractInput();
    public abstract bool RetrieveAttackInput();
    public abstract bool RetrieveDashInput();
    public abstract bool RetrieveLeftMouseInput();
    public abstract bool RetrieveRightMouseInput();

    public static void MovingLeft(){movementDirection = MovementDirection.Left;}
    public static void MovingRight() {movementDirection = MovementDirection.Right;}
    public static void StandingStill(){movementDirection=MovementDirection.Standing;}

    public static MovementDirection GetMoveDirection(){return movementDirection;}

}