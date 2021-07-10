using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObject_Auto_Add : MonoBehaviour
{
    void Awake()
    {
        GameObject_Manager.instance.allObjects.Add(gameObject);
    }

    void OnDestroy()
    {
        GameObject_Manager.instance.allObjects.Remove(gameObject);
    }
}
