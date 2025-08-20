using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    public float walkSpeed = 7, runSpeed = 15;
    public float lineOfSight;
    public float attackZone;
    public float gasAvoidRange;
    public Vector3 initialPos;
    public bool isAttacking = false;
    public int maxHealth = 30;
    public string uniqueId;
    public float attackWindup = 0.5f;
    public float attackRecovery = 0.2f;
    private Transform player, gas;
    float speed;
    Animator anim;
    Coroutine attackCoroutine;
    float attackCooldown = 2;
    float lastAttackTime;
    bool facingRight = false;

    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        
        anim = GetComponent<Animator>();
        initialPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gas = GameObject.FindGameObjectWithTag("Checkpoint").transform;
        speed = 0;

        if (uniqueId == null) uniqueId = gameObject.scene.name + "_" + gameObject.name + "_" + transform.position.ToString();

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
        float distanceFromGas = Vector2.Distance(gas.position, transform.position);

        if (distanceFromGas < gasAvoidRange)
        {
            StopAttack();
            anim.SetFloat("speed", 0.5f);
            Vector3 targetPos = new Vector3(initialPos.x, transform.position.y, transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            return;
        }else if (distanceFromPlayer > lineOfSight) ReturnToInitialPos();

        if (distanceFromPlayer < lineOfSight)
        {
            FacePlayer();
            if (distanceFromPlayer > attackZone)
            {
               ChasePlayer();
            }
            else if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
            {
                StartAttack();
            }
        }
        else
        {
            ReturnToInitialPos();
        }

        if (transform.position == initialPos)
        {
            anim.SetFloat("speed", 0);
            speed = 0;
        }

        //captured player
        if (CharacterCheckAttack.instance.beingAttacked == CharacterCheckAttack.BeingAttacked.Captured) anim.SetFloat("speed", 0);

    }

    void FacePlayer()
    {
        float diff = player.position.x - transform.position.x;

       if (Mathf.Abs(diff) > 0.1f)
        {
            bool faceRight = diff > 0;
            if (faceRight != facingRight)
            {
                facingRight = faceRight;
                transform.rotation = facingRight ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
            }
        }
    }

    void ChasePlayer()
    {
        if(isAttacking) return;

        anim.SetFloat("speed", 0.8f);
        speed = runSpeed;

        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, targetPos , speed * Time.deltaTime);
    }

    void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        speed = runSpeed;
        attackCoroutine = StartCoroutine(AttackRoutine());
    }
    void StopAttack()
    {
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        isAttacking = false;
        anim.SetFloat("speed", 0.5f);
        speed = walkSpeed;
    }

    void ReturnToInitialPos()
    {
        if(isAttacking) return;

        anim.SetFloat("speed", 0.5f);
        speed = walkSpeed;

        Vector3 targetPos = new Vector3(initialPos.x, transform.position.y, transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        maxHealth -= damage;
        healthBar.value = maxHealth;
        if (maxHealth <= 0)
        {
            GameController.instance.killedEnemies.Add(uniqueId);
            Destroy(gameObject);
        }
    }
    private IEnumerator AttackRoutine()
    {
        anim.SetFloat("speed", 0);
        yield return new WaitForSeconds(attackWindup);
        anim.SetBool("Attacking", true);

        float lungeSpeed = speed * 2f;
        //float lungeTime = 0.2f;
        float elapsedTime = 0;

        Vector3 attackPos = new Vector3(player.position.x, transform.position.y, transform.position.z);

        while (Vector3.Distance(transform.position, attackPos) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, attackPos, lungeSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        // while (elapsedTime < lungeTime)
        // {
        //     float progress = elapsedTime / lungeTime;
        //     transform.position = Vector2.Lerp(startPos, attackPos, progress); 
        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }

        yield return new WaitForSeconds(attackRecovery);
        isAttacking = false;
        anim.SetBool("Attacking", false);
        attackCoroutine = null;
    }

    public void ApplyKnockback(Vector2 sourcePos, float force, float duration)
    {
        StartCoroutine(KnockbackRoutine(sourcePos, force, duration));
    }

    private IEnumerator KnockbackRoutine(Vector2 sourcePos, float force, float duration)
    {
        // direction only on X axis
        float dir = (transform.position.x - sourcePos.x) > 0 ? 1f : -1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position += new Vector3(dir * force * Time.deltaTime, 0f, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackZone);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gasAvoidRange);
    } 

}
