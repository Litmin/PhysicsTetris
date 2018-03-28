using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Brick : MonoBehaviour
{
    public enum BrickState
    {
        Falling = 0,
        Physics = 1
    }

    public enum RotateState
    {
        zero = 0,
        Ninety = 1,
        OneEightZero = 2,
        TwoSevenZero = 3
    }

    public event Action CollisionEnterEvent;

    public event Action<Brick> FalloutScreen;

    private BrickState m_BrickState;

    public RotateState m_RotateState;

    private Rigidbody2D m_Rigidbody;

    //移动的Trigger
    public List<BrickMoveTrigger> m_MoveTriggerZeroLeft;
    public List<BrickMoveTrigger> m_MoveTriggerZeroRight;

    public List<BrickMoveTrigger> m_MoveTriggerNinetyLeft;
    public List<BrickMoveTrigger> m_MoveTriggerNinetyRight;

    public List<BrickMoveTrigger> m_MoveTriggerOneEightZeroLeft;
    public List<BrickMoveTrigger> m_MoveTriggerOneEightZeroRight;

    public List<BrickMoveTrigger> m_MoveTriggerTwoSevenZeroLeft;
    public List<BrickMoveTrigger> m_MoveTriggerTwoSevenZeroRight;

    public BrickMoveTrigger m_DownTriggerZero;
    public BrickMoveTrigger m_DownTriggerNinety;
    public BrickMoveTrigger m_DownTriggerOneEightZero;
    public BrickMoveTrigger m_DownTriggerTwoSevenZero;

    //旋转的Trigger
    public BrickRotateTrigger m_RotateTrigger;

    private List<Collider2D> AllChildCollider;
    //下落
    private float m_FallSpeed;
    private float m_FallNomralSpeed;
    private float m_FallMaxSpeed;

    //物理模拟

    //暂停
    private bool m_Pause;

    //方块是否与下方的方块碰撞
    private bool CollisionWithDown = true;

    private void Awake()
    {
        m_BrickState = BrickState.Falling;

        m_Pause = false;

        m_FallNomralSpeed = 10f;
        m_FallMaxSpeed = 25f;
        m_FallSpeed = m_FallNomralSpeed;

        m_Rigidbody = GetComponent<Rigidbody2D>();

        AllChildCollider = new List<Collider2D>();

        //收集方块所有碰撞体让它们忽略对方
        for (int i = 0; i < transform.childCount; i++)
        {
            var collider = transform.GetChild(i).gameObject.GetComponent<CompositeCollider2D>();
            if (collider != null)
            {
                AllChildCollider.Add(collider);
            }
        }
        var selfCollider = GetComponent<Collider2D>();
        for (int i = 0; i < AllChildCollider.Count; i++)
        {
            Physics2D.IgnoreCollision(AllChildCollider[i], selfCollider);
            for (int j = i + 1; j < AllChildCollider.Count; j++)
            {
                Physics2D.IgnoreCollision(AllChildCollider[i], AllChildCollider[j]);
            }
        }
    }

    public void Init(RotateState rotateState)
    {
        m_RotateState = rotateState;
        switch(m_RotateState)
        {
            case RotateState.zero:
                break;
            case RotateState.Ninety:
                break;
            case RotateState.OneEightZero:
                break;
            case RotateState.TwoSevenZero:
                break;
            default:
                break;
        }
    }

    private void Start ()
    {
		
	}
	
	private void Update ()
    {
        //方块掉出屏幕
        if(transform.localPosition.y < -40)
        {
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            m_Rigidbody.Sleep();
            if(FalloutScreen != null)
            {
                FalloutScreen(this);
            }
            //播放方块落水音效
            int SfxIndex = UnityEngine.Random.Range(1, 5);
            string SfxString = "Sfx_BrickFallWater0" + SfxIndex.ToString();
            AudioManager.instance.PlaySfx(SfxString);
            Destroy(gameObject);
        }
	}
    private void FixedUpdate()
    {
        if (m_Pause)
        {
            return;
        }
        if (m_BrickState == BrickState.Falling)
        {
            m_Rigidbody.velocity = new Vector2(0, -m_FallSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_BrickState == BrickState.Falling)
        {
            Fall2Physics();
            //如果下方接触方块 向下施加力
            //switch (m_RotateState)
            //{
            //    case RotateState.zero:
            //        if(m_DownTriggerZero.OverLap)
            //        {
            //            //m_Rigidbody.AddForceAtPosition
            //        }
            //        break;
            //    case RotateState.Ninety:
            //        if(m_DownTriggerNinety.OverLap)
            //        {

            //        }
            //        break;
            //    case RotateState.OneEightZero:
            //        if(m_DownTriggerOneEightZero.OverLap)
            //        {

            //        }
            //        break;
            //    case RotateState.TwoSevenZero:
            //        if(m_DownTriggerTwoSevenZero.OverLap)
            //        {

            //        }
            //        break;
            //    default:
            //        break;
            //}
        }
    }

    private void Fall2Physics()
    {
        if (m_BrickState == BrickState.Falling)
        {
            m_BrickState = BrickState.Physics;
            //使用物理模拟
            if(CollisionEnterEvent != null)
            {
                CollisionEnterEvent();
            }
            //碰撞trigger不激活
            foreach (var collider in AllChildCollider)
            {
                collider.gameObject.SetActive(false);
            }
            m_Rigidbody.gravityScale = 2;

            if(CollisionWithDown)
            {
                //播放方块碰撞音效
                int SfxIndex = UnityEngine.Random.Range(1, 4);
                string SfxString = "Sfx_BrickLandSpring0" + SfxIndex.ToString();
                AudioManager.instance.PlaySfx(SfxString);
            }
        }
    }
    
    //移动
    public void Move(bool left)
    {
        if(left == true)
        {
            if(CheckCanMove(left))
            {
                transform.localPosition = new Vector3(transform.localPosition.x - 1,
                                                        transform.localPosition.y,
                                                        transform.localPosition.z);
                AudioManager.instance.PlaySfx("Sfx_MoveBrick");
            }
            else
            {
                AudioManager.instance.PlaySfx("Sfx_NudgeBrick");
                CollisionWithDown = false;
                Fall2Physics();
                //向左施加力
            }
        }
        else if(left != true)
        {
            if(CheckCanMove(left))
            {
                transform.localPosition = new Vector3(transform.localPosition.x + 1,
                                                        transform.localPosition.y,
                                                        transform.localPosition.z);
                AudioManager.instance.PlaySfx("Sfx_MoveBrick");
            }
            else
            {
                AudioManager.instance.PlaySfx("Sfx_NudgeBrick");
                CollisionWithDown = false;
                Fall2Physics();
                //向右施加力
            }
        }
    }
    //轻推
    public void Push(bool left)
    {

    }

    private bool CheckCanMove(bool left)
    {
        switch(m_RotateState)
        {
            case RotateState.zero:
                if(left)
                {
                    foreach(var moveTrigger in m_MoveTriggerZeroLeft)
                    {
                        if(moveTrigger.OverLap == true)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    foreach (var moveTrigger in m_MoveTriggerZeroRight)
                    {
                        if (moveTrigger.OverLap == true)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            case RotateState.Ninety:
                if (left)
                {
                    foreach (var moveTrigger in m_MoveTriggerNinetyLeft)
                    {
                        if (moveTrigger.OverLap == true)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    foreach (var moveTrigger in m_MoveTriggerNinetyRight)
                    {
                        if (moveTrigger.OverLap == true)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            case RotateState.OneEightZero:
                if (left)
                {
                    foreach (var moveTrigger in m_MoveTriggerOneEightZeroLeft)
                    {
                        if (moveTrigger.OverLap == true)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    foreach (var moveTrigger in m_MoveTriggerOneEightZeroRight)
                    {
                        if (moveTrigger.OverLap == true)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            case RotateState.TwoSevenZero:
                if (left)
                {
                    foreach (var moveTrigger in m_MoveTriggerTwoSevenZeroLeft)
                    {
                        if (moveTrigger.OverLap == true)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    foreach (var moveTrigger in m_MoveTriggerTwoSevenZeroRight)
                    {
                        if (moveTrigger.OverLap == true)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            default:
                return true;
        }
    }

    //旋转
    public void Rotate()
    {
        AudioManager.instance.PlaySfx("Sfx_Rotatebrick");
        if (CheckCanRotate())
        {
            transform.Rotate(new Vector3(0, 0, -90));
        }
        else
        {
            Fall2Physics();
            //施加一个旋转力
        }
    }

    private bool CheckCanRotate()
    {
        return !(m_RotateTrigger.OverLap);
    }

    //下落加速
    public void FallSpeedToMax()
    {
        StopCoroutine(FallSpeedToNormalCoroutine());
        StartCoroutine(FallSpeedToMaxCoroutine());
    }
    private IEnumerator FallSpeedToMaxCoroutine()
    {
        for(int i = 0;i < 60;i++)
        {
            m_FallSpeed = Mathf.Lerp(m_FallSpeed, m_FallMaxSpeed, i / 60);
            yield return new WaitForSeconds(0.01f);
        }
    }
    //下落恢复正常速度
    public void FallSpeedToNormal()
    {
        StopCoroutine(FallSpeedToMaxCoroutine());
        StartCoroutine(FallSpeedToNormalCoroutine());
    }
    private IEnumerator FallSpeedToNormalCoroutine()
    {
        for (int i = 0; i < 60; i++)
        {
            m_FallSpeed = Mathf.Lerp(m_FallSpeed, m_FallNomralSpeed, i / 60);
            yield return new WaitForSeconds(0.01f);
        }
    }

    //暂停
    public void Pause()
    {
        m_Pause = true;
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        m_Rigidbody.Sleep();
    }
    //恢复
    public void Resume()
    {
        m_Pause = false;
        m_Rigidbody.constraints = RigidbodyConstraints2D.None;
        m_Rigidbody.WakeUp();
    }
}
