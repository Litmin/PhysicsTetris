using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : ICommand
{
    public void excute(Player player)
    {
        player.ControlBrick.Move(false);
    }
}
