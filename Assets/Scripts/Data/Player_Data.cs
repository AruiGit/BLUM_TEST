using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player_Data
{
    public int money;
    public int health;
    public int maxHealth;
    public int damage;
    public bool isDashUnlocked;
    public float[] position;
    public int keys;
    public int sceneID;

    public Player_Data(Player_Controler player)
    {
        money = player.Coins;
        health = player.HealthPoints;
        maxHealth = player.MaxHealthPoints;
        damage = player.Damage;
        isDashUnlocked = player.IsDashUnlocked;
        position = new float[3];
        position[0] = player.GetPlayerPosition().x;
        position[1] = player.GetPlayerPosition().y;
        position[2] = player.GetPlayerPosition().z;
        sceneID = player.GetActiveSceneID();
        keys = player.SecretKeys;
    }
}
