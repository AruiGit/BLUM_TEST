using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible_Coin : Collectible
{
    public override void OnCollect()
    {
        base.OnCollect();
        StartCoroutine(AnimationTime(0.2f));
        player.AddCoints(value);
    }
}
