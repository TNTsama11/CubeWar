using CommunicationProtocol.Code;
using CommunicationProtocol.Dto;
using System.Collections;
using System.Collections.Generic;
using CommunicationProtocol.Dto;
using UnityEngine;
/// <summary>
/// Transform 模块控制器
/// </summary>
public class MainTransformCtrl:TransformBase
{
    private CharacterController characterController;
    private TransformDto transformDto;
    private AnimationMesg animationMesg;
    private string localAcc;

    private Vector3 euler;
    private Vector3 move;
    private int speed = 6;
    private const int defaultSpeed = 6;

    void Awake()
    {
        Bind(TransformEvent.TRANS_MOVE,TransformEvent.TRANS_POS,TransformEvent.TRANS_SET_SPEED,TransformEvent.TRANS_RESET_SPEED); 
        characterController = GetComponent<CharacterController>();
        transformDto = new TransformDto();
        animationMesg = new AnimationMesg();
        localAcc = PlayerPrefs.GetString("ID");
    }


    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case TransformEvent.TRANS_MOVE:
                //移动方法
                Move((Vector2)message);
                break;
            case TransformEvent.TRANS_POS:              
                break;
            case TransformEvent.TRANS_SET_SPEED:
                SetSpeed((int)message);
                break;
            case TransformEvent.TRANS_RESET_SPEED:
                ResetSpeed();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 设置速度
    /// </summary>
    private void SetSpeed(int speed)
    {
        this.speed = speed;
        AnimationMesg animationMesg = new AnimationMesg(localAcc, "Speed", false, speed/9);
        Dispatch(AreaCode.ANIMATION, AnimationEvent.ANIMATION_SET_FLOAT, animationMesg);
    }
    private void ResetSpeed()
    {
        this.speed = defaultSpeed;
        AnimationMesg animationMesg = new AnimationMesg(localAcc, "Speed", false, 0);
        Dispatch(AreaCode.ANIMATION, AnimationEvent.ANIMATION_SET_FLOAT, animationMesg);
    }

    private void Move(Vector2 dir)
    {
        animationMesg.Change(localAcc, "Walk", true);
        Dispatch(AreaCode.ANIMATION, AnimationEvent.ANIMATION_SET_BOOL, animationMesg);
        float angle= Mathf.Atan2(dir.x,dir.y)*Mathf.Rad2Deg;
        this.euler.y = angle;
        transform.rotation = Quaternion.Euler(euler);

        float speedX = Mathf.Abs(dir.x);
        float speedY = Mathf.Abs(dir.y);
        float tempSpeed = Mathf.Sqrt(speedY * speedY + speedX * speedX);
        move.x = dir.x;
        move.y = 0;
        move.z = dir.y;
        characterController.SimpleMove(move* tempSpeed*speed);

        float[] pos = new float[3];
        float[] rota = new float[3];
        pos[0] = this.transform.position.x;
        pos[1] = this.transform.position.y;
        pos[2] = this.transform.position.z;
        Vector3 v3= this.transform.rotation.eulerAngles;
        rota[0] = v3.x;
        rota[1] = v3.y;
        rota[2] = v3.z;
        transformDto.Change(null,pos,rota);
        Dispatch(AreaCode.GAME, GameEvent.GAME_UPLOAD_TRANS, transformDto);
    }
}
