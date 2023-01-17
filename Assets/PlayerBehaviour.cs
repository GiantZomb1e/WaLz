using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float maxSpeed = 0.50f;
    public float moveSpeed = 1.00f;
    public float jumpForce = 10.0f;
    private Rigidbody2D rb;
    private bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(0f,0f,0f);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D (Collider2D collide)
    {
        Debug.Log("collision!");
        if(collide.gameObject.tag == ("Tile")  && collide.isTrigger)
        {
            grounded = true;
        }

    }
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(horizontal, 0);
        Vector3 newPosition = transform.position + new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed;
        transform.position = newPosition;

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            grounded = false;
        }
    }
}

