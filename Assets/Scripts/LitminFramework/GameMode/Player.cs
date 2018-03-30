using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player
{
    //方块的类型
    public enum BrickType
    {
        I = 0,
        J = 1,
        L = 2,
        O = 3,
        S = 4,
        T = 5,
        Z = 6
    }

    public event Action<int> OnChangeNextBrickView;

    public event Action OnBrickFallOut;

    //本次得分
    public int Score;

    //游戏中所有方块
    public List<Brick> m_AllBricks = new List<Brick>();

    //当前控制的方块
    public Brick ControlBrick;

    //最高的方块
    public Brick HighestBrick;

    //方块生成的点
    public Vector3 BrickGenPoint;

    //下一个方块
    public BrickType NextBrickType;

    //游戏结束
    public bool bGameOver = false;

    //方块prefab
    private UnityEngine.Object BrickIprefab;
    private UnityEngine.Object BrickJprefab;
    private UnityEngine.Object BrickLprefab;
    private UnityEngine.Object BrickOprefab;
    private UnityEngine.Object BrickSprefab;
    private UnityEngine.Object BrickTprefab;
    private UnityEngine.Object BrickZprefab;

    private Transform BrickParent;

    //第一次生成方块
    private bool firstGen = true;

    //用来产生随机数的种子
    public int RandomSeed;
    public System.Random random;
    public Player()
    {
        //设置随机数种子
        RandomSeed = UnityEngine.Random.Range(1, 999);
        random = new System.Random(RandomSeed);


        //加载prefab  多人游戏需修改
        BrickIprefab = Resources.Load("Brick/BrickI");
        BrickJprefab = Resources.Load("Brick/BrickJ");
        BrickLprefab = Resources.Load("Brick/BrickL");
        BrickOprefab = Resources.Load("Brick/BrickO");
        BrickSprefab = Resources.Load("Brick/BrickS");
        BrickTprefab = Resources.Load("Brick/BrickT");
        BrickZprefab = Resources.Load("Brick/BrickZ");

        BrickParent = GameObject.FindGameObjectWithTag("BricksParent").transform;

        BrickGenPoint = new Vector3(0, 35, 0);
    }

    public void StartGame()
    {
        var brick = GenBrick();
        ControlBrick = brick;
        m_AllBricks.Add(brick);
    }

    public void RestartGame()
    {
        for(int i = 0;i < m_AllBricks.Count;i++)
        {
            if(m_AllBricks[i].gameObject != null)
            GameObject.Destroy(m_AllBricks[i].gameObject);
        }
        ControlBrick = null;
        m_AllBricks.Clear();
        BrickGenPoint = new Vector3(0, 35, 0);
        //设置随机数种子
        RandomSeed = UnityEngine.Random.Range(1, 999);
        random = new System.Random(RandomSeed);

        bGameOver = false;

        //更新摄像机位置
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);

        firstGen = true;
    }
    public void GameEnd()
    {
        foreach(var brick in m_AllBricks)
        {
            //固定位置
            brick.m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            brick.m_Rigidbody.Sleep();
            //拉远镜头
            bGameOver = true;
        }
    }

    public void Pause()
    {
        foreach(var brick in m_AllBricks)
        {
            brick.Pause();
        }
    }

    public void Resume()
    {
        foreach(var brick in m_AllBricks)
        {
            brick.Resume();
        }
    }

    public Brick GenBrick()
    {
       // aaa = new System.Random(1);
        //播放生成音效
        AudioManager.instance.PlaySfx("Sfx_BrickSpawn");
        if(firstGen)
        {
            firstGen = false;
            NextBrickType = (BrickType)_Random(0, 7);
        }
        GameObject brickObj;
        switch(NextBrickType)
        {
            case BrickType.I:
                brickObj = GameObject.Instantiate(BrickIprefab) as GameObject;
                break;
            case BrickType.J:
                brickObj = GameObject.Instantiate(BrickJprefab) as GameObject;
                break;
            case BrickType.L:
                brickObj = GameObject.Instantiate(BrickLprefab) as GameObject;
                break;
            case BrickType.O:
                brickObj = GameObject.Instantiate(BrickOprefab) as GameObject;
                break;
            case BrickType.S:
                brickObj = GameObject.Instantiate(BrickSprefab) as GameObject;
                break;
            case BrickType.T:
                brickObj = GameObject.Instantiate(BrickTprefab) as GameObject;
                break;
            case BrickType.Z:
                brickObj = GameObject.Instantiate(BrickZprefab) as GameObject;
                break;
            default:
                brickObj = GameObject.Instantiate(BrickIprefab) as GameObject;
                break;
        }
        var RandomRotate = _Random(0, 4);
        brickObj.transform.parent = BrickParent;
        Brick brickCom = brickObj.GetComponent<Brick>();
        switch (RandomRotate)
        {
            case 0:
                brickObj.transform.rotation = Quaternion.Euler(0,0,0);
                brickCom.m_RotateState = Brick.RotateState.zero;
                break;
            case 1:
                brickObj.transform.rotation = Quaternion.Euler(0, 0, -90);
                brickCom.m_RotateState = Brick.RotateState.Ninety;
                break;
            case 2:
                brickObj.transform.rotation = Quaternion.Euler(0, 0, -180);
                brickCom.m_RotateState = Brick.RotateState.OneEightZero;
                break;
            case 3:
                brickObj.transform.rotation = Quaternion.Euler(0, 0, -270);
                brickCom.m_RotateState = Brick.RotateState.TwoSevenZero;
                break;
        }
        brickObj.transform.localPosition = BrickGenPoint;

        NextBrickType = (BrickType)_Random(0, 7);
        //更改图标
        if(OnChangeNextBrickView != null)
        {
            OnChangeNextBrickView((int)NextBrickType);
        }

        brickCom.CollisionEnterEvent += HandleBrickFall2Physics;
        brickCom.FalloutScreen += HandleBrickFalloutScreen;

        return brickCom;
    }

    private void HandleBrickFall2Physics()
    {
        //计算最高的方块
        float highest = CaculateHighestBrick();
        //更新生成方块的高度
        BrickGenPoint = new Vector3(0, highest + 50f, 0);
        //更新摄像机位置
        if (highest > -5f)
        {
            Camera.main.transform.DOMoveY(highest + 5f, 0.2f);
        }
        else
        {
            if (Camera.main.transform.localPosition.y > 10f)
            {
                Camera.main.transform.DOMoveY(highest + 5f, 0.2f);
            }
        }

        var brick = GenBrick();
        ControlBrick = brick;
        m_AllBricks.Add(brick);
        //摄像机抖动
        Camera.main.transform.DOShakePosition(0.2f, 2f);
    }

    private void HandleBrickFalloutScreen(Brick _brick)
    {
        if (OnBrickFallOut != null)
        {
            OnBrickFallOut();
        }

        if (bGameOver)
            return;

        //计算最高的方块
        float highest = CaculateHighestBrick();
        //更新生成方块的高度
        BrickGenPoint = new Vector3(0, highest + 50f, 0);
        //更新摄像机位置
        if (highest > -5f)
        {
            Camera.main.transform.DOMoveY(highest + 5f, 0.2f);
        }
        else
        {
            if (Camera.main.transform.localPosition.y > 10f)
            {
                Camera.main.transform.DOMoveY(highest + 5f, 0.2f);
            }
        }

        //处理方块掉出屏幕
        if (_brick == ControlBrick)
        {
            var brick = GenBrick();
            ControlBrick = brick;
            m_AllBricks.Add(brick);
        }

        m_AllBricks.Remove(_brick);
    }

    //生成随机数
    public int _Random(int min,int max)
    {
        return random.Next(min,max);
    }

    public float CaculateHighestBrick()
    {
        float Highest = -15f;
        foreach(var brick in m_AllBricks)
        {
            if(brick != ControlBrick)
            {
                if (brick.selfCollider.bounds.max.y > Highest)
                {
                    Highest = brick.selfCollider.bounds.max.y;
                }
            }
        }
        return Highest;
    }
}
