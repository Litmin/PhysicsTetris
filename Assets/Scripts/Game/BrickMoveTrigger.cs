using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMoveTrigger : MonoBehaviour
{
    private bool m_OverLap;
    public bool OverLap
    {
        get
        {
            return m_OverLap;
        }
        private set
        {
            m_OverLap = value;
        }
    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
