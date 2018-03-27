using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDownCommand : ICommand
{
    public void excute(Player player)
    {
        player.ControlBrick.FallSpeedToNormal();
    }
}
