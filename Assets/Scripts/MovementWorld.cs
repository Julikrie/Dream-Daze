using UnityEngine;

public class MovementWorld : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb2D;
    private Animator animator;

    Vector2 movement;
    void Start()
    {
        Cursor.visible = false;

        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        // Diagonal Movement same speed as in Y and X direction
        movement = movement.normalized;

        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        animator.SetBool("isMoving", movement != Vector2.zero);
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
