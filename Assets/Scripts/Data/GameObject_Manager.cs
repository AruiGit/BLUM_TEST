using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObject_Manager : MonoBehaviour
{
    public static GameObject_Manager instance;
<<<<<<< HEAD
    public List<GameObject> allCollectibles = new List<GameObject>();
    public List<Enemy> allEnemies = new List<Enemy>();
    public List<GameObject> allDoors = new List<GameObject>();
    public GameObject gameManager;
    public GameObject player;
    public GameObject camera;
    public GameObject bossArea;
    public bool wasGameLoaded = false;

    public Save_Data data;
=======
    public List<GameObject> allObjects = new List<GameObject>();
>>>>>>> parent of 659a430 (Singleton, Qol)

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Tworze singletona!");
        }

        else
        {
            Destroy(gameObject);
        }
            
    }
}
