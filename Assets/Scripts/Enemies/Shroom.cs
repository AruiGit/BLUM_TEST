using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shroom : Enemy
{

    public void CrushDamage(int value)
    {
        if (canTakeDamage == true)
        {
            canTakeDamage = false;
            healthPoints -= value;
            hpBar.value = healthPoints;
            enemyAnimator.SetTrigger("isCrushed");
            StartCoroutine(TakeDamage());
        }
    }
}
