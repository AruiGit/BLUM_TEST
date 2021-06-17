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

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Controler>();
        shop.SetActive(false);
        restartGame.enabled = false;
        
    }
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

        for (int i = 0; i < player.GetMaxHealth(); ++i)
        {
            healthBars[i].sprite = playerHealth <= i ? emptyHeart : fullHeart;
        }
    }
    public void OpenShop()
    {
        shop.SetActive(true);
    }
    public void CloseShop()
    {
        shop.SetActive(false);
    }
}
