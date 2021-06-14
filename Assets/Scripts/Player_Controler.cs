using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    //Movement
    float horizontalInput;
    int speed = 5;
    float jumpHight = 7;
    Rigidbody2D rb;

    //Stats
    int healthPoints = 2;
    int money = 0;
    int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        Vector2 direction = new Vector2(horizontalInput, 0);
        if (horizontalInput != 0)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHight);
        }
        
    }

    public void AddCoints(int value)
    {
        money += value;
    }
}
