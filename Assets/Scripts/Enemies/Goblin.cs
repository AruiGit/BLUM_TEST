using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    [SerializeField]Transform attackPosition;
    [SerializeField] float attackRange;
    Vector3 attackPositionInit;
    bool canAttack = true;
    int dir;

    protected override void Start()
    {
        base.Start();
        attackPositionInit = attackPosition.localPosition;
    }
    protected override void Update()
    {
        base.Update();
        AttackPositionUptade();
    }

    void AttackPositionUptade()
    {
        if (CheckIfFlipped() == true)
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopStartMovement(false);
            if(transform.position.x-collision.transform.position.x < 0)
            {
                Flip(false);
            }
            else
            {
                Flip(true);
            }
            Attack();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopStartMovement(true);
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

    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }
}
