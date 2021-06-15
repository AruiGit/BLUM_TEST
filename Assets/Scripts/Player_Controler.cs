using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    bool isDeath = false;

    //Sprite and animations
    [SerializeField]SpriteRenderer sprite;
    [SerializeField]Animator playerAnimator;

    //Movement
    float horizontalInput;
    int speed = 5;
    float maxJumpHight = 10;
    float jumpHight;
    Rigidbody2D rb;
    bool isGrounded;
    bool isPreparingToJump = false;
    int dir;

    //Stats
    [SerializeField]int healthPoints = 3;
    int maxHealthPoints;
    int money = 0;
    int damage = 1;

    //Attack
    [SerializeField]Transform attackPosition;
    [SerializeField] float attackRange = 0.5f;
    bool canAttack = true;
    bool isAttacking = false;
    bool damageDealt = false;
    AudioSource attackSound;

    //TakingDamage
    bool canTakeDamage = true;
    bool isColliding = false;

    //Envo
    bool haveSecretKey = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxHealthPoints = healthPoints;
        attackSound = GetComponent<AudioSource>();
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

    }

    void Movement()
    {
        if (isPreparingToJump == false )
        {
            horizontalInput = Input.GetAxis("Horizontal");
            Vector2 direction = new Vector2(horizontalInput, 0);
            playerAnimator.SetBool("isRuning", true);
            if (horizontalInput != 0)
            {
                transform.Translate(direction * speed * Time.deltaTime);
            }
            if (horizontalInput < 0 && isAttacking == false)
            {
                sprite.flipX = true;
                playerAnimator.SetBool("isFlipped", true);
                attackPosition.localPosition =new Vector2(-0.135f, 0);
                dir = -1;
            }
            else if(horizontalInput>0 && isAttacking == false)
            {
                playerAnimator.SetBool("isFlipped", false);
                sprite.flipX = false;
                attackPosition.localPosition = new Vector2(0.135f, 0);
                dir = 1;
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
            attackSound.Play();
            isAttacking = true;
           Collider2D[] enemiesArrey = Physics2D.OverlapCircleAll(attackPosition.position, attackRange,LayerMask.GetMask("Enemy"));
            playerAnimator.SetTrigger("isAttacking");
            canAttack = false;
            StartCoroutine(attackCooldown());
            
            foreach(Collider2D enemy in enemiesArrey)
            {
                if (damageDealt == false)
                {
                    damageDealt = true;
                    enemy.gameObject.GetComponent<Enemy>().TakeDamage(damage, dir);
                    Debug.Log(enemy.gameObject.name);
                }

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

        if (value <= 0)
        {
            if (canTakeDamage == true)
            {
                healthPoints += value;
                canTakeDamage = false;
                StartCoroutine(TakeDamage());
            }

        }
        else
        {
            healthPoints += value;
        }
        


        if (healthPoints > maxHealthPoints)
        {
            healthPoints = maxHealthPoints;
        }
       
    }
    public int GetHealth()
    {
        return healthPoints;
    }

    public void TakeDamage(int value, int direction)
    {
       
            
            ChangeHealth(-value);
            rb.AddForce(new Vector2(125 * direction, 0));
           
        
        
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Dotykam ziemi triggerem");
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy") && isColliding == false)
        {
            if (collision.gameObject.GetComponent<Enemy>().CheckEnemyType() == true)
            {
                collision.gameObject.GetComponent<Enemy>().CrushDamage(damage);
                rb.velocity = new Vector2(rb.velocity.x, 7);
                ChangeHealth(0);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 7);
                ChangeHealth(-1);
            }
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isColliding = false;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isColliding = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isColliding == false)
        {
            ChangeHealth(-1);
            Vector2 dir = transform.position - collision.transform.position;
            if (dir.x > 0)
            {
                // rb.velocity = new Vector2(5, rb.velocity.y);
                rb.AddForce(new Vector2(350, 0));
            }
            else
            {
                //rb.velocity = new Vector2(-5, rb.velocity.y);
                rb.AddForce(new Vector2(-350, 0));
            }
            isColliding = true;
        }

    }

    public void ChangeMaxHealth(int value)
    {
        maxHealthPoints += value;
    }
    public int GetMaxHealth()
    {
        return maxHealthPoints;
    }
    
    public void ChangeSecretKey()
    {
        haveSecretKey = !haveSecretKey;
    }
    public void ChangeDamage(int value)
    {
        damage += value;
    }

    IEnumerator TakeDamage()
    {
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }
    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(0.517f);
        canAttack = true;
        isAttacking = false;
        damageDealt = false;
        playerAnimator.ResetTrigger("isAttacking");
    }
}
