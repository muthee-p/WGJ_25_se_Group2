using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [SerializeField] private GameObject hit;
    public int damage;
    public CharacterController owner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && owner.attacking == CharacterController.Attacking.Attacking)
        {
            EnemyController enemyHealth = other.GetComponent<EnemyController>();
            if (enemyHealth != null)
            {
                enemyHealth.isPushedBack = true;
                enemyHealth.TakeDamage(damage);
                GameObject spawnedHit = Instantiate(hit, transform.position, Quaternion.identity);
                //Vector2 knockDir = (5f * (other.transform.position - transform.position)).normalized;
                enemyHealth.ApplyKnockback(owner.transform.position, 15f, 0.4f);
                Destroy(spawnedHit, 0.15f);
            }
        }
    }
}

