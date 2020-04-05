using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息中心
/// 只负责各模块间消息转发
/// </summary>
public class MessageCenter : MonoBase
{
    public static MessageCenter Instance = null;
    void Awake()
    {
        Instance = this;
        //添加一个模块就得添加一个脚本
        gameObject.AddComponent<TransformManager>();
        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<AudioManager>();
        gameObject.AddComponent<NetManager>();
        gameObject.AddComponent<SceneMgr>();
        gameObject.AddComponent<GameManager>();
        gameObject.AddComponent<AnimationManager>();
        gameObject.AddComponent<FightManager>();
        gameObject.AddComponent<EffectsManager>();
        gameObject.AddComponent<SkillManager>();
        DontDestroyOnLoad(this.gameObject);
    }
    /// <summary>
    /// 处理消息
    /// </summary>
    public override void Execute(int eventCode, object message)
    {

    }
    /// <summary>
    /// 派发消息
    /// </summary>
    /// <param name="areaCode">模块码</param>
    /// <param name="eventCode">事件码</param>
    /// <param name="message"> 传递参数</param>
    public void Dispatch(int areaCode,int eventCode,object message)
    {
        switch (areaCode)
        {
            case AreaCode.GAME:
                GameManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.UI:
                UIManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.NET:
                NetManager.Instance.Execute(eventCode,message);
                break;
            case AreaCode.AUDIO:
                AudioManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.EFFECT:
                EffectsManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.ANIMATION:
                AnimationManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.TRANSFORM:
                TransformManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.FIGHT:
                FightManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.SCENE:
                SceneMgr.Instance.Execute(eventCode, message);
                break;
            case AreaCode.SKILL:
                SkillManager.Instance.Execute(eventCode, message);
                break;
            default:break;
        }
    }



    public MessageCenter()
    {
        
    }
}
