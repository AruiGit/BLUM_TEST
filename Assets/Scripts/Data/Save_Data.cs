using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save_Data
{
    public List<Enemies_Data> enemiesToLoad = new List<Enemies_Data>();
    public Player_Data playerToSave;

    public Save_Data(List<Enemy> listToLoad, Player_Controler player)
    {
        foreach(Enemy enemy in listToLoad)
        {
            enemiesToLoad.Add(new Enemies_Data(enemy));
        }
        playerToSave = new Player_Data(player);
    }
}
