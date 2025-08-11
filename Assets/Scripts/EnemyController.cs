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
    private Transform player;
    private GameObject canvas;
    bool canvasShown = false;
    bool audioPlayed = false;
    int maxHealth = 30;
    AudioSource audioSource;
    Animator anim;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        initialPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = transform.Find("Canvas").gameObject;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSight && distanceFromPlayer > attackZone)
        {
            anim.SetFloat("speed", 0.5f);
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, player.position.x, speed * Time.deltaTime), transform.position.y, transform.position.z);

            if (!audioPlayed)
            {
                audioSource.clip = enemyInSight;
                audioSource.Play();
                audioPlayed = true;
            }

            if (!canvasShown)
            {
                canvasShown = true;
                canvas.SetActive(true);
            }
        }
        else if (distanceFromPlayer < attackZone && !isAttacking)
        {
            isAttacking = true;
            canvas.SetActive(false);
            StartCoroutine(AttackRoutine());
        }
        else
        {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, initialPos.x, speed * Time.deltaTime), transform.position.y, transform.position.z);
            isAttacking = false;
            audioPlayed = false;
            audioSource.Stop();
            audioSource.clip = null;
        }

        if (player.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (transform.position == initialPos)
        {
            anim.SetFloat("speed", 0);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("Player"))
        // {
        //     caught = true;
        //     HealthScript.instance.GameOver("You have been caught!");
        // }
    }

    public void TakeDamage(int damage)
    {
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            GameController.instance.AddDeadEnemy(gameObject);
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
