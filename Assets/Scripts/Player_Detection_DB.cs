using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Detection_DB : Player_Detection
{
    
    protected override void Start()
    {
        enemy = GetComponentInParent<Death_Bringer>();
    }


}
