using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    public float speed = 7;
    public float lineOfSight;
    public float attackZone;
    public float gasAvoidRange;
    public Vector3 initialPos;
    public bool isAttacking = false;
    public int maxHealth = 30;
    public string uniqueId;
    private Transform player, gas;
    Animator anim;
    Coroutine attackCoroutine;

    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        
        anim = GetComponent<Animator>();
        initialPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gas = GameObject.FindGameObjectWithTag("Checkpoint").transform;

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
        float distanceFromGas = Vector2.Distance(gas.position, transform.position);

        if (distanceFromGas < gasAvoidRange) // <-- set gasAvoidRange in inspector
        {
            StopCoroutine(attackCoroutine);
            anim.SetFloat("speed", 0.5f);
            transform.position = new Vector3(
                Mathf.MoveTowards(transform.position.x, initialPos.x, speed * Time.deltaTime),
                transform.position.y,
                transform.position.z
            );
            isAttacking = false;
            return;
        }

        if (distanceFromPlayer < lineOfSight && distanceFromPlayer > attackZone)
        {
            anim.SetFloat("speed", 0.8f);
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, player.position.x, speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (distanceFromPlayer < attackZone && !isAttacking)
        {
            isAttacking = true;
            attackCoroutine = StartCoroutine(AttackRoutine());
        }
        else
        {
            anim.SetFloat("speed", 0.5f);
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
        healthBar.value = maxHealth;
        if (maxHealth <= 0)
        {
            GameController.instance.killedEnemies.Add(uniqueId);
            Destroy(gameObject);
        }
    }
    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(.8f);

        Vector2 targetPos = player.position;
        float lungeSpeed = speed * 0.4f;
        float lungeTime = 0.2f;
        float elapsedTime = 0;
        while (elapsedTime < lungeTime)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, lungeSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.8f);
        transform.GetChild(0).gameObject.SetActive(false);
        isAttacking = false;
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
