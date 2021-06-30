using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Bringer : Enemy
{
    bool attackHitted = false;
    int dir;
    bool canAttack = true;

    protected override void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        enemyAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dyingSound = GetComponent<AudioSource>();
        enemyColliders = GetComponents<Collider2D>();
        finishDistance = 1f;
    }

    void Update()
    {
        if (healthPoints > 0 && playerSeen == false)
        {
            base.Movement();
            enemyAnimator.SetBool("isWalking", true);
        }
        else if(healthPoints>0 && playerSeen == true)
        {
            Movement();
            Attack();
        }
        else
        {
            enemyAnimator.SetBool("isWalking", false);
            if (isPlaying == false)
            {
                isPlaying = true;
                dyingSound.Play();
                rb.gravityScale = 0;
                foreach (Collider2D col in enemyColliders)
                {
                    col.enabled = false;
                }
            }
            rb.velocity = new Vector2(0, 0);
            enemyAnimator.SetTrigger("isDead");
            StartCoroutine(DeathTimer());
        }
    }

    void Attack()
    {
        
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 6  && player.transform.position.y - transform.position.y < 3 && canAttack==true)
        {
            canAttack = false;
            canMove = false;
            StartCoroutine(Attacking());
            if (isFlipped == true)
            {
                dir = -1;
                enemyAnimator.SetBool("isFlipped", true);
            }
            else
            {
                dir = 1;
                enemyAnimator.SetBool("isFlipped", false);
            }
            enemyAnimator.SetTrigger("isAttacking");

            if (attackHitted == true)
            {
                
                player.TakeDamage(damage,1);
                attackHitted = false;
            }
        }
    }

    public void AttackHitted()
    {
        attackHitted = true;
    }

    protected override void Movement()
    {
        if (canMove == true)
        {
            if (transform.position.x - player.transform.position.x > 0)
            {
                sprite.flipX = true;
                isFlipped = true;
                enemyAnimator.SetBool("isWalking", true);
            }
            else
            {
                sprite.flipX = false;
                isFlipped = false;
                enemyAnimator.SetBool("isWalking", true);
            }
            if (transform.position.x - player.transform.position.x == 0)
            {
                enemyAnimator.SetBool("isWalking", false);
            }
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), movementStep * Time.deltaTime);
        }
    }

    IEnumerator Attacking()
    {
        yield return new WaitForSeconds(0.767f);
        canMove = true;
        yield return new WaitForSeconds(1.5f- 0.767f);
        canAttack = true;
        
    }
   
}
