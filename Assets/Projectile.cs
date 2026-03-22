using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private float despawnRange = 50f;
    private float distanceTravelled = 0;
    
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }
    private void FixedUpdate()
    {
        transform.Translate(speed * Time.fixedDeltaTime * direction);
        distanceTravelled += speed * Time.fixedDeltaTime;
        if(distanceTravelled > despawnRange)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("player hit");
        }
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
