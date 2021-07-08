using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Death_Bringer : Enemy
{
    bool attackHitted = false;
    public int dir;
    public bool canAttack = true;
    public GameObject spellPrefab;
    bool isCasting = false;
    int stamina = 3;
    int maxStamina;
    public ParticleSystem particle;
    bool isRegenerating = false;
    Vector2 tempPlayerPosition;
    Vector2 particleLocation;
    public Camera_Movement camera;

    //UI
    [SerializeField] Slider staminaSlider;
    [SerializeField] Boss_Area bossArea;

    protected override void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        enemyAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dyingSound = GetComponent<AudioSource>();
        enemyColliders = GetComponents<Collider2D>();
        finishDistance = 2f;
        maxStamina = stamina;
        staminaSlider.maxValue = maxStamina;
        particleLocation = particle.transform.localPosition;
    }

    protected override void Update()
    {
        if (player == null)
        {
           player = GameObject_Manager.instance.player.GetComponent<Player_Controler>();
        }
        if (bossArea == null)
        {
            bossArea = GameObject_Manager.instance.bossArea.GetComponent<Boss_Area>();
            staminaSlider = GameObject_Manager.instance.bossArea.GetComponentInChildren<Slider>();
        }
        UpdateUI();
        if (healthPoints > 0 && playerSeen == false)
        {
            base.Movement();
            canMove = true;
            enemyAnimator.SetBool("isWalking", true);
            stamina = maxStamina;
            
        }
        else if(healthPoints>0 && playerSeen == true)
        {
            if (isRegenerating == false)
            {
                Attack();
                Cast();
                Movement();
            }
            if(stamina==0)
            {
                Regenerate();
                enemyAnimator.SetBool("isWalking", false);
                isRegenerating = true;
            }
            AttackHit();
        }
        else
        {
            
            if (isPlaying == false)
            {
                bossArea.BossDead();
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
            player.IsDashUnlocked = true;
            StartCoroutine(DeathTimer(0.767f));
        }
        
    }

    void Attack()
    {
        
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 6  && player.transform.position.y - transform.position.y < 3 && canAttack==true && stamina >0 )
        {
            canAttack = false;
            canMove = false;
            StartCoroutine(Attacking(0.8f, 1.5f));
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
            if (stamina == 1)
            {
                
                if (dir < 0)
                {
                    particle.transform.localPosition = new Vector2(-particleLocation.x, particle.transform.localPosition.y);
                }
                else
                {
                    particle.transform.localPosition = new Vector2(particleLocation.x, particle.transform.localPosition.y);
                }
                
                StartCoroutine(ActivateParticle());
            }
            enemyAnimator.SetTrigger("isAttacking");
            stamina--;
        }
        
        
    }
    void AttackHit()
    {
        if (attackHitted == true)
        {
            player.ChangeCollision();
            player.TakeDamage(damage, dir * 2);
            attackHitted = false;
            StartCoroutine(player.StunTime(0.5f));
        }
    }
    void Cast()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 6 && player.transform.position.y - transform.position.y > 3 && canAttack == true && stamina > 0)
        {
            stamina--;
            isCasting = true;
            canMove = false;
            canAttack = false;
            tempPlayerPosition = player.transform.position;
            StartCoroutine(Attacking(0.716f, 3f));
            enemyAnimator.SetTrigger("isCasting");
        }
    }

    void Regenerate()
    {
        canAttack = false;
        StartCoroutine(Regeneration());
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            canMove = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            canMove = true;
        }
    }
    void UpdateUI()
    {
        staminaSlider.value = stamina;
    }


    IEnumerator ActivateParticle()
    {
        yield return new WaitForSeconds(0.33f);
        particle.Play();
        StartCoroutine(camera.CameraShake(0.5f, 0.2f));
    }
    IEnumerator Attacking(float attackTime, float attackCooldown)
    {
        yield return new WaitForSeconds(attackTime);
        canMove = true;
        if (isCasting == true)
        {
            Instantiate(spellPrefab, new Vector2(tempPlayerPosition.x, tempPlayerPosition.y + 3), Quaternion.identity);
            isCasting = false;
        }
        yield return new WaitForSeconds(attackCooldown- attackTime);
        canAttack = true;
    }

    IEnumerator Regeneration()
    {
        for(int i = 0; i < maxStamina; i++)
        {
            if (stamina < maxStamina)
            {
                stamina++;
            }
            yield return new WaitForSeconds(2f);
        }
        isRegenerating = false;
    }
}
