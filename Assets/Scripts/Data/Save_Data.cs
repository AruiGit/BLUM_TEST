using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save_Data
{
    public Player_Data playerToSave;

    public Save_Data(Player_Controler player)
    {

        playerToSave = new Player_Data(player);
    }
}
