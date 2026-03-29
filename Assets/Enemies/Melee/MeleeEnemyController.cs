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
    private float timeCounter = 0;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("no player in the scene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateBehavior();
        UpdateTimer(Time.deltaTime);
    }
    private void StateBehavior()
    {
        var distanceToPlayer = GetDistanceToPlayer();
        //Debug.Log(enemyState.ToString());
        switch (enemyState)
        {
            case EnemyState.Idle: {Idle(distanceToPlayer); break; }
            case EnemyState.Walking: {Walking(distanceToPlayer); break; }
            case EnemyState.AttackStarted: {AttackStarted(distanceToPlayer); break; }
            case EnemyState.AttackEnded: {AttackEnded(distanceToPlayer); break; } 
            case EnemyState.Attacking: {Attacking(distanceToPlayer); break; }  
        }
    }
    private void Idle(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectionRange)
        {
            enemyState = EnemyState.Walking;
        }
    }
    private void Walking(float distanceToPlayer)
    {
        
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
    private void AttackStarted(float distanceToPlayer) 
    {
        if (TimerReached(attackTelegraphTime))
        {
            enemyState=EnemyState.Attacking;
            StartTimer();
        }
    }
    private void Attacking(float distanceToPlayer)
    {
        if (TimerReached(attackTime))
        {
            enemyState = EnemyState.AttackEnded;
            StartTimer();
        }
    }
    private void AttackEnded(float distanceToPlayer) 
    {
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
}
