using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible_Coin : Collectible
{
    public override void OnCollect(Player_Controler player)
    {
       base.OnCollect(player);
        StartCoroutine(AnimationTime(0.2f));
        player.AddCoints(value);
    }
}
