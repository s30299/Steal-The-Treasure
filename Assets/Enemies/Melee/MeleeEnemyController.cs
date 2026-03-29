using UnityEngine;
public enum EnemyState
{
    Idle,Walking,AttackStarted,Attacking,AttackEnded
}
public class MeleeEnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject player;
    private EnemyState enemyState=EnemyState.Idle;
    [SerializeField] private float detectionRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRangeMargin;
    [SerializeField] private float attackTelegraphTime;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackEndTime;
    [SerializeField] private float attackCooldown;

    private Color attackBeginColor = Color.orange;
    private Color attackColor = Color.red;
    private Color idleColor = Color.white;
    private SpriteRenderer sr;
    private float timeCounter = 0;
    private bool drawAttackRange = false;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("no player in the scene");
        }
        sr=GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        StateBehavior(Time.deltaTime);
        UpdateTimer(Time.deltaTime);
    }
    private void StateBehavior(float delta)
    {
        var distanceToPlayer = GetDistanceToPlayer();
        //Debug.Log(enemyState.ToString());
        switch (enemyState)
        {
            case EnemyState.Idle: {Idle(distanceToPlayer); break; }
            case EnemyState.Walking: {Walking(distanceToPlayer,delta); RotateSpriteTowardsPlayer(player.transform.position); break; }
            case EnemyState.AttackStarted: {AttackStarted(); RotateSpriteTowardsPlayer(player.transform.position); break; }
            case EnemyState.AttackEnded: {AttackEnded(); break; } 
            case EnemyState.Attacking: {Attacking(distanceToPlayer); break; }  
        }
    }
    private void Idle(float distanceToPlayer)
    {
        sr.color = idleColor;
        if (distanceToPlayer <= detectionRange)
        {
            enemyState = EnemyState.Walking;
        }
    }
    private void Walking(float distanceToPlayer,float delta)
    {
        MoveTo(player.transform.position, delta);
        if(distanceToPlayer > detectionRange)
        {
            enemyState=EnemyState.Idle;
        }
        else if(distanceToPlayer < attackRange + attackRangeMargin && TimerReached(attackCooldown))
        {
            enemyState=EnemyState.AttackStarted;
            StartTimer();
        }
    }
    private void AttackStarted() 
    {
        sr.color = attackBeginColor;
        if (TimerReached(attackTelegraphTime))
        {
            enemyState=EnemyState.Attacking;
            StartTimer();
        }
    }
    private void Attacking(float distanceToPlayer)
    {
        drawAttackRange = true;
        sr.color=attackColor;
        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Player Hit");
        }
        if (TimerReached(attackTime))
        {
            enemyState = EnemyState.AttackEnded;
            StartTimer();
        }
    }
    private void AttackEnded() 
    {
        drawAttackRange = false;
        sr.color = attackBeginColor;
        if (TimerReached(attackEndTime))
        {
            enemyState = EnemyState.Idle;
            StartTimer();
        }
    }
    private float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.transform.position);
    }
    private bool TimerReached(float time)
    {
        return timeCounter >= time;
    }
    private void StartTimer()
    {
        timeCounter = 0;
    }
    private void UpdateTimer(float delta)
    {
        timeCounter += delta;
    }
    private void MoveTo(Vector2 position, float delta)
    {
        transform.Translate( delta * speed * (position-HelperFunctions.Vector3toVector2(transform.position)).normalized);
    }
    private void RotateSpriteTowardsPlayer(Vector2 position)
    {
        if (transform.position.x >= position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
    void OnDrawGizmosSelected()
    {
        if (drawAttackRange)
        {
            Gizmos.color = new(255, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
