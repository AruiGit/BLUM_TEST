using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObject_Manager : MonoBehaviour
{
    public static GameObject_Manager instance;
    public List<GameObject> allObjects = new List<GameObject>();
<<<<<<< HEAD
=======
    public GameObject gameManager;
    public GameObject player;
    public GameObject camera;
>>>>>>> parent of e66e14e (Save/Load)

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
