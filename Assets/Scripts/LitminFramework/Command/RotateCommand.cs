using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCommand : ICommand
{
    public void excute(Player player)
    {
        player.ControlBrick.Rotate();
    }
}
