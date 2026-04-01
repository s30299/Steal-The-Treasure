using System;
using System.Collections;
using UnityEngine;

public class Dash : Capability
{

    [SerializeField] 
    private DashSkillDefinition _definition;
    [SerializeField] 
    private PlayerProgress _progress;
    private SkillState _skillState;

    private int level => _skillState.currentLevel;
    private float _dashSpeed => _definition.GetDashSpeed(level);
    private float _dashDuration => _definition.GetDashDuration(level);
    private float _IFrames => _definition.GetIFrames(level);
    private int _maxDashes => _definition.GetMaxDashes(level);
    private float _dashRechargeTime => _definition.GetDashRechargeTime(level);
    private  Controller controller => Controller;
    private float _inputDeadzone = 0.1f;

    private int _currentDashes;
    private bool _isDashing;
    private Vector2 _dashDirection;

    private Coroutine _dashRoutine;
    private Coroutine _rechargeRoutine;
    private Coroutine _IFramesRoutine;
    private Health _health;

    private Animator _animator;

    public int CurrentDashes => _currentDashes;
    public int MaxDashes => _maxDashes;

    public Action OnDash, OffDash;

    protected override void Awake()
    {
        base.Awake();

        if (_health == null)
            _health = GetComponent<Health>();
            
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
        _currentDashes = _maxDashes;
        _animator = GetComponent<Animator>();
    }
    protected override void Update()
    {
        base.Update();

        Vector2 move = controller.RetrieveMoveInput();
        if (Mathf.Abs(move.x) > _inputDeadzone)
            controller.LastHorizontalFacing = Vector2.right * Mathf.Sign(move.x);

        if (!CanRegisterInput()) return;

        if (_isDashing)
            return;

        if (_currentDashes > 0 && Controller.RetrieveDashInput())
        {
            // Debug.Log("DASH");
            _dashRoutine ??= StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        _isDashing = true;
        _currentDashes--;
        // Debug.Log($" DashRoutine start| MaxDashCount: {_maxDashes} CurrentDashCount: {_currentDashes}");

        _rechargeRoutine ??= StartCoroutine(RechargeDash());

        _dashDirection = Controller.LastHorizontalFacing;
        _animator.SetTrigger("Dashed");

        float elapsed = 0f;
        OnDash?.Invoke();
        if (_IFramesRoutine != null)
        {
            StopCoroutine(_IFramesRoutine);
        }
        _IFramesRoutine = StartCoroutine(IFrames(_IFrames));
        while (elapsed < _dashDuration)
        {
            Vector2 velocity = Controller.Rigidbody2D.linearVelocity;

            velocity.x = _dashDirection.x * _dashSpeed;

            Controller.Rigidbody2D.linearVelocity = velocity;

            elapsed += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        _isDashing = false;
        _dashRoutine = null;
        OffDash?.Invoke();
    }

    private IEnumerator RechargeDash()
    {
        while (_currentDashes < _maxDashes)
        {
            yield return new WaitForSeconds(_dashRechargeTime);
            _currentDashes++;
            // Debug.Log($" RechargeDash | CurrentDashCount: {_currentDashes}");

        }
        _rechargeRoutine = null;
    }
    private IEnumerator IFrames(float duration)
    {   
        // Debug.Log("Invincibility Frames Start");
        _health.IsInvincible = true;
        yield return new WaitForSeconds(duration);
        // Debug.Log("Invincibility Frames End");
        _health.IsInvincible = false;
        _IFramesRoutine = null;
    }
}