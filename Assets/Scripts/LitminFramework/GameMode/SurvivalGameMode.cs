using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalGameMode :AbstractGameMode
{
    private int FalloutCount = 0;

    public event Action<int> OnFalloutCountChange;

    public override bool CheckGameEnd()
    {
        if(FalloutCount >= 3)
        {
            return true;
        }
        return false;
    }

    public void BrickFalloutEvent( )
    {
       FalloutCount++;
        if(OnFalloutCountChange != null)
        {
            OnFalloutCountChange(FalloutCount);
        }
    }
}
