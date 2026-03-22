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
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
        if(player == null)
        {
            Debug.LogError("no player in the scene");
        }
        if (delayOnSpawn) { timer = shootProjectileDelay; }
    }
    public void Update()
    {
        if (timer > 0) { timer -= Time.deltaTime; }
        else if (PlayerInRange())
        {
            timer = shootProjectileDelay;
            ShootProjectile();
        }
    }
    private void ShootProjectile()
    {
        var newProjectile = Instantiate(projectile, transform.position, transform.rotation).GetComponent<Projectile>();
        newProjectile.SetDirection(HelperFunctions.Vector3toVector2(player.transform.position) - HelperFunctions.Vector3toVector2(transform.position));
        newProjectile.speed = projectileSpeed;
    }
    private bool PlayerInRange()
    {
        return Vector2.Distance(HelperFunctions.Vector3toVector2(player.transform.position), HelperFunctions.Vector3toVector2(transform.position)) < detectionRange;
    }
}
