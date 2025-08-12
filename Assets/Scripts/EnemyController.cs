using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public AudioClip enemyInSight;
    public float speed = 7;
    public float lineOfSight;
    public float attackZone;
    public Vector3 initialPos;
    public bool isAttacking = false;
    public int maxHealth = 30;
    public string uniqueId;
    private Transform player;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        initialPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (uniqueId == null)  uniqueId = gameObject.scene.name + "_" + gameObject.name + "_" + transform.position.ToString();

        if (GameController.instance.killedEnemies.Contains(uniqueId))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterController.instance.movement == CharacterController.Movement.Fainted || CharacterController.instance.gameState == CharacterController.GameState.Paused) return;

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSight && distanceFromPlayer > attackZone)
        {
            anim.SetFloat("speed", 0.5f);
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, player.position.x, speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (distanceFromPlayer < attackZone && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackRoutine());
        }
        else
        {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, initialPos.x, speed * Time.deltaTime), transform.position.y, transform.position.z);
            isAttacking = false;
        }

        //face player
        if (player.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //back to idle
        if (transform.position == initialPos)
        {
            anim.SetFloat("speed", 0);
        }

        //captured player
        if(CharacterCheckAttack.instance.beingAttacked == CharacterCheckAttack.BeingAttacked.Captured) anim.SetFloat("speed", 0);

    }

    public void TakeDamage(int damage)
    {
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            GameController.instance.killedEnemies.Add(uniqueId);
            Destroy(gameObject);
        }
    }
    private IEnumerator AttackRoutine()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackZone);
    } 
}
