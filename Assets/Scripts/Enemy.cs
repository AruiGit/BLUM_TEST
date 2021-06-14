using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    int currentPatrolPoint = 0;
    float finishDistance = 0.5f;
    float movementStep = 2;

    int healthPoints = 3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Debug.Log(currentPatrolPoint);
    }

    void Movement()
    {
        if (patrolPoints != null)
        {
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

    public void TakeDamage(int value)
    {
        healthPoints -= value;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
