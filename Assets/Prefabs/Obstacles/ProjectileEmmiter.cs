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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == ProjectileEmmiterType.PlayerActivated && collision.gameObject.CompareTag("Player"))
        {
            ShootProjectile();
        }
    }
}
