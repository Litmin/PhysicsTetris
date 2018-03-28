using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private AbstractGameMode m_GameMode;

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

    //游戏结束事件
    public event Action OnGameEnd;
    //游戏重新开始事件
    public event Action OnGameRestart;

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

        //绑定方块掉落事件
        if(m_GameMode is SurvivalGameMode)
        {
            m_Players[0].OnBrickFallOut += (m_GameMode as SurvivalGameMode).BrickFalloutEvent;
            //绑定更新生命值事件
            (m_GameMode as SurvivalGameMode).OnFalloutCountChange += uiView.ChangeHeartCount;
        }

        //绑定UI事件
        uiView.OnMoveLeft += this.MoveLeft;
        uiView.OnMoveRight += this.MoveRight;
        uiView.OnSpeedUp += this.SpeedUp;
        uiView.OnSpeedDown += this.SpeedDown;
        uiView.OnRotate += this.Rotate;

        OnGameEnd += uiView.HandleGameEnd;
        OnGameRestart += uiView.HandleGameRestart;
        m_Players[0].OnChangeNextBrickView += uiView.ChangeNextBrickIcon;

        //开始游戏
        m_Players[0].StartGame();

        //播放音乐
        AudioManager.instance.StopAllMusic();
        AudioManager.instance.PlayMusic("Music_SurvivalGame");
    }


    void Update ()
    {
        FramesPast++;

        if(m_GameMode.CheckGameEnd())
        {
            //游戏结束
            if(OnGameEnd != null)
            {
                OnGameEnd();
            }
            m_Players[0].GameEnd();
        }
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
