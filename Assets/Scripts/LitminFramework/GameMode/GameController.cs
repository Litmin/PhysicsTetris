using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameType
    {
        Survival,
        Jigsaw,
        Racing
    }
    //游戏类型
    public GameType m_GameType;

    private SurvivalGameMode m_GameMode;

    //玩家人数
    public int m_PlayerNum;
    public List<Player> m_Players;

    //游戏经过的帧数
    public long FramesPast = 0;

    //是否开启回放
    public bool Record = true;

    //暂停
    public bool m_Pause = false;

    //UI
    public GameSceneView uiView;

	void Start ()
    {
		switch(m_GameType)
        {
            case GameType.Jigsaw:
                break;
            case GameType.Racing:
                break;
            case GameType.Survival:
                m_GameMode = new SurvivalGameMode();
                break;
            default:
                break;
        }
        m_Players = new List<Player>();
        for(int i = 0;i < m_PlayerNum;i++)
        {
            m_Players.Add(new Player());
        }

        m_Players[0].OnGenBrick += m_GameMode.BindFalloutScreenEvent;

        //绑定UI事件
        uiView.OnMoveLeft += this.MoveLeft;
        uiView.OnMoveRight += this.MoveRight;
        uiView.OnSpeedUp += this.SpeedUp;
        uiView.OnSpeedDown += this.SpeedDown;
        uiView.OnRotate += this.Rotate;

        //开始游戏
        m_Players[0].StartGame();
    }
	
	void Update ()
    {
        FramesPast++;

    }

    //游戏暂停
    public void Pause()
    {
        m_Pause = true;
        m_Players[0].Pause();
    }
    //恢复游戏
    public void Resume()
    {
        m_Pause = false;
        m_Players[0].Resume();
    }

    //重新开始
    public void Restart()
    {

    }

    //左移
    public void MoveLeft()
    {
        if (m_Players[0].bGameOver || m_Pause)
            return;
        new MoveLeftCommand().excute(m_Players[0]);
    }

    //右移
    public void MoveRight()
    {
        if (m_Players[0].bGameOver || m_Pause)
            return;
        new MoveRightCommand().excute(m_Players[0]);
    }

    //加速
    public void SpeedUp()
    {
        if (m_Players[0].bGameOver || m_Pause)
            return;
        new SpeedUpCommand().excute(m_Players[0]);
    }

    //减速
    public void SpeedDown()
    {
        if (m_Players[0].bGameOver || m_Pause)
            return;
        new SpeedDownCommand().excute(m_Players[0]);
    }

    //旋转
    public void Rotate()
    {
        if (m_Players[0].bGameOver || m_Pause)
            return;
        new RotateCommand().excute(m_Players[0]);
    }
}
