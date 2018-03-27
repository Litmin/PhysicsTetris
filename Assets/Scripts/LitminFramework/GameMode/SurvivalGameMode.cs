using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalGameMode :AbstractGameMode
{
    private int deadLine;

    private int FalloutCount = 0;

    private bool CheckGameEnd(Player player)
    {
        if(FalloutCount >= 3)
        {
            return true;
        }
        return false;
    }

    public void BindFalloutScreenEvent(Brick brick)
    {
        brick.FalloutScreen += delegate { FalloutCount++; };
    }


}
