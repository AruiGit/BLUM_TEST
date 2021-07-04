using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    Player_Controler player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Player_Controler>();
        if (player != null)
        {
            if (player.GetSecretKey() >0)
            {
                player.ChangeSecretKey(-1);
                Destroy(this.gameObject);
            }
        }
    }
}
