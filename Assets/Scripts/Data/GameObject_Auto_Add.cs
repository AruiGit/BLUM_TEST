using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObject_Auto_Add : MonoBehaviour
{
    void Awake()
    {
        if (gameObject.name == "Game_Manager")
        {
            GameObject_Manager.instance.allObjects.Insert(0, gameObject);
        }
        else
        {
            GameObject_Manager.instance.allObjects.Add(gameObject);
        }
        
        Debug.Log(gameObject + " dodane do listy");
    }

    void OnDestroy()
    {
        GameObject_Manager.instance.allObjects.Remove(gameObject);
    }
}
