using UnityEngine;

[RequireComponent(typeof(Controller))]
public abstract class Capability : MonoBehaviour
{
    protected Controller _controller;
    public Controller Controller { get { return _controller; } }
    public bool IsLocked { get;  set; }

    protected virtual void Awake() => _controller = GetComponent<Controller>();
    protected virtual void Start() { }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
    protected virtual void Update() { }
    protected virtual void LateUpdate() { }
    protected virtual void FixedUpdate() { }

    public virtual bool CanRegisterInput()
    {
        if (IsLocked)
            return false;

        if (Controller == null)
            return false;
        
        if (Controller.InputLocked)
            return false;

        if (Time.timeScale == 0f)
            return false;

        return true;
    }
}