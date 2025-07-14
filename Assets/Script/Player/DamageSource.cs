using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    public int DamageAmount
    {
        get => damageAmount;
        set => damageAmount = value;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamage(damageAmount);
        if (collision.gameObject.GetComponent<Projectile>())
        {
            collision.gameObject.GetComponent<Projectile>().DeleteProjectile();
        } 
    }
}
