using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObject_Manager : MonoBehaviour
{
    public static GameObject_Manager instance;
    public List<GameObject> allObjects = new List<GameObject>();

    public GameObject gameManager;
    public GameObject player;
    public GameObject camera;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
            
    }
}
