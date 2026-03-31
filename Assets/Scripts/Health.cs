using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;
    private int current;
    private Rigidbody2D rb;
    private PlayerController playerController;

    public bool IsInvincible { get; set; } = false;
    public int CurrentHealth => current;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        current = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    public int TakeDamage(int amount, Vector2 hitDirection, float knockback, GameObject attacker = null)
    {
        if(IsInvincible)
            return current;
        current -= amount;
        if (rb != null && knockback > 0f)
            rb.AddForce(hitDirection.normalized * knockback, ForceMode2D.Impulse);
        if (current <= 0)
        {
            Die(attacker);
            return 0;
        }
        return current;
    }

    protected virtual void Die(GameObject attacker = null)
    {
        playerController.OnDeath();
    }
}