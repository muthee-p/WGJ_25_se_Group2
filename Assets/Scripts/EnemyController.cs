using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float lineOfSight;
    public Vector3 initialPos;
    private Transform player;
    bool caught = false;
    int maxHealth = 30, currentHealth, damage = 10;

    void Start()
    {
        initialPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSight && !caught)
        {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, player.position.x, speed * Time.deltaTime),transform.position.y, transform.position.z);
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            caught = true;
            HealthScript.instance.GameOver("You have been caught!");
        }
        if (collision.gameObject.CompareTag("PlayerWeapon") && CharacterController.instance.attacking == CharacterController.Attacking.Attacking)
        {
            maxHealth -= damage;
            if (maxHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
     }
   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
    }
}
