using System;
using UnityEngine;

public class Jump : Capability
{
    [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
    [SerializeField, Range(0, 3)] private int _maxAirJumps = 0;
    [SerializeField, Range(0f, 8f)] private float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 8f)] private float _upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 0.3f)] private float _coyoteTime = 0.2f;
    [SerializeField, Range(0f, 0.3f)] private float _jumpBufferTime = 0.2f;
    [SerializeField, Range(0f, 1f)] private float _verticalVelocityEpsilon = 0.05f;

    public int MaxAirJumps { get => _maxAirJumps; set => _maxAirJumps = value; }

    private Vector2 _velocity;

    private int _jumpPhase;
    private float _defaultGravityScale, _jumpSpeed, _coyoteCounter, _jumpBufferCounter;

    private bool _desiredJump, _isJumping, _isJumpReset, _landed, _forceJumpNow, _wasOnLadder;

    public Action OnJumped, OnLanded;

    protected override void Awake()
    {
        base.Awake();

        _isJumpReset = true;
        _defaultGravityScale = 1f;
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
        else if (_jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        else if (!_desiredJump)
        {
            _isJumpReset = true;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsLocked)
            return;


        if (_wasOnLadder)
        {
            _wasOnLadder = false;
            return;
        }

        _velocity = Controller.Rigidbody2D.linearVelocity;

        float vy = Controller.Rigidbody2D.linearVelocity.y;
        bool vyIsZero = Mathf.Abs(vy) < _verticalVelocityEpsilon;

        if ((Controller.Ground.OnGround && vyIsZero) || (Controller.Ground.OnWall && vy < 0f))
            Grounded();
        else
            _coyoteCounter -= Time.fixedDeltaTime;


        if (_jumpBufferCounter > 0 || _forceJumpNow)
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

        if (Mathf.Abs(Controller.Rigidbody2D.linearVelocity.y) < _verticalVelocityEpsilon)
        {
            Controller.Rigidbody2D.gravityScale = _defaultGravityScale;
            return;
        }
    }

    private void JumpAction()
    {
        if (IsLocked)
            return;

        bool canStartDoubleJump = _coyoteCounter <= 0f && !_isJumping;
        bool canContinueJumping = _jumpPhase < _maxAirJumps && _isJumping;

        if (_coyoteCounter <= 0f && !_forceJumpNow && !canContinueJumping && !canStartDoubleJump)
            return;

        OnJumped?.Invoke();

        if ((_isJumping || canStartDoubleJump) && !_forceJumpNow)
            _jumpPhase += 1;


        _jumpBufferCounter = 0;
        _coyoteCounter = 0;
        _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight * _upwardMovementMultiplier);
        _isJumping = true;
        _landed = false;
        _forceJumpNow = false;

        if (_velocity.y > _verticalVelocityEpsilon)
        {
            _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
        }
        else if (_velocity.y < -_verticalVelocityEpsilon)
        {
            _jumpSpeed += Mathf.Abs(Controller.Rigidbody2D.linearVelocity.y);
        }

        _velocity.y += _jumpSpeed;
    }

    public void RequestJump()
    {
        Grounded();

        _desiredJump = true;
        _forceJumpNow = true;
    }

    public void Bounced()
    {
        _isJumping = true;
        if (_jumpPhase == 1)
            _jumpPhase = 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.contacts[0].normal == new Vector2(0, -1))
            return;

        if (_landed)
            return;

        OnLanded?.Invoke();

        _landed = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!Controller.Ground.OnWall && !Controller.Ground.OnGround)
            _landed = false;
    }
}
