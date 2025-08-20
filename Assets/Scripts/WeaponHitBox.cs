using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public int damage;
    public CharacterController owner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && owner.attacking == CharacterController.Attacking.Attacking)
        {
            EnemyController enemyHealth = other.GetComponent<EnemyController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                //Vector2 knockDir = (5f * (other.transform.position - transform.position)).normalized;
                enemyHealth.ApplyKnockback(owner.transform.position, 15f, 0.3f);
            }
        }
    }
}

