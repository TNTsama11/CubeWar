using CommunicationProtocol.Code;
using CommunicationProtocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 战斗模块控制器
/// </summary>
public class FightController : FightBase
{
    private string account;
    private Transform FirePoint;
    private float fireInterval = 0.6f;
    private float defaultFireInterval = 0.6f;
    public bool IsLocalPlayer = false;

    private FireType fireType=FireType.None;
    private Arms curArms;
    private BulletBase curBullet;
    private DamageMesg damageMesg;
    private EffectMesg effectMesg;
    private ShootDto shootDto;


	void Awake()
	{
        Bind(FightEvent.FIGHT_DO_ATTACK,FightEvent.FIGHT_SET_FIREINTERVAL,FightEvent.FIGHT_RESET_FIREINTERVAL,FightEvent.FIGHT_SYNC_ARMSTYPS);
        FirePoint = this.transform.Find("FirePoint");
        curBullet = new BulletBase();
        damageMesg = new DamageMesg();
        effectMesg = new EffectMesg();
        shootDto = new ShootDto();
	}
	
    void Start()
    {
        curArms = new Straight(FirePoint, account);
        fireType = FireType.Straight;
        if (IsLocalPlayer)
        {
             StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        SocketMessage socketMessage = new SocketMessage();
        shootDto.Change(account);
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            socketMessage.Change(OpCode.GAME, GameCode.GAME_DO_ATTACK_CERQ, shootDto);
            Dispatch(AreaCode.NET, 0, socketMessage);
            curArms.Fire();
        }
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case FightEvent.FIGHT_DO_ATTACK:
                DoAttack(message.ToString());
                break;
            case FightEvent.FIGHT_SET_FIREINTERVAL:
                SetFireInterval((float)message);
                break;
            case FightEvent.FIGHT_RESET_FIREINTERVAL:
                ResetFireInterval();
                break;
            case FightEvent.FIGHT_SYNC_ARMSTYPS:
                SyncArmsType(message as ArmsDto);
                break;
            default:
                break;
        }
    }

    public void SetAccount(string acc)
    {
        this.account = acc;
    }

    public string GetAccount()
    {
        return this.account;
    }


    private void CreateArm(FireType type)
    {
        switch (type)
        {
            case FireType.Straight:
                curArms = new Straight(FirePoint, account);
                break;
            case FireType.HighSpeed:
                curArms = new HighSpeed(FirePoint, account);
                break;
            case FireType.Shotgun:
                curArms = new Shotgun(FirePoint, account);
                break;
            case FireType.Flare:
                curArms = new Flare(FirePoint, account);
                break;
            default:
                break;
        }
        fireInterval = curArms.FireInterval;
        defaultFireInterval = curArms.FireInterval;
    }

    private void SyncArmsType(ArmsDto dto)
    {
        if (dto == null)
        {
            return;
        }
        if (dto.Account == this.account)
        {
            this.CreateArm((FireType)dto.Type);
        }
    }

    private void DoAttack(string acc)
    {
        if (acc != account)
        {
            return;
        }
        curArms.Fire();
    }
    /// <summary>
    /// 设置攻击间隔
    /// </summary>
    private void SetFireInterval(float time)
    {
        if (IsLocalPlayer)
        {
            fireInterval = time;
        }
    }
    private void ResetFireInterval()
    {
        if (IsLocalPlayer)
        {
            fireInterval = defaultFireInterval;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (IsLocalPlayer)
        {
            string tag = other.gameObject.tag;
            switch (tag)
            {
                case "Bullet":
                    curBullet = other.GetComponent<BulletBase>();
                    if (curBullet.Account == account)
                    {
                        return;
                    }
                    damageMesg.Change(curBullet.Account, curBullet.Damage);
                    Dispatch(AreaCode.GAME, GameEvent.GAME_REDUCE_HP, damageMesg);
                    break;
                case "Props":
                    PickUpProps(other.GetComponent<PropsBase>().PropsType, other.GetComponent<PropsBase>());
                    break;
                default:
                    break;
            }           
        }
        switch (other.gameObject.tag)
        {
            case "Bullet":
                AnimationMesg animationMesg = new AnimationMesg(account, "Damage", true);
                Dispatch(AreaCode.ANIMATION, AnimationEvent.ANIMATION_SET_BOOL, animationMesg);
                break;
            case "Props":
                ShowPropsEffects(other.gameObject);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 拾取道具
    /// </summary>
    private void PickUpProps(PropsType propsType,PropsBase props)
    {
        switch (propsType)
        {
            case PropsType.Cure:
                Cure cure = props as Cure;
                if (cure != null)
                {
                    Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_PICKUP_CURE, Camera.main.transform.position);
                    Dispatch(AreaCode.GAME, GameEvent.GAME_AUGMENT_HP, cure.Value); //给玩家加血
                    Dispatch(AreaCode.GAME, GameEvent.GAME_REMOVE_PROPS_SEND, cure.id); //移除道具
                }
                break;
            case PropsType.Food:
                Food food = props as Food;
                if (food != null)
                {
                    Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_PICKUP_FOOD, Camera.main.transform.position);
                    Dispatch(AreaCode.GAME, GameEvent.GAME_AUGMENT_HG, food.value);
                    Dispatch(AreaCode.GAME, GameEvent.GAME_REMOVE_PROPS_SEND, food.id);
                }
                break;
            case PropsType.Arms:
                Arm arm = props as Arm;
                if (arm != null)
                {
                    Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_PICKUP_ARM, Camera.main.transform.position);
                    PickUpArms(arm);
                }
                break;
            default:
                break;
        }
    }

    private void PickUpArms(Arm arm)
    {
        if (arm.fireType == fireType)
        {
            return;
        }
        fireType = arm.fireType;
        CreateArm(arm.fireType);
        ArmsDto armsDto = new ArmsDto(account,(int)arm.fireType);
        SocketMessage socketMessage = new SocketMessage(OpCode.GAME, GameCode.GAME_SYNC_ATTACKTYPE_CERQ, armsDto);
        Dispatch(AreaCode.NET, 0, socketMessage);
    }

    /// <summary>
    /// 播放对应的特效
    /// </summary>
    private void ShowPropsEffects(GameObject go)
    {
        PropsType type = go.GetComponent<PropsBase>().PropsType;
        switch (type)
        {
            case PropsType.Cure:
                effectMesg.Change(this.gameObject, EffectType.CureEffect);
                Dispatch(AreaCode.EFFECT, EffectsEvents.SHOW_EFFECTS, effectMesg);
                break;
            case PropsType.Food:
                effectMesg.Change(this.gameObject, EffectType.FoodEffect);
                Dispatch(AreaCode.EFFECT, EffectsEvents.SHOW_EFFECTS, effectMesg);
                break;
            case PropsType.Arms:
                effectMesg.Change(this.gameObject, EffectType.ArmsEffect);
                Dispatch(AreaCode.EFFECT, EffectsEvents.SHOW_EFFECTS, effectMesg);
                break;

            default:
                break;
        }
    }

    public void SetArms(int type)
    {
        CreateArm((FireType)type);
    }
}
