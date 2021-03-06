﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AbstractGameMode
{
    public abstract bool CheckGameEnd();

    public abstract void RestartGame();
}
