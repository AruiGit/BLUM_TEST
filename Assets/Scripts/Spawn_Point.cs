using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Point : MonoBehaviour
{
    public Transform spawnPosition;
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        player.transform.position = spawnPosition.position;
    }
}
