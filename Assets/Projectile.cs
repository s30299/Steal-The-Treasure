using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private float despawnRange = 50f;
    private float distanceTravelled = 0;
    [SerializeField] private AudioClip impactSound;
    private AudioSource audioSource;
    
    public void SetDirection(Vector2 direction,bool flip=false)
    {
        this.direction = direction.normalized;
        GetComponent<SpriteRenderer>().flipX = flip;
        audioSource=GetComponent<AudioSource>();
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
        
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(impactSound);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().OnDeath();
        }
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            if (impactSound != null)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;
                Invoke(nameof(DestroyProjectile), impactSound.length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
