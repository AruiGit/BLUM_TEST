using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]int value = 1;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (gameObject.CompareTag("Coin"))
            {
                collision.GetComponent<Player_Controler>().AddCoints(value);
                Destroy(this.gameObject);
            }


            
        }
    }
}
