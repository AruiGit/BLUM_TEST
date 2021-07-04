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
    [SerializeField] GameObject playerPrefab;

    //Shop
    [SerializeField]GameObject shop;

    private static Game_Manager gameManagerInstance;

    void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gameManagerInstance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Controler>();
        shop.SetActive(false);
        restartGame.enabled = false;
        for(int i = 3; i < 6; i++)
        {
            healthBars[i].enabled = false;
        }
        
    }
    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player_Controler>();
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Destroy(gameObject);
        }
        UpdateUI();
        if (player.CheckDeath() == true)
        {
            restartGame.enabled = true;
            if (Input.GetKeyDown(KeyCode.R))
            {
                player.DestroyPlayer();
                Instantiate(playerPrefab);
                ReloadUI();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            restartGame.enabled = false;
        }
    }

    void CoinUiUpdate()
    {
        cointText.text = "Coins: " + player.GetCoins();
    }
    void HearthUiUpdate()
    {
        
        healthBars[player.GetMaxHealth()-1].enabled = true;

        int playerHealth = player.GetHealth();

        for (int i = 0; i < player.GetMaxHealth(); ++i)
        {
            healthBars[i].sprite = playerHealth <= i ? emptyHeart : fullHeart;
        }
    }
    void UpdateUI()
    {
        CoinUiUpdate();
        HearthUiUpdate();
    }

    void ReloadUI()
    {
        for(int i = 0; i < 3; i++)
        {
            healthBars[3 + i].enabled = false;
        }
        shop.GetComponent<Shop>().ResetShop();
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
