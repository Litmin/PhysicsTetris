using System;
using UnityEngine;

public abstract class AbstractPoolableBehavior : MonoBehaviour
{
    private void Awake()
    {
        this._Init();
    }

    protected virtual void _Init()
    {

    }

    public virtual void ReStart()
    {

    }

    public void Cleanup()
    {

    }

    public bool m_RemoveFromParent;
}
