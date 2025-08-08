using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public float speed = 10.0f;
    private Rigidbody2D rb;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        
    }
    
    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }
}
