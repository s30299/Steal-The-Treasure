using UnityEngine;
public class RangedEnemyController : MonoBehaviour
{
    private GameObject player;
    public float shootProjectileDelay=5f;
    private float timer=0;
    public float projectileSpeed = 10f;
    public float detectionRange = 15f;
    public GameObject projectile;
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
        if(player == null)
        {
            Debug.LogError("no player in the scene");
        }
    }
    public void Update()
    {
        if (timer > 0) { timer -= Time.deltaTime; }
        else if (PlayerInRange())
        {
            timer = shootProjectileDelay;
            var newProjectile =Instantiate(projectile,transform.position,transform.rotation);
            newProjectile.GetComponent<Projectile>().SetDirection(HelperFunctions.Vector3toVector2(player.transform.position) - HelperFunctions.Vector3toVector2(transform.position));
        }
    }
    private bool PlayerInRange()
    {
        return Vector2.Distance(HelperFunctions.Vector3toVector2(player.transform.position), HelperFunctions.Vector3toVector2(transform.position)) < detectionRange;
    }
}
