using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    public GameObject SinglePlayerBtn;
    public GameObject MultiPlayerBtn;
    public GameObject OptionBtn;
    
	void Start ()
    {

    }
	
	void Update ()
    {
		
	}

    public void LeaveMainMenuEffect()
    {

    }

    public void SinglePlayerButton_Click()
    {
        LitminSceneManager.instance.LoadScene("GameScene");
    }
}
