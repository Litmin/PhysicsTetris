using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class GameSceneView : MonoBehaviour
{
    //游戏背景
    public Canvas bgCanvas;
    private void Start()
    {
        //设置游戏背景canvas对象的camera
        bgCanvas.worldCamera = Camera.main;
    }
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    //改变生命值
    public void ChangeHeartCount(int num)
    {
        switch(3 - num)
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
    public Image[] NextBrickIcons;
    //修改下次方块的图标
    public void ChangeNextBrickIcon(int num)
    {
        foreach(var brickImage in NextBrickIcons)
        {
            brickImage.gameObject.SetActive(false);
        }
        NextBrickIcons[num].gameObject.SetActive(true);
    }

    //游戏结束菜单
    public GameObject GameEndMenu;
    public Image GameEndMenuBlackTex;

    public void HandleGameEnd()
    {
        //弹出菜单
        GameEndMenuBlackTex.DOFade(0.3f, 0.3f).SetDelay(1.0f);
        GameEndMenu.GetComponent<RectTransform>().DOLocalMoveY(250f, 0.7f).SetDelay(1.0f).SetEase(Ease.OutBack);
    }

    //分数变化
    public Text scoreText;
    public void HandleScoreChange(int score)
    {
        scoreText.text = score.ToString();
    }

    public void GameEndMenuMoveOut()
    {
        //移出菜单
        GameEndMenuBlackTex.DOFade(0, 0.3f);
        GameEndMenu.GetComponent<RectTransform>().DOLocalMoveY(-1400f, 0.7f).SetEase(Ease.OutBack);

        ChangeHeartCount(0);
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

    public event Action OnRestart;
    //游戏结束后的操作
    //重新开始
    public void RestartBtnClick()
    {
        GameEndMenuMoveOut();
        if (OnRestart != null)
        {
            OnRestart();
        }
    }

    //观看回放
    public void LookRecordClick()
    {

    }

    //保存回放
    public void SaveRecordClick()
    {

    }

    //返回主界面
    public void ReturnMainMenuClick()
    {
        LitminSceneManager.instance.LoadScene("MainMenuScene");
    }

}
