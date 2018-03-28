using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    
	void Start ()
    {
        AudioManager.instance.StopAllMusic();
        AudioManager.instance.PlayMusic("Music_Menu");
    }

    //主菜单元素
    public Button SinglePlayerBtn;
    public Button MultiPlayerBtn;
    public Button OptionBtn;
    public Button InstructionBtn;

    //单人模式菜单元素
    public Button SurvivalModeBtn;
    public Button RacingModeBtn;
    public Button JigsawModeBtn;
    public Button SingleMenuToMainBtn;

    //主菜单移出效果
    private void MainMenuMoveOutEffect(float delay)
    {
        SinglePlayerBtn.GetComponent<RectTransform>().DOLocalMoveX(-800f, 0.7f).SetDelay(delay).SetEase(Ease.InBack);
        MultiPlayerBtn.GetComponent<RectTransform>().DOLocalMoveX(-800f, 0.7f).SetDelay(0.2f + delay).SetEase(Ease.InBack);
        OptionBtn.GetComponent<RectTransform>().DOLocalMoveX(-800f, 0.7f).SetDelay(0.4f + delay).SetEase(Ease.InBack);
        InstructionBtn.GetComponent<RectTransform>().DOLocalMoveX(-800f, 0.7f).SetDelay(0.6f + delay).SetEase(Ease.InBack);
    }
    //主菜单移回效果
    private void MainMenuMoveInEffect(float delay)
    {
        SinglePlayerBtn.GetComponent<RectTransform>().DOLocalMoveX(0, 0.7f).SetDelay(delay).SetEase(Ease.OutBack);
        MultiPlayerBtn.GetComponent<RectTransform>().DOLocalMoveX(0, 0.7f).SetDelay(0.2f + delay).SetEase(Ease.OutBack);
        OptionBtn.GetComponent<RectTransform>().DOLocalMoveX(0, 0.7f).SetDelay(0.4f + delay).SetEase(Ease.OutBack);
        InstructionBtn.GetComponent<RectTransform>().DOLocalMoveX(0, 0.7f).SetDelay(0.6f + delay).SetEase(Ease.OutBack);
    }
    //单人游戏菜单移入
    private void SinglePlayerMenuMoveInEffect(float delay)
    {
        SurvivalModeBtn.GetComponent<RectTransform>().DOLocalMoveX(0, 0.7f).SetDelay(delay).SetEase(Ease.OutBack);
        RacingModeBtn.GetComponent<RectTransform>().DOLocalMoveX(0, 0.7f).SetDelay(delay + 0.2f).SetEase(Ease.OutBack);
        JigsawModeBtn.GetComponent<RectTransform>().DOLocalMoveX(0, 0.7f).SetDelay(delay + 0.4f).SetEase(Ease.OutBack);
        SingleMenuToMainBtn.GetComponent<RectTransform>().DOLocalMoveX(0, 0.7f).SetDelay(delay + 0.6f).SetEase(Ease.OutBack);

    }
    //单人游戏菜单移出
    private void SinglePlayerMenuMoveOutEffect(float delay)
    {
        SurvivalModeBtn.GetComponent<RectTransform>().DOLocalMoveX(800, 0.7f).SetDelay(delay).SetEase(Ease.InBack);
        RacingModeBtn.GetComponent<RectTransform>().DOLocalMoveX(800, 0.7f).SetDelay(delay + 0.2f).SetEase(Ease.InBack);
        JigsawModeBtn.GetComponent<RectTransform>().DOLocalMoveX(800, 0.7f).SetDelay(delay + 0.4f).SetEase(Ease.InBack);
        SingleMenuToMainBtn.GetComponent<RectTransform>().DOLocalMoveX(800, 0.7f).SetDelay(delay + 0.6f).SetEase(Ease.InBack);
    }

    //单人游戏按钮
    public void SinglePlayerButton_Click()
    {
        PlayClickSfx();
        MainMenuMoveOutEffect(0);
        SinglePlayerMenuMoveInEffect(0.7f);
    }
    //多人游戏按钮
    public void MultiPlayerButton_Click()
    {
        PlayClickSfx();
    }

    //设置按钮
    public void OptionButton_Click()
    {
        PlayClickSfx();
    }

    //说明按钮
    public void InstructionButton_Click()
    {
        PlayClickSfx();
    }

    //生存模式按钮
    public void SurvivalModeButton_Click()
    {
        PlayClickSfx();
        LitminSceneManager.instance.LoadScene("GameSurvivalScene");
    }

    //拼图模式按钮
    public void JigsawModeButton_Click()
    {
        PlayClickSfx();
    }
    //竞速模式按钮
    public void RacingModeButton_Click()
    {
        PlayClickSfx();
    }

    //单人游戏选择菜单返回主菜单按钮
    public void SingleMenuToMainButton_Click()
    {
        PlayClickSfx();
        SinglePlayerMenuMoveOutEffect(0);
        MainMenuMoveInEffect(0.7f);
    }


    //播放点击音效
    private void PlayClickSfx()
    {
        int i = Random.Range(0, 4);
        switch(i)
        {
            case 0:
                AudioManager.instance.PlaySfx("Sfx_SelectItem01");
                break;
            case 1:
                AudioManager.instance.PlaySfx("Sfx_SelectItem02");
                break;
            case 2:
                AudioManager.instance.PlaySfx("Sfx_SelectItem03");
                break;
            case 3:
                AudioManager.instance.PlaySfx("Sfx_SelectItem04");
                break;
            default:
                AudioManager.instance.PlaySfx("Sfx_SelectItem01");
                break;
        }
    }

    //点击logo 抖动
    public Image logo;
    public void LogoClick()
    {
        PlayClickSfx();
        logo.rectTransform.DOShakePosition(0.5f,10);
    }
}
