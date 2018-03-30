using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordFile
{
    public enum OperateCommand
    {
        MoveLeft,
        MoveRight,
        SpeedUp,
        SpeedDown,
        Rotate
    }

    //游戏结束时经过的帧数
    public long GameEndFrame;
    //本局游戏的随机数种子
    public int randomSeed;

    public struct OperateRecord
    {
        public OperateRecord(OperateCommand operateCommand,long inframe)
        {
            m_OperateCommand = operateCommand;
            frame = inframe;
        }
        public OperateCommand m_OperateCommand;
        public long frame;
    }

    public List<OperateRecord> OperateCommandList = new List<OperateRecord>();

    public void AddMoveLeftCommand(long frame)
    {
        OperateCommandList.Add(new OperateRecord(OperateCommand.MoveLeft, frame));
    }
    public void AddMoveRightCommand(long frame)
    {
        OperateCommandList.Add(new OperateRecord(OperateCommand.MoveRight, frame));
    }
    public void AddSpeedUpCommand(long frame)
    {
        OperateCommandList.Add(new OperateRecord(OperateCommand.SpeedUp, frame));
    }
    public void AddSpeedDownCommand(long frame)
    {
        OperateCommandList.Add(new OperateRecord(OperateCommand.SpeedDown, frame));
    }
    public void AddRotateCommand(long frame)
    {
        OperateCommandList.Add(new OperateRecord(OperateCommand.Rotate, frame));
    }

}
