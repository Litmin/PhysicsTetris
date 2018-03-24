using System.Collections;
using System.Collections.Generic;

public class GameRoot : MonoBehaviourSingleton<GameRoot>
{
    public void Init()
    {
        //初始化游戏的所有资源
        InitAllGameResource();
 

    }

    private void InitAllGameResource()
    {
        ResourceManager.instance.RegisterResource("Music_Menu", "Audio/Music/044_Menu", false);
    }
}

