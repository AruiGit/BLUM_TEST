using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    bool isDead = false;

    //Sprite and animations
    [SerializeField]SpriteRenderer sprite;
    [SerializeField]Animator playerAnimator;

    //Movement
    float horizontalInput;
    int speed = 5;
    float maxSpeed;
    float maxJumpHight = 10;
    float jumpHight;
    Rigidbody2D rb;
    bool isGrounded;
    bool isPreparingToJump = false;
    int dir;
    RaycastHit2D ray;
    Collider2D collider;

    //Stats
    [SerializeField]int healthPoints = 3;
    int maxHealthPoints;
    int money = 30;
    int damage = 3;

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
    public bool haveSecretKey = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxHealthPoints = healthPoints;
        attackSound = GetComponent<AudioSource>();
        maxSpeed = speed;
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isDead == false)
        {
            Movement();
            Jump();
            Attack();
            Dash();
        }

        if (healthPoints <= 0)
        {
            isDead = true;
            playerAnimator.SetTrigger("isDead");
            StartCoroutine(deathTimer());
            collider.enabled = false;
            rb.gravityScale = 0;
        }
    }
    void Movement()
    {
        if (isPreparingToJump == false && isColliding==false)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            Vector2 direction = new Vector2(horizontalInput, 0);
            playerAnimator.SetBool("isRuning", true);
            ray = Physics2D.Raycast(transform.position, direction, 0.7f, LayerMask.GetMask("Map"));
            if(ray.collider!= null)
            {
                    return;
            }
            
            if (horizontalInput != 0) 
            {
                // transform.Translate(direction * speed * Time.deltaTime);
                 rb.velocity= new Vector2 (horizontalInput * speed,rb.velocity.y);
              
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
        if (isColliding == true)
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

        if (isGrounded == false)
        {
            isPreparingToJump = false;
        }

        if (rb.velocity.y < -0.2f)
        {
            playerAnimator.SetBool("isFalling", true);
        }
        else
        {
            playerAnimator.SetBool("isFalling", false);
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Dashing");
          //  rb.AddForce(new Vector2(1000 * dir, 0));
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
                Debug.Log(enemy.gameObject);
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
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Enemy") && isColliding == false)
        {
            if (collision.gameObject.GetComponent<Shroom>() != null)
            {
                collision.gameObject.GetComponent<Shroom>().CrushDamage(damage);
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
        if (collision.gameObject.CompareTag("Death_Bringer"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 15);
            ChangeHealth(-1);
            isColliding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Death_Bringer"))
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
        if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("Spike"))
        {
            isColliding = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isColliding == true)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Death_Bringer"))
        {
            ChangeHealth(-1);
            Vector2 dir = transform.position - collision.transform.position;
            if (dir.x > 0)
            {
                rb.velocity=new Vector2(7, 0);
            }
            else
            {
                rb.velocity=new Vector2(-7, 0);
            }
            isColliding = true;
        }
        if(collision.gameObject.CompareTag("Spike"))
        {
            ChangeHealth(-1);
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
    public bool GetSecretKey()
    {
        return haveSecretKey;
    }
    public void ChangeDamage(int value)
    {
        damage += value;
    }
    public bool CheckDeath()
    {
        return isDead;
    }
    public void ChangeCollision()
    {
        isColliding = !isColliding;
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
    IEnumerator deathTimer()
    {
        yield return new WaitForSeconds(0.517f);
        sprite.enabled = false;
    }
    public IEnumerator StunTime(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeCollision();
    }
}
