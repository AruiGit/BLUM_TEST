using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    Player_Controler player;
    [SerializeField] GameObject HP_UP, Secret_Key, DMG_UP;
    [SerializeField] Text HpPriceText, KeyPriceText, DmgPriceText;
    int HpPrice = 10;
    int DmgPrice = 20;
    int KeyPrice = 1;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Controler>();
        UpdateUI();
    }


    public void BuyHPUp()
    {
        if (player.GetCoins() >= HpPrice)
        {
            player.AddCoints(-HpPrice);
            player.ChangeMaxHealth(1);
            HpPrice= PriceUpdate(HpPrice);
            if (player.GetMaxHealth() == 6)
            {
                HP_UP.SetActive(false);
            }

            UpdateUI();
        }
    }
    public void BuySecretKey()
    {
        if (player.GetCoins() >= KeyPrice)
        {
            player.AddCoints(-KeyPrice);
            player.ChangeSecretKey();
            KeyPrice=PriceUpdate(KeyPrice);

            UpdateUI();
        }
    }
    public void BuyDamageUP()
    {
        if (player.GetCoins() >= DmgPrice)
        {
            player.AddCoints(-DmgPrice);
            player.ChangeDamage(1);
            DmgPrice = PriceUpdate(DmgPrice);

            UpdateUI();
        }
    }

    int PriceUpdate(int price)
    {
        if (price == 1)
        {
            price = 15;
        }
        else
        {
            price *= 2;
        }
        return price;
    }

    void UpdateUI()
    {
        HpPriceText.text = "Max Health UP  Price: " + HpPrice;
        KeyPriceText.text = "Secret Key  Price: " + KeyPrice;
        DmgPriceText.text = "DMG UP  Price: " + DmgPrice;
    }
}
