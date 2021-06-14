using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    int currentPatrolPoint = 0;
    float finishDistance = 0.5f;
    float movementStep = 2;
    [SerializeField] bool isShroomType = true;
    Animator enemyAnimator;
    Rigidbody2D rb;

    SpriteRenderer sprite;

    int healthPoints = 2;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (healthPoints <= 0)
        {
            enemyAnimator.SetTrigger("isDead");
            StartCoroutine(DeathTimer());
            
        }
    }

    void Movement()
    {

        if (patrolPoints != null)
        {

            if(transform.position.x- patrolPoints[currentPatrolPoint].position.x > 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
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
        healthPoints -= value;
        rb.AddForce(new Vector2(125 * direction, 0));
        
    }

    public void CrushDamage(int value)
    {
        healthPoints -= value;
        enemyAnimator.SetTrigger("isCrushed");
        Debug.Log("Gniecienie przeciwnika");
       // enemyAnimator.ResetTrigger("isCrushed");
       
    }

    public bool CheckEnemyType()
    {
        return isShroomType;
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
