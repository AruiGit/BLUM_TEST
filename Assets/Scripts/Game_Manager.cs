using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{


    //Player UI
    [SerializeField] Text cointText;
    [SerializeField] Image[] healthBars;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] Text restartGame;

    Player_Controler player;


    //Shop
    [SerializeField]GameObject shop;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Controler>();
        shop.SetActive(false);
        restartGame.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        CoinUiUpdate();
        HearthUiUpdate();
        if (player.CheckDeath() == true)
        {
            restartGame.enabled = true;
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void CoinUiUpdate()
    {
        cointText.text = "Coins: " + player.GetCoins();
    }

    void HearthUiUpdate()
    {
        if (player.GetMaxHealth() == 4)
        {
            healthBars[3].enabled = true;
        }
        else
        {
            healthBars[3].enabled = false;
        }
        int playerHealth = player.GetHealth();

        if (playerHealth == 4)
        {
            foreach (Image healthBar in healthBars)
            {
                healthBar.sprite = fullHeart;
            }
        }
        if (playerHealth == 3)
        {
            healthBars[3].sprite = emptyHeart;

            healthBars[2].sprite = fullHeart;
            healthBars[1].sprite = fullHeart;
            healthBars[0].sprite = fullHeart;
        }
        if (playerHealth == 2)
        {
            healthBars[2].sprite = emptyHeart;
            healthBars[3].sprite = emptyHeart;

            healthBars[1].sprite = fullHeart;
            healthBars[0].sprite = fullHeart;
        }
        if (playerHealth == 1)
        {
            healthBars[2].sprite = emptyHeart;
            healthBars[1].sprite = emptyHeart;
            healthBars[3].sprite = emptyHeart;

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

    public void OpenShop()
    {
        shop.active = true;
    }

    public void CloseShop()
    {
        shop.active = false;
    }
}
