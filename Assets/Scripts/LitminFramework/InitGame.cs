using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
    private void Start()
    {
        GameRoot.instance.Init();

        SceneManager.LoadScene("MainMenuScene");
    }



}
