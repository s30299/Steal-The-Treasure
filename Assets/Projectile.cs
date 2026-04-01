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
        //GetComponent<SpriteRenderer>().flipX = flip;
        transform.Rotate(new(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        audioSource=GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.volume = PlayerPrefs.GetFloat("effectsVolume", 1);
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(speed * Time.fixedDeltaTime * Vector2.right);
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
            GameObject.FindWithTag("Player").GetComponent<Health>().TakeDamage(1,direction,1,gameObject);
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
