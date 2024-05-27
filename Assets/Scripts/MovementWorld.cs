using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementWorld : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb2D;

    Vector2 movement;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        if(movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
            
            rb2D.rotation = angle;
        }
    }
    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
