using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack : MonoBehaviour
{

   public Death_Bringer enemy;

    void Awake()
    {
        enemy=GetComponentInParent<Death_Bringer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.AttackHitted();
        }
    }
}
