using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : AnimationBase
{

    private string account;
    private Animator animator;
    private bool isWalking=false;
    private float walkTimer;

	void Awake()
	{
        Bind(AnimationEvent.ANIMATION_SET_BOOL,AnimationEvent.ANIMATION_SET_FLOAT);
        
    }
	
    void Start()
    {
        animator = this.gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        StopWalk();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AnimationEvent.ANIMATION_SET_BOOL:
                SetBool(message as AnimationMesg);
                break;
            case AnimationEvent.ANIMATION_SET_FLOAT:
                SetFloat(message as AnimationMesg);
                break;
            default:
                break;
        }
    }

    public void SetAccount(string acc)
    {
        this.account = acc;
    }

    private void SetBool(AnimationMesg mesg)
    {
        //Debug.Log("执行AC.SB");
        if (mesg.Account != this.account)
        {
            return;
        }
        if (animator == null)
        {
            return;
        }
        animator.SetBool(mesg.AnimationName, mesg.BoolValue);
        //重置停止Walk的计时器
        if (mesg.AnimationName == "Walk")
        {
            isWalking = mesg.BoolValue;
            walkTimer = 0f;
        }
    }

    private void SetFloat(AnimationMesg mesg)
    {
        if (mesg.Account != this.account)
        {
            return;
        }
        if (animator == null)
        {
            return;
        }
        animator.SetFloat(mesg.AnimationName, mesg.FloatValue);
    }
    /// <summary>
    /// 停止walk动画
    /// </summary>
    private void StopWalk()
    {
        if (isWalking)
        {
            if (walkTimer < 0.1f)
            {
                walkTimer += Time.deltaTime;
            }
            else
            {
                animator.SetBool("Walk", false);
                walkTimer = 0f;
                isWalking = false;
            }
        }
    }
}
