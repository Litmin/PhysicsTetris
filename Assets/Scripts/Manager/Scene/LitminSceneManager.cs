using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LitminSceneManager : MonoBehaviourSingleton<LitminSceneManager>
{
    private Image cameraFadeSprite;
    protected override void Awake()
    {
        base.Awake();
        cameraFadeSprite = Camera.main.gameObject.GetComponent<CameraEffect>().fadeImage;
    }
    private void Start()
    {
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(loadSceneEffect(sceneName));
    }

    IEnumerator loadSceneEffect(string sceneName)
    {
        if (cameraFadeSprite != null)
        {
            cameraFadeSprite.gameObject.SetActive(true);
            cameraFadeSprite.DOColor(new Color(0, 0, 0, 1), 0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        if (cameraFadeSprite != null)
        {
            cameraFadeSprite.DOColor(new Color(0, 0, 0, 0), 0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        cameraFadeSprite.gameObject.SetActive(false);
    }
}
