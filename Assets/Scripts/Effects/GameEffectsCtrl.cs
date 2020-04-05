using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffectsCtrl : EffectsBase
{

    public GameObject CureEffect;
    public GameObject FoodEffect;
    public GameObject ArmsEffect;
    public GameObject FullPowerEffect;
    public GameObject DeathEffect;
    public GameObject RespawnEffect;

    private void Awake()
    {
        Bind(EffectsEvents.SHOW_EFFECTS);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case EffectsEvents.SHOW_EFFECTS:
                ShowEffects(message as EffectMesg);
                break;
            default:
                break;
        }
    }

    private void ShowEffects(EffectMesg mesg)
    {
        if (mesg == null)
        {
            return;
        }
        EffectType type = mesg.Effect;
        GameObject go = mesg.Parent;
        switch (type)
        {
            case EffectType.CureEffect:               
                Instantiate(CureEffect, go.transform);
                break;
            case EffectType.FoodEffect:
                Instantiate(FoodEffect, go.transform);
                break;
            case EffectType.ArmsEffect:
                Instantiate(ArmsEffect,go.transform);
                break;
            case EffectType.Skill_Fullpower:
                Instantiate(FullPowerEffect, go.transform);
                break;
            case EffectType.DeathEffect:
                Instantiate(DeathEffect, go.transform.position,Quaternion.identity);
                break;
            case EffectType.RespawnEffect:
                GameObject obj= Instantiate(RespawnEffect, go.transform.position, Quaternion.identity);
                obj.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                break;
            default:
                break;
        }
    }
}


