using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public float speed = 10.0f;
    private Rigidbody2D rb;
    private float moveInput;

    enum State
    {
        // Moving,
        // Paused,
        // Dead
    }
    //State state = State.Paused;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        // switch (state)
        // {
        //     case State.Moving:
        //         break;
        //     case State.Dead:
        //         break;
        //     case State.Paused:
        //         break;
        // }
    }
    
    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }
}
