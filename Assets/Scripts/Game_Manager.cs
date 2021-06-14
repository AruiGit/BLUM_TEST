using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{

    [SerializeField] Text cointText;
    [SerializeField] Image[] healthBars;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    Player_Controler player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Controler>();
    }

    // Update is called once per frame
    void Update()
    {
        CoinUiUpdate();
        HearthUiUpdate();
    }

    void CoinUiUpdate()
    {
        cointText.text = "Coins: " + player.GetCoins();
    }

    void HearthUiUpdate()
    {
        int playerHealth = player.GetHealth();

        if (playerHealth == 3)
        {
            foreach(Image healthBar in healthBars)
            {
                healthBar.sprite = fullHeart;
            }
        }
        if (playerHealth == 2)
        {
            healthBars[2].sprite = emptyHeart;

            healthBars[1].sprite = fullHeart;
            healthBars[0].sprite = fullHeart;
        }
        if (playerHealth == 1)
        {
            healthBars[2].sprite = emptyHeart;
            healthBars[1].sprite = emptyHeart;

            healthBars[0].sprite = fullHeart;
        }
        if (playerHealth <= 0)
        {
            foreach (Image healthBar in healthBars)
            {
                healthBar.sprite = emptyHeart;
            }
        }
    }
}
