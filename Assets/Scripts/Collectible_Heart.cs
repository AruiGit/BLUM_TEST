using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible_Heart : Collectible
{
    public override void OnCollect(Player_Controler player)
    {
        animator.SetTrigger("pickedHeart");
        base.OnCollect(player);
        StartCoroutine(AnimationTime(0.517f));
        player.HealthPoints = 1;
    }
}
