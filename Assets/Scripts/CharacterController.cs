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
                audioSource.Play();
                break;
            case Movement.Running:
                speed = runSpeed;
                break;
            case Movement.Fainted:
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
        if (Input.GetMouseButton(0) && movement != Movement.Fainted && weapon == Weapon.Armed)
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
        transform.localRotation = Quaternion.Euler(0, 0, -90);
    }

    public void ResetStates()
    {
        movement = Movement.Walking;
        weapon = Weapon.Unarmed;
        gameState = GameState.Paused;
        attacking = Attacking.NotAttacking;
    }

    void Attack()
    {

        StartCoroutine(AttackRoutine());
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        AudioSource.PlayClipAtPoint(attackSound, transform.position);
        weaponHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        weaponHitbox.SetActive(false);
    }

    public void Unpause()
    {
        gameState = GameState.Playing;
    }
    
    public void Pause()
    {
        gameState = GameState.Paused;
    }

}
