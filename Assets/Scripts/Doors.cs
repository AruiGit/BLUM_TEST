using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (collision.GetComponent<Player_Controler>().GetSecretKey() == true)
            {
                collision.GetComponent<Player_Controler>().ChangeSecretKey();
                Destroy(gameObject);
            }
        }
    }
}
