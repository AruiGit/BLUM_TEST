using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    bool isDeath = false;

    //Sprite and animations
    SpriteRenderer sprite;
    Animator playerAnimator;

    //Movement
    float horizontalInput;
    int speed = 5;
    float maxJumpHight = 10;
    float jumpHight;
    Rigidbody2D rb;
    bool isGrounded;
    bool isPreparingToJump = false;

    //Stats
    [SerializeField]int healthPoints = 3;
    int money = 0;
    int damage = 1;
    float attackSpeed = 1; //attacksPerSec

    //Attack
    [SerializeField]Transform attackPosition;
    [SerializeField] float attackRange = 0.5f;
    bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath == false)
        {
            Movement();
            Jump();
            Attack();
        }

        if (healthPoints <= 0)
        {
            isDeath = true;
            playerAnimator.SetTrigger("isDead");
        }

        Debug.Log(rb.velocity.y);
    }

    void Movement()
    {
        if (isPreparingToJump == false)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            Vector2 direction = new Vector2(horizontalInput, 0);
            playerAnimator.SetBool("isRuning", true);
            if (horizontalInput != 0)
            {
                transform.Translate(direction * speed * Time.deltaTime);
            }
            if (horizontalInput < 0)
            {
                sprite.flipX = true;
                attackPosition.localPosition =new Vector2(-0.082f,0);
            }
            else
            {
                sprite.flipX = false;
                attackPosition.localPosition = new Vector2(0.082f, 0);
            }
            
        }
        if (horizontalInput == 0)
        {
            playerAnimator.SetBool("isRuning", false);
        }
    }
    void Jump()
    {
        if (isGrounded == true)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                
                rb.velocity = new Vector2(rb.velocity.x, jumpHight);
                playerAnimator.ResetTrigger("prepToJump");
                playerAnimator.SetBool("isJumping", true);
                isGrounded = false;
                jumpHight = 0;
            }

            if (Input.GetKey(KeyCode.Space))
            {
               
                jumpHight += 10 * Time.deltaTime;
                jumpHight = Mathf.Clamp(jumpHight, 3f, maxJumpHight);
                playerAnimator.SetTrigger("prepToJump");
                isPreparingToJump = true;
                
            }
            else
            {
                isPreparingToJump = false;
            }

            if (isGrounded == true)
            {
                playerAnimator.SetBool("isJumping", false);
            }
        }

        if (rb.velocity.y < 0)
        {
            playerAnimator.SetBool("isFalling", true);
        }
        else
        {
            playerAnimator.SetBool("isFalling", false);
        }

    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.E) && canAttack==true)
        {
            
           Collider2D[] enemiesArrey = Physics2D.OverlapCircleAll(attackPosition.position, attackRange,LayerMask.GetMask("Enemy"));
            playerAnimator.SetTrigger("isAttacking");
            canAttack = false;
            StartCoroutine(attackCooldown());

            foreach(Collider2D enemy in enemiesArrey)
            {
                enemy.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPosition == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    public void AddCoints(int value)
    {
        money += value;
    }
    public int GetCoins()
    {
        return money;
    }

    public void ChangeHealth(int value)
    {
        healthPoints += value;
       
    }
    public int GetHealth()
    {
        return healthPoints;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Dotykam ziemi triggerem");
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            rb.velocity = new Vector2(rb.velocity.x, 5);
        }
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ChangeHealth(-1);
            Vector2 dir = transform.position - collision.transform.position;
            if (dir.x > 0)
            {
                rb.velocity = new Vector2(5, 0);
            }
            else
            {
                rb.velocity = new Vector2(-5, 0);
            }
        }

    }

    
    

    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(1 / attackSpeed);
        canAttack = true;
        playerAnimator.ResetTrigger("isAttacking");
    }
}
