using DG.Tweening;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController instance;
    [SerializeField] GameObject weaponHitbox;
    [SerializeField] AudioClip attackSound;
    public float walkSpeed = 10.0f, runSpeed = 20.0f;
    private float speed;
    private Rigidbody2D rb;
    private float moveInput;
    private bool facingRight = true;
    bool isAttacking = false;
    Animator anim;
    AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region States
    public enum Movement
    {
        Idle,
        Walking,
        Running,
        Fainted
    }
    public enum Weapon
    {
        Armed,
        Unarmed
    }
    public enum GameState
    {
        Playing,
        Paused
    }
    public enum Attacking
    {
        Attacking,
        NotAttacking
    }

    public Movement movement = Movement.Idle;
    public Weapon weapon = Weapon.Armed;
    public GameState gameState = GameState.Paused;
    public Attacking attacking = Attacking.NotAttacking;

    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = walkSpeed;
        weaponHitbox.SetActive(false);
        weaponHitbox.GetComponent<WeaponHitbox>().owner = this;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    #region Movement
    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (movement != Movement.Fainted && gameState == GameState.Playing)
        {
            if (Mathf.Abs(moveInput) > 0.01f)
            {
                anim.SetFloat("speed", 0.5f);
                if (!audioSource.isPlaying) audioSource.Play();
                if (Input.GetKey(KeyCode.LeftShift))
                    movement = Movement.Running;
                else
                    movement = Movement.Walking;
            }
            else
            {
                anim.SetFloat("speed", 0);
                movement = Movement.Idle;
                if (audioSource.isPlaying) audioSource.Stop();
            }
        }

        switch (movement)
        {
            case Movement.Idle:
                speed = 0;
                break;
            case Movement.Walking:
                speed = walkSpeed;
                break;
            case Movement.Running:
                speed = runSpeed;
                break;
            case Movement.Fainted:
                Faint();
                break;
        }

        switch (weapon)
        {
            case Weapon.Armed:
                weaponHitbox.SetActive(true);
                break;
            case Weapon.Unarmed:
                weaponHitbox.SetActive(false);
                break;
        }

        //attack
        if (Input.GetMouseButton(0) && movement != Movement.Fainted && weapon == Weapon.Armed && gameState == GameState.Playing)
        {
            attacking = Attacking.Attacking;
            Attack();
        }
        else
        {
            attacking = Attacking.NotAttacking;
        }
    }

    void FixedUpdate()
    {
        if (movement == Movement.Fainted) return;
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        if (facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (!facingRight && moveInput < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Faint()
    {
        movement = Movement.Fainted;
        speed = 0;
        anim.SetFloat("speed", 0);
        transform.DORotate(
            new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90f),
            1f,
        RotateMode.Fast
        );

    }
    #endregion
    #region States Cont'd
    public void ResetStates()
    {
        transform.DORotate(
            new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f),
            1f,
        RotateMode.Fast
        );

        movement = Movement.Idle;
        gameState = GameState.Playing;
        attacking = Attacking.NotAttacking;
    }

    public void Unpause()
    {
        gameState = GameState.Playing;
    }
    
    public void Pause()
    {
        gameState = GameState.Paused;
    }
    #endregion

    #region Combat
    void Attack()
    {
        StartCoroutine(AttackRoutine());
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        AudioSource.PlayClipAtPoint(attackSound, transform.position, 0.5f);
        weaponHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        weaponHitbox.SetActive(false);
        isAttacking = false;
    }
    #endregion

}
