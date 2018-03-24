using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    public static T instance
    {
        get
        {
            return _GetInstance();
        }
    }

    private static T _GetInstance()
    {
        if(_instance == null)
        {
            _instance = FindObjectOfType(typeof(T)) as T;
            if(_instance == null)
            {
                GameObject obj = new GameObject();
                //obj.hideFlags = HideFlags.HideAndDontSave;
                _instance = (T)obj.AddComponent(typeof(T));
                DontDestroyOnLoad(obj);
            }
        }
        return _instance;
    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected static T _instance;
}
