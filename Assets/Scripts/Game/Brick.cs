using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public enum BrickState
    {
        Falling = 0,
        Physics = 1
    }

    public BrickState m_BrickState;

    //下落

    //物理模拟

    private void Awake()
    {
        m_BrickState = BrickState.Falling;
    }
    private void Start ()
    {
		
	}
	
	private void Update ()
    {
		if(m_BrickState == BrickState.Falling)
        {

        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void MoveStep()
    {

    }

    private void Rotate()
    {

    }
}
