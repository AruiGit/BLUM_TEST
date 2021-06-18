using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible_Heart : Collectible
{
    public override void OnCollect()
    {
        animator.SetTrigger("pickedHeart");
        base.OnCollect();
        StartCoroutine(AnimationTime(0.517f));
        player.ChangeHealth(1);
    }
}
