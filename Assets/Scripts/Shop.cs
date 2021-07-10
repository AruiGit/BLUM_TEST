using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    Player_Controler player;
    [SerializeField] GameObject HP_UP, Secret_Key, DMG_UP;
    [SerializeField] Text HpPriceText, KeyPriceText, DmgPriceText;
    int hpPrice;
    int dmgPrice;
    int keyPrice ;
    int startHpPrice = 10;
    int startDmgPrice = 20;
    int startKeyPrice = 1;
    void Start()
    {
        hpPrice = startHpPrice;
        dmgPrice = startDmgPrice;
        keyPrice = startKeyPrice;
        player = GameObject_Manager.instance.player.GetComponent<Player_Controler>();
        SetPrices();
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject_Manager.instance.player.GetComponent<Player_Controler>();
        }
        if (player.MaxHealthPoints == 6)
        {
            HP_UP.SetActive(false);
        }
    }

    public void SetPrices()
    {
        if (player.MaxHealthPoints > 3)
        {
            if (player.MaxHealthPoints == 6)
            {
                HP_UP.SetActive(false);
            }
            for (int i = 3; i < player.MaxHealthPoints; i++)
            {
                hpPrice = PriceUpdate(hpPrice);
            }
        }

        for(int i = 1; i < player.DmgUPBought; i++)
        {
            dmgPrice = PriceUpdate(dmgPrice);
        }

        for(int i = 0; i < player.KeysBought; i++)
        {
            keyPrice = PriceUpdate(keyPrice);
        }
        UpdateUI();
    }

    public void BuyHPUp()
    {
        if (player.Coins >= hpPrice)
        {
            player.Coins = -hpPrice;
            player.MaxHealthPoints = 1;
            hpPrice= PriceUpdate(hpPrice);
            if (player.MaxHealthPoints == 6)
            {
                HP_UP.SetActive(false);
            }

            UpdateUI();
        }
    }
    public void BuySecretKey()
    {
        if (player.Coins >= keyPrice)
        {
            player.Coins = -keyPrice;
            player.SecretKeys = 1;
            player.KeysBought += 1;
            Debug.Log(player.KeysBought);
            keyPrice=PriceUpdate(keyPrice);

            UpdateUI();
        }
    }
    public void BuyDamageUP()
    {
        if (player.Coins >=dmgPrice)
        {
            player.Coins = -dmgPrice;
            player.Damage = 1;
            player.DmgUPBought += 1;
            dmgPrice = PriceUpdate(dmgPrice);

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
    public void ResetShop()
    {
        keyPrice = startKeyPrice;
        dmgPrice = startDmgPrice;
        hpPrice = startHpPrice;
        HP_UP.SetActive(true);
        UpdateUI();
    }
    void UpdateUI()
    {
        HpPriceText.text = "Max Health UP  Price: " + hpPrice;
        KeyPriceText.text = "Secret Key  Price: " + keyPrice;
        DmgPriceText.text = "DMG UP  Price: " + dmgPrice;
    }
}
