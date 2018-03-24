using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraEffect : MonoBehaviour
{
    public Image fadeImage;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

    }
}
