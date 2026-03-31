using System;
using UnityEngine;

public class Jump : Capability
{
    [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
    [SerializeField, Range(0, 3)] private int _maxAirJumps = 1; // 1 = double jump
    [SerializeField, Range(0f, 8f)] private float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 8f)] private float _upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 0.3f)] private float _coyoteTime = 0.2f;
    [SerializeField, Range(0f, 0.3f)] private float _jumpBufferTime = 0.2f;
    [SerializeField, Range(0f, 1f)] private float _verticalVelocityEpsilon = 0.05f;

    public int MaxAirJumps { get => _maxAirJumps; set => _maxAirJumps = value; }

    private Vector2 _velocity;

    // ile air jumpów już zużyto
    private int _jumpPhase;

    private float _defaultGravityScale;
    private float _jumpSpeed;
    private float _coyoteCounter;
    private float _jumpBufferCounter;

    private bool _desiredJump;
    private bool _isJumping;
    private bool _isJumpReset;
    private bool _landed;
    private bool _forceJumpNow;

    public Action OnJumped, OnLanded;

    protected override void Awake()
    {
        base.Awake();

        _isJumpReset = true;
        _defaultGravityScale = Controller.Rigidbody2D.gravityScale;
    }

    protected override void Update()
    {
        base.Update();

        RegisterInput();
        HandleJumpReset();
    }

    private void RegisterInput()
    {
        if (IsLocked)
            return;

        _desiredJump = Controller.RetrieveJumpInput();
    }

    private void HandleJumpReset()
    {
        if (_desiredJump && _isJumpReset)
        {
            _isJumpReset = false;
            _desiredJump = false;
            _jumpBufferCounter = _jumpBufferTime;
        }
        else if (_jumpBufferCounter > 0f)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        else if (!Controller.RetrieveJumpInput())
        {
            _isJumpReset = true;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsLocked)
            return;

        _velocity = Controller.Rigidbody2D.linearVelocity;

        bool touchingGround = Controller.Ground.OnGround;
        bool touchingLadder = Controller.Ground.OnLadder;

        float vy = _velocity.y;
        bool vyIsZero = Mathf.Abs(vy) < _verticalVelocityEpsilon;

        // Reset skoków na ziemi
        if (touchingGround && vyIsZero)
        {
            Grounded();
        }
        // Reset skoków na drabinie - zawsze, niezależnie od strony kontaktu
        else if (touchingLadder)
        {
            Grounded();
        }
        else if (Controller.Ground.OnWall && vy < 0f)
        {
            Grounded();
        }
        else
        {
            _coyoteCounter -= Time.fixedDeltaTime;
        }

        if (_jumpBufferCounter > 0f || _forceJumpNow)
            JumpAction();

        SetGravity();

        Controller.Rigidbody2D.linearVelocity = _velocity;
    }

    public void Grounded(float coyoteTimeMultiplier = 1f)
    {
        _jumpPhase = 0;
        _coyoteCounter = _coyoteTime * coyoteTimeMultiplier;
        _isJumping = false;
    }

    private void SetGravity()
    {
        if (IsLocked)
            return;

        // Na drabinie zwykle nie chcesz ciężkiej grawitacji od skoku
        if (Controller.Ground.OnLadder)
        {
            Controller.Rigidbody2D.gravityScale = _defaultGravityScale;
            return;
        }

        float vy = Controller.Rigidbody2D.linearVelocity.y;
        bool vyPositive = vy > _verticalVelocityEpsilon;
        bool vyNegative = vy < -_verticalVelocityEpsilon;

        if (Controller.RetrieveJumpInput() && vyPositive)
        {
            Controller.Rigidbody2D.gravityScale = _upwardMovementMultiplier;
            return;
        }

        if (!Controller.RetrieveJumpInput() || vyNegative)
        {
            Controller.Rigidbody2D.gravityScale = _downwardMovementMultiplier;
            return;
        }

        Controller.Rigidbody2D.gravityScale = _defaultGravityScale;
    }

    private void JumpAction()
    {
        if (IsLocked)
            return;

        bool groundedJump = Controller.Ground.OnGround || Controller.Ground.OnLadder || _coyoteCounter > 0f;
        bool airJump = !groundedJump && _jumpPhase < _maxAirJumps;

        if (!_forceJumpNow && !groundedJump && !airJump)
            return;

        OnJumped?.Invoke();

        // skok z ziemi/drabiny/coyote nie zużywa air jumpa
        if (groundedJump)
        {
            _jumpPhase = 0;
        }
        else if (!_forceJumpNow)
        {
            _jumpPhase++;
        }

        _jumpBufferCounter = 0f;
        _coyoteCounter = 0f;
        _isJumping = true;
        _landed = false;
        _forceJumpNow = false;

        _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * Controller.Rigidbody2D.gravityScale * _jumpHeight);

        if (_velocity.y > _verticalVelocityEpsilon)
        {
            _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
        }
        else if (_velocity.y < -_verticalVelocityEpsilon)
        {
            _jumpSpeed += Mathf.Abs(_velocity.y);
        }

        _velocity.y += _jumpSpeed;
    }

    public void RequestJump()
    {
        _desiredJump = true;
        _forceJumpNow = true;
    }

    public void Bounced()
    {
        _isJumping = true;

        // odbicie nie powinno zabierać dodatkowego skoku
        if (_jumpPhase > 0)
            _jumpPhase--;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.contactCount > 0 && other.contacts[0].normal == new Vector2(0, -1))
            return;

        if (_landed)
            return;

        OnLanded?.Invoke();
        _landed = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!Controller.Ground.OnWall && !Controller.Ground.OnGround && !Controller.Ground.OnLadder)
            _landed = false;
    }
}