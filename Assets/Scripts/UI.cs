using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private static UI uiInstance;

    void Awake()
    {
        if (uiInstance == null)
        {
            DontDestroyOnLoad(this);
            uiInstance = this;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Destroy(gameObject);
        }
    }
}
