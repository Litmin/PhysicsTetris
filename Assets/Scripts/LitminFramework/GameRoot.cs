using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameRoot : MonoBehaviourSingleton<GameRoot>
{

    public void Init()
    {
        //初始化游戏的所有资源
        InitAllGameResource();
 

    }

    private void InitAllGameResource()
    {
        //注册音乐
        ResourceManager.instance.RegisterResource("Music_Menu", "Audio/Music/044_Menu", false);
        ResourceManager.instance.RegisterResource("Music_SurvivalGame", "Audio/Music/000_Track_01_single_speed", false);
        //注册音效
        ResourceManager.instance.RegisterResource("Sfx_BrickSpawn", "Audio/Sfx/004_Brick_spawn", false);
        ResourceManager.instance.RegisterResource("Sfx_BrickLand", "Audio/Sfx/007_Brick_land", false);
        ResourceManager.instance.RegisterResource("Sfx_BrickLandheavy", "Audio/Sfx/007_Brick_land_heavy", false);
        ResourceManager.instance.RegisterResource("Sfx_Finishtower", "Audio/Sfx/016_Finish_tower", false);
        ResourceManager.instance.RegisterResource("Sfx_Rotatebrick", "Audio/Sfx/028_Rotate_brick", false);
        ResourceManager.instance.RegisterResource("Sfx_Wingameloop", "Audio/Sfx/046_Win_game_loop", false);
        ResourceManager.instance.RegisterResource("Sfx_SelectItem01", "Audio/Sfx/051_Select_item_01", false);
        ResourceManager.instance.RegisterResource("Sfx_SelectItem02", "Audio/Sfx/051_Select_item_02", false);
        ResourceManager.instance.RegisterResource("Sfx_SelectItem03", "Audio/Sfx/051_Select_item_03", false);
        ResourceManager.instance.RegisterResource("Sfx_SelectItem04", "Audio/Sfx/051_Select_item_04", false);
        ResourceManager.instance.RegisterResource("Sfx_MoveBrick", "Audio/Sfx/056_Move_brick_v2", false);
        ResourceManager.instance.RegisterResource("Sfx_NudgeBrick", "Audio/Sfx/059_Nudge_brick_v2", false);

        ResourceManager.instance.RegisterResource("Sfx_BrickFallWater01", "Audio/Sfx/038_Water_splash_01", false);
        ResourceManager.instance.RegisterResource("Sfx_BrickFallWater02", "Audio/Sfx/038_Water_splash_02", false);
        ResourceManager.instance.RegisterResource("Sfx_BrickFallWater03", "Audio/Sfx/038_Water_splash_03", false);
        ResourceManager.instance.RegisterResource("Sfx_BrickFallWater04", "Audio/Sfx/038_Water_splash_04", false);
        ResourceManager.instance.RegisterResource("Sfx_BrickLandSpring01", "Audio/Sfx/206_Bricks_spring_01", false);
        ResourceManager.instance.RegisterResource("Sfx_BrickLandSpring02", "Audio/Sfx/206_Bricks_spring_02", false);
        ResourceManager.instance.RegisterResource("Sfx_BrickLandSpring03", "Audio/Sfx/206_Bricks_spring_03", false);



    }
}

