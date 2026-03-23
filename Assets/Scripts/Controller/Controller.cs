using UnityEngine;
using System.Collections;

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
}