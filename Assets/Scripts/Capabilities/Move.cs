using UnityEngine;

public class Move : Capability
{
    [field: SerializeField, Range(0f, 100f)] public float MaxSpeed { get; set; } = 3f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;
    private Vector2 _direction, _desiredVelocity, _velocity;
    private float _maxSpeedChange, _acceleration;
    private bool _onGround;
    private Animator _animator;
    private SpriteRenderer sr;
    private float lastDirection=1;
    protected override void Update()
    {
        base.Update();
        if (_animator == null || sr==null)
        {
            _animator = GetComponent<Animator>();
            sr= GetComponent<SpriteRenderer>();
        }
        RegisterMoveInput();

        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(MaxSpeed - Controller.Ground.Friction, 0f);
    }

    private void RegisterMoveInput()
    {
        if (IsLocked)
        {
            _direction.x = 0;
            return;
        }

        if (Time.timeScale == 0)
        {
            _direction.x = 0;
            return;
        }

        _direction.x = Controller.RetrieveMoveInput().x;
        if (lastDirection * _direction.x < 0)
        {
            sr.flipX = _direction.x < 0;
        }
        if (_direction.x != 0) { lastDirection = _direction.x; _animator.SetBool("IsWalking", true); }
        else { _animator.SetBool("IsWalking", false); }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _onGround = Controller.Ground.OnGround;
        _velocity = Controller.Rigidbody2D.linearVelocity;

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        Controller.Rigidbody2D.linearVelocity = _velocity;
    }
}
