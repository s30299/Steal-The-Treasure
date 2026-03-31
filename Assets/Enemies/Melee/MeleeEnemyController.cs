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
    private Animator animator;

    private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip walkSound;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("no player in the scene");
        }
        sr=GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(idleSound);
        }
        //sr.color = idleColor;
        if (distanceToPlayer <= detectionRange)
        {
            enemyState = EnemyState.Walking;
            audioSource.Stop();
            animator.SetBool("IsWalking", true);
        }
    }
    private void Walking(float distanceToPlayer,float delta)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(walkSound);
        }
        if (distanceToPlayer > detectionRange)
        {
            enemyState=EnemyState.Idle;
            audioSource.Stop();
            animator.SetBool("IsWalking", false);
        }
        else if(distanceToPlayer < attackRange + Random.Range(-attackRangeMargin,attackRangeMargin) && TimerReached(attackCooldown))
        {
            enemyState=EnemyState.AttackStarted;
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("attack");
            StartTimer();
        }
        else if (distanceToPlayer > attackRange)
        {
            MoveTo(player.transform.position, delta);
        }
    }
    private void AttackStarted() 
    {
        //sr.color = attackBeginColor;
        if (TimerReached(attackTelegraphTime))
        {
            enemyState=EnemyState.Attacking;
            audioSource.Stop();
            animator.SetTrigger("endTelegraph");
            StartTimer();
        }
    }
    private void Attacking(float distanceToPlayer)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(attackSound);
        }
        drawAttackRange = true;
        //sr.color=attackColor;
        if (distanceToPlayer <= attackRange)
        {
            player.GetComponent<PlayerController>().OnDeath();
        }
        if (TimerReached(attackTime))
        {
            enemyState = EnemyState.AttackEnded;
            animator.SetTrigger("endAttack");
            StartTimer();
        }
    }
    private void AttackEnded() 
    {
        drawAttackRange = false;
        //sr.color = attackBeginColor;
        if (TimerReached(attackEndTime))
        {
            enemyState = EnemyState.Idle;
            animator.SetTrigger("finishEnd");
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
