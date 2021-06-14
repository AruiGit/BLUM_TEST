using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField]Transform attackPosition;
    [SerializeField] float attackRange;
    int damage = 1;
    Vector3 attackPositionInit;
    bool canAttack = true;
    int dir;

    Enemy enemy;
   Animator enemyAnimator;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        attackPositionInit = attackPosition.localPosition;
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackPositionUptade();
        //enemyAnimator = enemy.GetAnimator();
    }

    void AttackPositionUptade()
    {
        if (enemy.CheckIfFlipped() == true)
        {
            attackPosition.localPosition = -attackPositionInit;
            dir = -1;
        }
        else
        {
            attackPosition.localPosition = attackPositionInit;
            dir = 1;
        }
    }
    void Attack()
    {
        if (canAttack == true)
        {
            Collider2D[] playerArrey = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, LayerMask.GetMask("Player"));
            enemyAnimator.SetTrigger("isAttacking");
            canAttack = false;
            StartCoroutine(attackCooldown());

            foreach (Collider2D player in playerArrey)
            {
                player.gameObject.GetComponent<Player_Controler>().TakeDamage(damage, dir);
            }
        }

        
    }
    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.StopStartMovement(false);
            if(transform.position.x-collision.transform.position.x < 0)
            {
                enemy.Flip(false);
            }
            else
            {
                enemy.Flip(true);
            }
            Attack();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.StopStartMovement(true);
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
}
