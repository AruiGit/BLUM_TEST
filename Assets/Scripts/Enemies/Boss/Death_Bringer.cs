using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Bringer : Enemy
{
    bool attackHitted = false;
    int dir;
    bool canAttack = true;
    public GameObject spellPrefab;
    bool isCasting = false;

    protected override void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        enemyAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dyingSound = GetComponent<AudioSource>();
        enemyColliders = GetComponents<Collider2D>();
        finishDistance = 2f;
    }

    protected override void Update()
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
            Cast();
        }
        else
        {
            
            if (isPlaying == false)
            {
                enemyAnimator.SetTrigger("isDead");
                isPlaying = true;
                dyingSound.Play();
                rb.gravityScale = 0;
                foreach (Collider2D col in enemyColliders)
                {
                    col.enabled = false;
                }
            }
            rb.velocity = new Vector2(0, 0);
            
            StartCoroutine(DeathTimer(0.767f));
        }
    }

    void Attack()
    {
        
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 6  && player.transform.position.y - transform.position.y < 3 && canAttack==true)
        {
            canAttack = false;
            canMove = false;
            StartCoroutine(Attacking(0.767f));
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
        }
        if (attackHitted == true)
        {
            player.ChangeCollision();
            StartCoroutine(player.StunTime(1f));
            player.TakeDamage(damage, dir * 10);
            attackHitted = false;
        }
    }

    void Cast()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 6 && player.transform.position.y - transform.position.y > 3 && canAttack == true)
        {
            isCasting = true;
            canMove = false;
            canAttack = false;
            StartCoroutine(Attacking(0.716f));
            enemyAnimator.SetTrigger("isCasting");
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

    IEnumerator Attacking(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        canMove = true;
        if (isCasting == true)
        {
            Instantiate(spellPrefab, new Vector2(player.transform.position.x, player.transform.position.y + 3), Quaternion.identity);
            isCasting = false;
        }
        yield return new WaitForSeconds(1.5f- attackTime);
        canAttack = true;
    }
}
