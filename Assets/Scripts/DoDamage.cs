using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockback = 5f;
    [SerializeField] private LayerMask enemyLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamage(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamage(collision.collider);
    }

    private void TryDamage(Collider2D col)
    {
        if (!_health.IsInvincible)
            return;

        // sprawdz layer
        if (((1 << col.gameObject.layer) & enemyLayer) == 0)
            return;

        var enemyHealth = col.GetComponent<Health>();
        if (enemyHealth != null)
        {
            Vector2 dir = (col.transform.position - transform.position).normalized;

            enemyHealth.TakeDamage(damage, dir, knockback, gameObject);
        }
    }
}