﻿using System.Collections;
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

    //是否开启记录回放
    public bool Record = true;

    //观看回放完成
    public bool bLookRecordOver = false;

    //玩家操作还是回放中
    public bool bPlayerOperate = true;

    private RecordFile m_RecordFile;

    private int RecordOperateIndex = 0;

    //暂停
    public bool m_Pause = false;

    //UI
    public GameSceneView uiView;

    //游戏结束事件
    public event Action OnGameEnd;
    //游戏重新开始事件
    public event Action OnGameRestart;
    //分数变化
    public event Action<int> OnScoreChange;

    public bool bGameEnd = false;

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

        uiView.OnRestart += this.Restart;
        uiView.OnLookRecordClick += this.StartLookRecord;

        OnGameEnd += uiView.HandleGameEnd;
        OnGameRestart += uiView.GameEndMenuMoveOut;
        OnScoreChange += uiView.HandleScoreChange;
        m_Players[0].OnChangeNextBrickView += uiView.ChangeNextBrickIcon;

        //开始游戏
        m_Players[0].StartGame();

        //播放音乐
        AudioManager.instance.StopAllMusic();
        AudioManager.instance.PlayMusic("Music_SurvivalGame");

        m_RecordFile = new RecordFile();
        m_RecordFile.randomSeed = m_Players[0].RandomSeed;
    }

    System.Random a;
    void Update ()
    {
        FramesPast++;
        if (bPlayerOperate &&!bGameEnd && m_GameMode.CheckGameEnd())
        {
            bGameEnd = true;
            //游戏结束
            if(OnGameEnd != null)
            {
                OnGameEnd();
            }

            m_RecordFile.GameEndFrame = FramesPast;
     
            m_Players[0].GameEnd();
            m_Players[0].Score = m_Players[0].m_AllBricks.Count;
            if (OnScoreChange != null)
            {
                OnScoreChange(m_Players[0].Score);
            }

            AudioManager.instance.StopMusic("Music_SurvivalGame");
            AudioManager.instance.PlayMusic("Sfx_Wingameloop");
        }
        if(!bPlayerOperate && bLookRecordOver == false)
        {
            if(FramesPast >= m_RecordFile.GameEndFrame)
            {
                bLookRecordOver = true;
                //回放结束
                if (OnGameEnd != null)
                {
                    OnGameEnd();
                }
                m_Players[0].GameEnd();
                AudioManager.instance.StopMusic("Music_SurvivalGame");
                AudioManager.instance.PlayMusic("Sfx_Wingameloop");
            }
            else
            {
                if (RecordOperateIndex < m_RecordFile.OperateCommandList.Count)
                {
                    //还有操作
                    if(FramesPast >= m_RecordFile.OperateCommandList[RecordOperateIndex].frame)
                    {
                        switch (m_RecordFile.OperateCommandList[RecordOperateIndex].m_OperateCommand)
                        {
                            case RecordFile.OperateCommand.MoveLeft:
                                MoveLeft();
                                break;
                            case RecordFile.OperateCommand.MoveRight:
                                MoveRight();
                                break;
                            case RecordFile.OperateCommand.SpeedUp:
                                SpeedUp();
                                break;
                            case RecordFile.OperateCommand.SpeedDown:
                                SpeedDown();
                                break;
                            case RecordFile.OperateCommand.Rotate:
                                Rotate();
                                break;
                            default:
                                break;
                        }
                        RecordOperateIndex++;
                    }
                }

            }
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
        m_Players[0].RestartGame();
        m_Players[0].StartGame();

        m_RecordFile.randomSeed = m_Players[0].RandomSeed;

        m_GameMode.RestartGame();
        bGameEnd = false;
        AudioManager.instance.StopMusic("Sfx_Wingameloop");
        AudioManager.instance.PlayMusic("Music_SurvivalGame");
        FramesPast = 0;
        bPlayerOperate = true;
        m_RecordFile.OperateCommandList.Clear();

        uiView.canOperate = true;
    }
    //开始回放
    public void StartLookRecord()
    {
        bLookRecordOver = false;

        m_Players[0].RestartGame();

        m_Players[0].RandomSeed = m_RecordFile.randomSeed;
        m_Players[0].random = new System.Random(m_Players[0].RandomSeed);

        m_Players[0].StartGame();

        m_GameMode.RestartGame();
        bGameEnd = false;
        AudioManager.instance.StopMusic("Sfx_Wingameloop");
        AudioManager.instance.PlayMusic("Music_SurvivalGame");
        FramesPast = 0;
        bPlayerOperate = false;
        RecordOperateIndex = 0;

        uiView.canOperate = false;
    }


    //左移
    public void MoveLeft()
    {
        if (m_Players[0].bGameOver || m_Pause)
            return;
        new MoveLeftCommand().excute(m_Players[0]);
        if(Record && bPlayerOperate)
        {
            m_RecordFile.AddMoveLeftCommand(FramesPast);
        }
    }

    //右移
    public void MoveRight()
    {
        if (m_Players[0].bGameOver || m_Pause)
            return;
        new MoveRightCommand().excute(m_Players[0]);
        if (Record && bPlayerOperate)
        {
            m_RecordFile.AddMoveRightCommand(FramesPast);
        }
    }

    //加速
    public void SpeedUp()
    {
        if (m_Players[0].bGameOver || m_Pause)
            return;
        new SpeedUpCommand().excute(m_Players[0]);
        if (Record && bPlayerOperate)
        {
            m_RecordFile.AddSpeedUpCommand(FramesPast);
        }
    }

    //减速
    public void SpeedDown()
    {
        if (m_Players[0].bGameOver || m_Pause)
            return;
        new SpeedDownCommand().excute(m_Players[0]);
        if (Record && bPlayerOperate)
        {
            m_RecordFile.AddSpeedDownCommand(FramesPast);
        }
    }

    //旋转
    public void Rotate()
    {
        if (m_Players[0].bGameOver || m_Pause)
            return;
        new RotateCommand().excute(m_Players[0]);
        if (Record && bPlayerOperate)
        {
            m_RecordFile.AddRotateCommand(FramesPast);
        }
    }

}
