using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameSceneView : MonoBehaviour
{
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public void ChangeHeartCount(int num)
    {
        switch(num)
        {
            case 0:
                heart1.SetActive(false);
                heart2.SetActive(false);
                heart3.SetActive(false);
                break;
            case 1:
                heart1.SetActive(true);
                heart2.SetActive(false);
                heart3.SetActive(false);
                break;
            case 2:
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(false);
                break;
            case 3:
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(true);
                break;
            default:
                break;
        }
    }

    //游戏结束菜单
    public GameObject GameEndMenu;
    public GameObject GameEndMenuBlackTex;

    public void HandleGameEnd()
    {
        //弹出菜单

    }

    public void HandleGameStart()
    {
        //移出菜单
    }


    //游戏控制

    //左移
    public event Action OnMoveLeft;
    public void MoveLeftBtnClick()
    {
        if(OnMoveLeft != null)
        {
            OnMoveLeft();
        }
    }

    //右移
    public event Action OnMoveRight;
    public void MoveRightBtnClick()
    {
        if(OnMoveRight != null)
        {
            OnMoveRight();
        }
    }

    //加速
    public event Action OnSpeedUp;
    public void SpeedUpBtnClick()
    {
        if(OnSpeedUp != null)
        {
            OnSpeedUp();
        }
    }

    //减速
    public event Action OnSpeedDown;
    public void SpeedDownBtnClick()
    {
        if(OnSpeedDown != null)
        {
            OnSpeedDown();
        }
    }


    //旋转
    public event Action OnRotate;
    public void RotateBtnClick()
    {
        if(OnRotate != null)
        {
            OnRotate();
        }
    }

}
