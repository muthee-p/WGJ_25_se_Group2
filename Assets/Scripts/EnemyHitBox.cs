using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthScript.instance.TakeDamage(damage);
        }
    }
}

