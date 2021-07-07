using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    int currentPatrolPoint = 0;
    protected float finishDistance = 0.5f;
    protected float movementStep = 2;
    protected Animator enemyAnimator;
    protected Rigidbody2D rb;
    protected bool isFlipped;
    protected bool canMove = true;
    protected bool canTakeDamage = true;

    protected bool isPlaying = false;
    protected AudioSource dyingSound;
    protected Collider2D[] enemyColliders;

    protected SpriteRenderer sprite;

    //Stats
    public int healthPoints = 2;
    public int damage = 1;

    //Drops
    [SerializeField]GameObject coinPrefab,hearthPrefab;

    //Player_Detection
    protected bool playerSeen = false;
    protected Player_Controler player;

    protected virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dyingSound = GetComponent<AudioSource>();
        enemyColliders = GetComponents<Collider2D>();
        player = GameObject_Manager.instance.player.GetComponent<Player_Controler>();
    }
    protected virtual void Update()
    {
        if (player == null)
        {
            player = GameObject_Manager.instance.player.GetComponent<Player_Controler>();
        }
        if(healthPoints > 0)
        {
            Movement();
        }
        else
        {
            if (isPlaying == false)
            {
                enemyAnimator.SetTrigger("isDead");
                isPlaying = true;
                dyingSound.Play();
                rb.gravityScale = 0;
                foreach(Collider2D col in enemyColliders)
                {
                    col.enabled = false;
                }
            }
            rb.velocity = new Vector2(0, 0);
            
            StartCoroutine(DeathTimer(0.5f));
        }
    }

    protected virtual void Movement()
    {
        if (patrolPoints != null && canMove == true && patrolPoints.Length!=0)
        {
            if(transform.position.x- patrolPoints[currentPatrolPoint].position.x > 0)
            {
                sprite.flipX = true;
                isFlipped = true;
            }
            else
            {
                sprite.flipX = false;
                isFlipped = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolPoint].position, movementStep*Time.deltaTime);

            if (Mathf.Abs(Vector2.Distance(transform.position, patrolPoints[currentPatrolPoint].position))  <= finishDistance)
            {
                if (currentPatrolPoint < patrolPoints.Length-1)
                {
                    currentPatrolPoint++;
                }
                else
                {
                    currentPatrolPoint = 0;
                }
            }
        }
    }
    public void TakeDamage(int value, int direction)
    {
        if (canTakeDamage == true)
        {
            canTakeDamage = false;
            healthPoints -= value;
            rb.AddForce(new Vector2(250 * direction, 0));
            enemyAnimator.SetTrigger("isHit");
            StartCoroutine(TakeDamage());
        }
    }
    public bool CheckIfFlipped()
    {
        return isFlipped;
    }
    public void Flip(bool value)
    {
        sprite.flipX = value;
    }
    public void StopStartMovement(bool value)
    {
        canMove = value;
    }
    void Drop()
    {
        int dropID = Random.Range(0, 100);
        if (dropID < 50)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        else if (dropID >= 50)
        {
            Instantiate(hearthPrefab, transform.position, Quaternion.identity);
        }
    }

    public void PlayerDetection(bool value, Player_Controler player)
    {
        playerSeen = value;
        this.player = player;
    }
    public void PlayerDetection(bool value)
    {
        playerSeen = value;
    }

    protected IEnumerator DeathTimer(float deathTimeAnim)
    {
        yield return new WaitForSeconds(deathTimeAnim);
        Drop();
        Destroy(gameObject);
    }
    protected IEnumerator TakeDamage()
    {
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }
}
