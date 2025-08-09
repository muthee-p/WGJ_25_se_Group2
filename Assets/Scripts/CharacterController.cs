using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController instance;
    public float walkSpeed = 10.0f, runSpeed = 20.0f;
    private float speed;
    private Rigidbody2D rb;
    private float moveInput;

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
        Walking,
        Running,
        Fainted
    }
    public enum Weapon
    {
        Armed,
        Unarmed
    }
    public enum Soberiety
    {
        Sober,
        Hallucinating
    }
    public Movement movement = Movement.Walking;
    public Weapon weapon = Weapon.Unarmed;
    public Soberiety soberiety = Soberiety.Hallucinating;

    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = walkSpeed;
    }

    void Update()
    {
        if (movement == Movement.Fainted) return;

        switch (movement)
        {
            case Movement.Walking:
                speed = walkSpeed;
                moveInput = Input.GetAxis("Horizontal");
                break;
            case Movement.Running:
                speed = runSpeed;
                moveInput = Input.GetAxis("Horizontal");
                break;
            case Movement.Fainted:
                break;
        }

        //run
        if (Input.GetKey(KeyCode.LeftShift) && movement == Movement.Walking)
        {
            movement = Movement.Running;
        }
        else if (movement != Movement.Fainted)
        {
            movement = Movement.Walking;
        }
    }

    void FixedUpdate()
    {
        if (movement == Movement.Fainted) return;
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    public void Faint()
    {
        movement = Movement.Fainted;
    }

    public void ResetStates()
    {
        movement = Movement.Walking;
        weapon = Weapon.Unarmed;
        soberiety = Soberiety.Hallucinating;
    }

}
