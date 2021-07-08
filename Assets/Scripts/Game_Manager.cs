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

    [SerializeField] Player_Controler currentPlayer;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject spawnedPlayer;

    //Shop
    [SerializeField]GameObject shop;

    //Menu
    [SerializeField] GameObject pauseMenu;
    bool isMenuOpened = false;

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
        GameObject_Manager.instance.gameManager = this.gameObject;
    }
    void Start()
    {
        shop.SetActive(false);
        restartGame.enabled = false;
        pauseMenu.SetActive(false);
        for(int i = 3; i < 6; i++)
        {
            healthBars[i].enabled = false;
        }
        currentPlayer = GameObject.Find("Player").GetComponent<Player_Controler>();
       
        
    }
    void Update()
    {
        if (currentPlayer==null)
        {
            currentPlayer = GameObject_Manager.instance.player.GetComponent<Player_Controler>();
        }
        if (GameObject_Manager.instance.gameManager == null)
        {
            GameObject_Manager.instance.gameManager = this.gameObject;
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Destroy(gameObject);
        }
        if (currentPlayer.enabled == false)
        {
            currentPlayer.enabled = true;
        }
        UpdateUI();
        PauseMenu();
        if (currentPlayer.CheckDeath() == true)
        {
            restartGame.enabled = true;
            if (Input.GetKeyDown(KeyCode.R))
            {
                currentPlayer.DestroyPlayer();
                Player_Controler newPlayer=Instantiate(playerPrefab).GetComponent<Player_Controler>();
                if (GameObject_Manager.instance.wasGameLoaded == true)
                {
                    newPlayer.ReloadPlayer(GameObject_Manager.instance.data);
                }
                ReloadUI();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            restartGame.enabled = false;
        }
    }
    private void OnDestroy()
    {
        GameObject_Manager.instance.gameManager = null;
    }

    void CoinUiUpdate()
    {
        cointText.text = "Coins: " + currentPlayer.Coins;
    }
    void HearthUiUpdate()
    {
        for(int i = 0; i < currentPlayer.MaxHealthPoints; i++)
        {
            healthBars[i].enabled = true;
        }


        int playerHealth = currentPlayer.HealthPoints;

        for (int i = 0; i < currentPlayer.MaxHealthPoints; ++i)
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
    public void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOpened = !isMenuOpened;
            pauseMenu.SetActive(isMenuOpened);
        }
    }
    public void SavePlayer()
    {
        currentPlayer.SavePlayer();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void GetPlayer(Player_Controler player)
    {
        currentPlayer = player;
        Debug.Log(player);
    }
}
