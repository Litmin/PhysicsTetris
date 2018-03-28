using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class BrickRotateTrigger : MonoBehaviour
{
    private List<GameObject> selfObjs;

    private bool overLap = false;
    public bool OverLap
    {
        get
        {
            return overLap;
        }
        private set
        {
            overLap = value;
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
        OverLap = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        OverLap = false;
    }
}
