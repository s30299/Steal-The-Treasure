using UnityEngine;

public interface IDamageable
{
    int TakeDamage(int amount, Vector2 hitDirection, float knockback, GameObject attacker = null);
}
