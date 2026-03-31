using UnityEngine;
public class RangedEnemyController : MonoBehaviour
{
    private GameObject player;
    public float shootProjectileDelay=5f;
    private float timer=0;
    public float projectileSpeed = 10f;
    public float detectionRange = 15f;
    public bool delayOnSpawn = true;
    public GameObject projectile;
    private Animator animator;
    private SpriteRenderer sr;
    [SerializeField] private AnimationClip attack;
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
        if(player == null)
        {
            Debug.LogError("no player in the scene");
        }
        if (delayOnSpawn) { timer = shootProjectileDelay; }
        animator=GetComponent<Animator>();
        sr=GetComponent<SpriteRenderer>();
    }
    public void Update()
    {
        RotateSpriteTowardsPlayer(player.transform.position);
        if (timer > 0) { timer -= Time.deltaTime; }
        else if (PlayerInRange())
        {
            animator.SetTrigger("attack");
            timer = shootProjectileDelay+ attack.length;
            Invoke(nameof(ShootProjectile), attack.length);
        }
    }
    private void ShootProjectile()
    {  
        var newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation).GetComponent<Projectile>();
        newProjectile.SetDirection(HelperFunctions.Vector3toVector2(player.transform.position) - HelperFunctions.Vector3toVector2(transform.position),sr.flipX);
        newProjectile.speed = projectileSpeed;
    }
    private bool PlayerInRange()
    {
        return Vector2.Distance(HelperFunctions.Vector3toVector2(player.transform.position), HelperFunctions.Vector3toVector2(transform.position)) < detectionRange;
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
}
