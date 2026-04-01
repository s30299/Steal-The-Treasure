using System;
using UnityEngine;

public class Jump : Capability
{
    [SerializeField] private JumpSkillDefinition _definition;
    [SerializeField] private PlayerProgress _progress;
    [SerializeField, Range(0f, 1f)] private float _verticalVelocityEpsilon = 0.05f;

    private SkillState _skillState;

    private int level => _skillState.currentLevel;
    private float _jumpHeight => _definition.GetJumpHeight(level);
    private int _maxAirJumps => _definition.GetMaxAirJumps(level);
    private float _downwardMovementMultiplier => _definition.GetDownwardMovementMultiplier(level);
    private float _upwardMovementMultiplier => _definition.GetUpwardMovementMultiplier(level);
    private float _coyoteTime => _definition.GetCoyoteTime(level);
    private float _jumpBufferTime => _definition.GetJumpBufferTime(level);

    private Vector2 _velocity;

    private int _jumpPhase;
    private float _defaultGravityScale;
    private float _jumpSpeed;
    private float _coyoteCounter;
    private float _jumpBufferCounter;

    private bool _desiredJump;
    private bool _isJumpReset;
    private bool _forceJumpNow;

    public Action OnJumped;

    private Animator _animator;
    protected override void Awake()
    {
        base.Awake();
        _animator=GetComponent<Animator>();

        _isJumpReset = true;
        _defaultGravityScale = Controller.Rigidbody2D.gravityScale;

        _skillState = _progress.skills.Find(skill => skill.skillDefinition == _definition);
        if (_skillState == null)
        {
            _skillState = new SkillState
            {
                skillDefinition = _definition,
                currentLevel = 1
            };
            _progress.skills.Add(_skillState);
        }
    }

    protected override void Update()
    {
        base.Update();

        RegisterInput();
        HandleJumpReset();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsLocked)
            return;

        _velocity = Controller.Rigidbody2D.linearVelocity;

        bool touchingGround = Controller.Ground.OnGround;
        bool touchingLadder = Controller.Ground.OnLadder;
        bool touchingWall = Controller.Ground.OnWall;

        float vy = _velocity.y;
        bool vyIsZero = Mathf.Abs(vy) < _verticalVelocityEpsilon;

        if ((touchingGround && vyIsZero) || touchingLadder || (touchingWall && vy < 0f))
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

    public void Grounded(float coyoteTimeMultiplier = 1f)
    {
        _jumpPhase = 0;
        _coyoteCounter = _coyoteTime * coyoteTimeMultiplier;
    }

    private void SetGravity()
    {
        if (IsLocked)
            return;

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

        if (groundedJump)
        {
            _jumpPhase = 0;
            _animator.SetTrigger("StartJump");
        }
        else if (!_forceJumpNow)
        {
            _jumpPhase++;
            _animator.SetTrigger("StartJump");
        }

        _jumpBufferCounter = 0f;
        _coyoteCounter = 0f;
        _forceJumpNow = false;

        _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * Controller.Rigidbody2D.gravityScale * _jumpHeight);

        if (_velocity.y > _verticalVelocityEpsilon)
        {
            _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
        }
        else if (_velocity.y < -_verticalVelocityEpsilon)
        {
            _animator.SetTrigger("JumpTopReached");
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
        if (_jumpPhase > 0)
            _jumpPhase--;
    }
}