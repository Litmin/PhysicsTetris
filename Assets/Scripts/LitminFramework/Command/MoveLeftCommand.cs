using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : ICommand
{
    public void excute(Player player)
    {
        player.ControlBrick.Move(true);
    }
}
