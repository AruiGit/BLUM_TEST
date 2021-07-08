using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObject_Data
{
    public enum type
    {
            Shroom,
            Goblin,
            Coin,
            Heart,
            Boss,
            Doors,
    }
    public int damage;
    public float[] position;
    public int hp;
    public int[][] patrolPoints;
    public int value;

    public GameObject_Data()
    {

    }
}
