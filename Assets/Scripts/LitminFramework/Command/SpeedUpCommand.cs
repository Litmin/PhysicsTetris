using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpCommand : ICommand
{
    public void excute(Player player)
    {
        player.ControlBrick.FallSpeedToMax();
    }
}
