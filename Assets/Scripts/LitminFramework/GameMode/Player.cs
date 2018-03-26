using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //方块的类型
    public enum BrickType
    {
        I,
        J,
        L,
        O,
        S,
        T,
        Z
    }
    //生成方块的旋转
    public enum BrickRotate
    {
        Zero,
        Ninety,
        OneEightZero,
        TwoSevenZero
    }

    //游戏中所有方块
    public List<Brick> m_AllBricks;

    //当前控制的方块
    public Brick ControlBrick;

    //最高的方块
    public Brick HighestBrick;

    //方块生成的点
    public Vector3 BrickGenPoint;

    //用来生成方块的prefab
    public List<GameObject> BrickPrefab;

    //下一个方块
    public BrickType NextBrickType;


	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
    public void StartGame()
    {

    }
    public void Restart()
    {

    }

    public void Pause()
    {

    }

    public void Resume()
    {

    }

    public void GenBrick()
    {

    }

    private void HandleBrickFall2Physics()
    {

    }
}
