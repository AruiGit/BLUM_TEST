using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    Player_Controler player;
    [SerializeField] GameObject HP_UP, Secret_Key, DMG_UP;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Controler>();
    }


    public void BuyHPUp()
    {
        if (player.GetCoins() >= 10)
        {
            player.AddCoints(-10);
            player.ChangeMaxHealth(1);
            HP_UP.SetActive(false);
        }
    }

    public void BuySecretKey()
    {
        if (player.GetCoins() >= 15)
        {
            player.AddCoints(-15);
            player.ChangeSecretKey();
            Secret_Key.SetActive(false);
        }
    }

    public void BuyDamageUP()
    {
        if (player.GetCoins() >= 20)
        {
            player.AddCoints(-20);
            player.ChangeDamage(1);
            DMG_UP.SetActive(false);
        }
    }
}
