using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public bool grounded = false;
    public Rigidbody2D PlayerRigidBody2D;
    public float walkspeed = 5;
    public float jumpstrenght = 5;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(0f,0f,0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D (Collision2D collide)
    {
        Debug.Log("collision!");
        if(collide.gameObject.tag == ("Tile"))
        {
            grounded = true;
        }

    }
    void FixedUpdate()
    {
        if(grounded && Input.GetKey(KeyCode.Space))
        {
           grounded = false;
           PlayerRigidBody2D.velocity = Vector2.up * jumpstrenght;
        }
        if (Input.GetKey(KeyCode.D) )
        {
            PlayerRigidBody2D.velocity = Vector2.right * walkspeed;
        }

        if (Input.GetKey(KeyCode.A) )
        {
            PlayerRigidBody2D.velocity = Vector2.left * walkspeed;
        }

        if (Input.GetKey(KeyCode.S) )
        {
            PlayerRigidBody2D.velocity = Vector2.down * walkspeed;
        }
    }
}
