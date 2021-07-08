using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemies_Data
{
    public int hp;
    public int patrolPoints;
    public float[] position;
    public int typeId;

    public Enemies_Data(Enemy enemy)
    {
        hp = enemy.HealthPoints;
        patrolPoints = enemy.patrolPoints.Length;
        position = new float[2];
        position[0] = enemy.transform.position.x;
        position[1] = enemy.transform.position.y;
        typeId = enemy.type;
    }
}
