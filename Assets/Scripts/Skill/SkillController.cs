using CommunicationProtocol.Code;
using CommunicationProtocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : SkillBase
{
    private string localAcc;

    void Awake()
    {
        Bind(SkillEvents.SKILL_DO_SKILL,SkillEvents.SKILL_SYNC_SKILL,SkillEvents.SKILL_SYNC_STOP);
    }

    void Start()
    {
        localAcc = PlayerPrefs.GetString("ID");
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SkillEvents.SKILL_DO_SKILL:
                DoSkill((SkillType)message);
                break;
            case SkillEvents.SKILL_SYNC_SKILL:
                SyncSkill(message as SkillMesg);
                break;
            case SkillEvents.SKILL_SYNC_STOP:
                SyncSkillStop(message as SkillDto);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 释放技能并发送给服务器
    /// </summary>
    private void DoSkill(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.FullPower:
                FullPower();
                break;
            default:
                break;
        }

        SkillDto dto = new SkillDto(localAcc, (int)skillType);
        SocketMessage socketMessage = new SocketMessage(OpCode.GAME, GameCode.GAME_DO_SKILLS_CERQ, dto);
        Dispatch(AreaCode.NET, 0, socketMessage);
        Debug.Log("向服务器发送技能请求");
    }

    private void SyncSkill(SkillMesg skillMesg)
    {
        if (skillMesg == null)
        {
            return;
        }
        switch (skillMesg.Type)
        {
            case SkillType.FullPower:
                AnimationMesg mesg = new AnimationMesg(skillMesg.Account, "Speed", false, 1);
                Dispatch(AreaCode.ANIMATION, AnimationEvent.ANIMATION_SET_FLOAT, mesg);
                EffectMesg effectMesg = new EffectMesg(skillMesg.gameObject, EffectType.Skill_Fullpower);
                Dispatch(AreaCode.EFFECT, EffectsEvents.SHOW_EFFECTS, effectMesg);
                break;

            default:
                break;
        }
    }

    private void SyncSkillStop(SkillDto dto)
    {
        if (dto == null)
        {
            return;
        }
        switch ((SkillType)dto.SkillType)
        {
            case SkillType.FullPower:
                AnimationMesg mesg = new AnimationMesg(dto.Account, "Speed", false, 0);
                Dispatch(AreaCode.ANIMATION, AnimationEvent.ANIMATION_SET_FLOAT, mesg);
                break;
            default:
                break;
        }
    }


    private void FullPower()
    {
        Dispatch(AreaCode.TRANSFORM, TransformEvent.TRANS_SET_SPEED, 9);
        Dispatch(AreaCode.FIGHT, FightEvent.FIGHT_SET_FIREINTERVAL, 0.3f);
        EffectMesg effectMesg = new EffectMesg(GameObject.FindGameObjectWithTag("Player"), EffectType.Skill_Fullpower);
        Dispatch(AreaCode.EFFECT, EffectsEvents.SHOW_EFFECTS, effectMesg);
        StartCoroutine(ResetFullPower());
        Dispatch(AreaCode.GAME, GameEvent.GAME_REDUCE_HG, 50);
    }

    IEnumerator ResetFullPower()
    {
        int t = 10;
        while (true)
        {
            if (t <= 0)
            {
                Dispatch(AreaCode.TRANSFORM, TransformEvent.TRANS_RESET_SPEED, null);
                Dispatch(AreaCode.FIGHT, FightEvent.FIGHT_RESET_FIREINTERVAL, null);
                SkillDto dto = new SkillDto(localAcc, (int)SkillType.FullPower);
                SocketMessage socketMessage = new SocketMessage(OpCode.GAME, GameCode.GAME_STOP_SKILL_CERQ, dto);
                Dispatch(AreaCode.NET, 0, socketMessage);
                break;
            }

            t--;
            yield return new WaitForSeconds(1f);
        }
    }
}
