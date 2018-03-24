using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CloudMove : MonoBehaviour
{
    [System.Serializable]
    public struct CloudMoveStartAndEnd
    {
        CloudMoveStartAndEnd(float initPos,float start,float end)
        {
            InitPos = initPos;
            StartPos = start;
            EndPos = end;
        }
        public float InitPos;
        public float StartPos;
        public float EndPos;
    }
    public List<CloudMoveStartAndEnd> PosArray;
    public List<GameObject> objArray;
    public float TotalMoveTime = 100f;
    private void Awake()
    {

    }
    void Start ()
    {
        moveCloud(0,50f);
        moveCloud(1,50f);
        moveCloud(2, 25f);
        moveCloud(3, 25f);
	}
	
	void Update ()
    {
		
	}

    private void moveCloud(int index,float time)
    {
        objArray[index].transform.DOMoveX(PosArray[index].EndPos,
            time * (Mathf.Abs(PosArray[index].InitPos - PosArray[index].EndPos)) / 
            (Mathf.Abs(PosArray[index].EndPos - PosArray[index].StartPos)))
            .SetEase(Ease.Linear)
            .OnComplete(delegate { objArray[index].transform.localPosition 
                                    = new Vector3(PosArray[index].StartPos, 
                                                objArray[index].transform.localPosition.y,
                                                objArray[index].transform.localPosition.z);
                                    objArray[index].transform.DOMoveX(PosArray[index].EndPos, time)
                                    .SetLoops(-1)
                                    .SetEase(Ease.Linear); });
    }
}
