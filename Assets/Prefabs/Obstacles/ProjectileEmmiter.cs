using UnityEditor;
using UnityEngine;
public enum ProjectileEmmiterType
{
    Repeating,PlayerActivated
}
public enum Direction
{
    Up,Down,Left,Right
}
public class ProjectileEmmiter : MonoBehaviour
{
    public ProjectileEmmiterType type;
    public Direction direction;
    public Vector2 projectileDirection;
    public GameObject projectile;
    public float projectileSpeed=15f;
    public float shootDelay;
    public float projectileRange = 50f;
    void Start()
    {
        switch (direction)
        {
            case Direction.Up: { projectileDirection = new(0, 1); break; }
            case Direction.Down: { projectileDirection = new(0, -1); break; }
            case Direction.Left: { projectileDirection = new(-1, 0); break; }
            case Direction.Right: { projectileDirection = new(1, 0); break; }
        }
        if (type == ProjectileEmmiterType.Repeating)
        {
            InvokeRepeating(nameof(ShootProjectile),0,shootDelay);
        }
        GetComponent<SpriteRenderer>().enabled = false;
    }
    private void ShootProjectile()
    {
        var newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation).GetComponent<Projectile>();
        newProjectile.SetDirection(projectileDirection);
        newProjectile.speed = projectileSpeed;
        newProjectile.despawnRange = projectileRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == ProjectileEmmiterType.PlayerActivated && collision.gameObject.CompareTag("Player"))
        {
            ShootProjectile();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        switch (direction)
        {
            case Direction.Left: { Gizmos.DrawLine(transform.position, transform.position + (Vector3.left * projectileRange)); break; }
            case Direction.Right: { Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * projectileRange)); break; }
            case Direction.Down: { Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * projectileRange)); break; }
            case Direction.Up: { Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * projectileRange)); break; }
        }
    }
}
