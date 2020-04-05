using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommunicationProtocol.Dto;
using UnityEngine.UI;
using CommunicationProtocol.Code;

public class GameController : GameBase
{

    /// <summary>
    /// 玩家Account与玩家信息对应的字典
    /// </summary>
    private Dictionary<string, UserDto> userDtoDict = new Dictionary<string, UserDto>();
    /// <summary>
    /// 玩家Account和玩家角色对应字典
    /// </summary>
    private Dictionary<string, GameObject> userGameObjDict = new Dictionary<string, GameObject>();
    /// <summary>
    /// 道具和道具ID对应的字典
    /// </summary>
    private Dictionary<int, GameObject> idPropsDict = new Dictionary<int, GameObject>();

    private int localHp;
    private int localHg;
    private int localKill;

    [SerializeField]
    private GameObject character;
    private string localAcc;
    private SocketMessage smg;
    private AnimationMesg animationMesg;
    private HpDto hpDto;
    private HgDto hgDto;
    private InfoDto infoDto;

    void Awake()
	{
        Bind(GameEvent.GAME_PLAYER_ADD,GameEvent.GAME_PLAYER_SPAWN,
            GameEvent.GAME_SYNC_TRANS,GameEvent.GAME_PLAYER_EXIT,
            GameEvent.GAME_UPLOAD_TRANS,GameEvent.GAME_SYNC_HP,
            GameEvent.GAME_SYNC_HG,GameEvent.GAME_SYNC_KILL,
            GameEvent.GAME_SYNC_INFO,GameEvent.GAME_REDUCE_HP,
            GameEvent.GAME_AUGMENT_HP,GameEvent.GAME_PLAYER_DEATH,
            GameEvent.GAME_CREAT_PROPS,GameEvent.GAME_REMOVE_PROPS,
            GameEvent.GAME_REMOVE_PROPS_SEND,GameEvent.GAME_DOSKILL,
            GameEvent.GAME_STOPSKILL,GameEvent.GAME_REDUCE_HG,
            GameEvent.GAME_AUGMENT_HG
            );

        localAcc = PlayerPrefs.GetString("ID");

        smg = new SocketMessage();
        animationMesg = new AnimationMesg();
        hpDto = new HpDto();
        hgDto = new HgDto();
        infoDto = new InfoDto();
    }

    void Update()
    {

    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case GameEvent.GAME_PLAYER_ADD:
                AddPlayer(message as UserDto);
                break;
            case GameEvent.GAME_PLAYER_SPAWN:
                SpawnPlayer(message as TransformDto);
                break;
            case GameEvent.GAME_SYNC_TRANS:
                SyncTrans(message as TransformDto);
                break;
            case GameEvent.GAME_PLAYER_EXIT:
                PlayerExit(message.ToString());
                break;
            case GameEvent.GAME_UPLOAD_TRANS:
                SendTrans(message as TransformDto);
                break;
            case GameEvent.GAME_SYNC_HP:
                SyncHp(message as HpDto);
                break;
            case GameEvent.GAME_SYNC_HG:
                SyncHg(message as HgDto);
                break;
            case GameEvent.GAME_SYNC_KILL:
                SyncKill(message as KillDto);
                break;
            case GameEvent.GAME_SYNC_INFO:
                SyncInfo(message as InfoDto);
                break;
            case GameEvent.GAME_REDUCE_HP:
                ReduceHp(message as DamageMesg);
                break;
            case GameEvent.GAME_AUGMENT_HP:
                AugmentHp((int)message);
                break;
            case GameEvent.GAME_AUGMENT_HG:
                AugmentHg((int)message);
                break;
            case GameEvent.GAME_REDUCE_HG:
                ReduceHg((int)message);
                break;
            case GameEvent.GAME_PLAYER_DEATH:
                PlayerDeath(message as DeathDto);
                break;
            case GameEvent.GAME_CREAT_PROPS:
                CreatProps(message as CreatPropsDto);
                break;
            case GameEvent.GAME_REMOVE_PROPS:
                RemoveProps((int)message,false);
                break;
            case GameEvent.GAME_REMOVE_PROPS_SEND:
                RemoveProps((int)message);
                break;
            case GameEvent.GAME_DOSKILL:
                SyncSkill(message as SkillDto);
                break;
            case GameEvent.GAME_STOPSKILL:
                SyncSkillStop(message as SkillDto);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 将玩家加入字典
    /// </summary>
    /// <param name="dto"></param>
    private void AddPlayer(UserDto dto)
    {
        userDtoDict.Add(dto.Account, dto);
    }
    /// <summary>
    /// 根据数据生成玩家角色（加入或者复活）
    /// </summary>
    /// <param name="dto"></param>
    private void SpawnPlayer(TransformDto dto)
    {        
        Vector3 pos = new Vector3(dto.pos[0], dto.pos[1], dto.pos[2]);
        Vector3 rota = new Vector3(dto.rota[0], dto.rota[1], dto.rota[2]);
        Quaternion qRota = Quaternion.Euler(rota);
        GameObject obj = Instantiate(character, pos, qRota);
        string acc = dto.Account;
        userGameObjDict.Add(acc, obj);
        UserDto userDto = userDtoDict[acc];
        GameObject model = Resources.Load("Model/Model" + userDto.ModelID) as GameObject;
        Instantiate(model, obj.transform);
        obj.gameObject.GetComponent<AnimationController>().SetAccount(acc);
        obj.gameObject.GetComponent<FightController>().SetAccount(acc);
        EffectMesg effectMesg = new EffectMesg(obj, EffectType.RespawnEffect);
        Dispatch(AreaCode.EFFECT, EffectsEvents.SHOW_EFFECTS, effectMesg);
        if (acc == localAcc)
        {
            obj.tag = "Player";
            //obj.AddComponent<CharacterController>();
            obj.AddComponent<MainTransformCtrl>();
            obj.gameObject.GetComponent<FightController>().IsLocalPlayer = true;
            obj.AddComponent<AudioListener>();
            obj.transform.Find("Canvas").gameObject.SetActive(false);
            Dispatch(AreaCode.UI, UIEvent.UI_SHOWHIDE_ETC, true);
            SocketMessage socketMessage = new SocketMessage(OpCode.GAME, GameCode.GAME_LOADCOMPLETE_CERQ, null);
            Dispatch(AreaCode.NET, 0, socketMessage);
            return;
        }
        obj.transform.Find("Canvas").transform.Find("NameText").GetComponent<Text>().text = userDto.Name;
        obj.transform.Find("Canvas").transform.Find("LVText").GetComponent<Text>().text ="LV."+userDto.Lv.ToString();
        Sprite sprite = Resources.Load("Icon/Icon" + userDto.IconID, typeof(Sprite)) as Sprite;
        obj.transform.Find("Canvas").transform.Find("ICON").GetComponent<Image>().sprite = sprite;
    }
    /// <summary>
    /// 处理传来的其他玩家的方位信息
    /// </summary>
    /// <param name="dto"></param>
    private void SyncTrans(TransformDto dto)
    {
        string acc = dto.Account;
        GameObject obj = userGameObjDict[acc];
        Vector3 pos = new Vector3(dto.pos[0], dto.pos[1], dto.pos[2]);
        Vector3 rota = new Vector3(dto.rota[0], dto.rota[1], dto.rota[2]);
        Quaternion qRota = Quaternion.Euler(rota);
        obj.transform.position = pos;
        //obj.transform.position = Vector3.Lerp(obj.transform.position, pos, 0.5f);
        obj.transform.rotation = qRota;
        //播放行走动画
        animationMesg.Change(acc, "Walk", true);
        Dispatch(AreaCode.ANIMATION, AnimationEvent.ANIMATION_SET_BOOL, animationMesg);
    }
    /// <summary>
    /// 同步血量
    /// </summary>
    private void SyncHp(HpDto dto)
    {
        string acc = dto.Account;
        if (acc == localAcc)
        {
            SetLocalHp(dto.Hp);
            return;
        }
        if (!userGameObjDict.ContainsKey(acc))
        {
            return;
        }
        GameObject obj = userGameObjDict[acc];
        Image img = obj.transform.Find("Canvas").transform.Find("HPBar").GetComponent<Image>();
        float fill = (float)dto.Hp / 100;
        img.fillAmount = fill;
        if (fill<0.9)
        {
            img.color = new Color(1 - fill, fill, 0);
        }
        else
        {
            img.color = Color.green;
        }
    }
    private void SetLocalHp(int hp)
    {
        localHp = hp;
        Dispatch(AreaCode.UI, UIEvent.UI_SET_GAME_HP, localHp);
    }
    /// <summary>
    /// 同步饥饿值
    /// </summary>
    private void SyncHg(HgDto dto)
    {
        string acc = dto.Account;
        if (acc == localAcc)
        {
            SetLocalHg(dto.Hg);
            return;
        }
        GameObject obj = userGameObjDict[acc];
        Image img = obj.transform.Find("Canvas").transform.Find("HGBar").GetComponent<Image>();
        img.fillAmount = (float)dto.Hg / 200;
    }
    private void SetLocalHg(int hg)
    {
        localHg = hg;
        Dispatch(AreaCode.UI, UIEvent.UI_SET_GAME_HG, localHg);
    }
    /// <summary>
    /// 同步击杀数目
    /// </summary>
    private void SyncKill(KillDto dto)
    {
        string acc = dto.Account;
        if (acc == localAcc)
        {
            SetLocalKill(dto.Kill);
        }
        GameObject obj = userGameObjDict[acc];
        obj.transform.Find("Canvas").transform.Find("KillText").GetComponent<Text>().text = dto.Kill.ToString();
    }
    private void SetLocalKill(int kill)
    {
        localKill = kill;
        Dispatch(AreaCode.UI, UIEvent.UI_SET_GAME_KILL, localKill);
    }
    /// <summary>
    /// 同步消息
    /// </summary>
    private void SyncInfo(InfoDto dto)
    {
        string msg = dto.message;
        if (dto.Account != string.Empty)
        {
            UserDto tempUserDto = userDtoDict[dto.Account];
            msg = tempUserDto.Name + ":" + msg;            
        }
        Dispatch(AreaCode.UI, UIEvent.UI_SET_GAME_INFO, msg);
    }

    /// <summary>
    /// 向服务器发送方位信息
    /// </summary>
    private void SendTrans(TransformDto dto)
    {
        dto.Account = localAcc;
        smg.Change(OpCode.GAME, GameCode.GAME_SYNC_TRASNFORM_CERQ, dto);
        Dispatch(AreaCode.NET, 0, smg);
    }
    /// <summary>
    /// 减少本地玩家的hp并发送给服务器
    /// </summary>
    private void ReduceHp(DamageMesg damageMesg)
    {
        int tempHp = localHp + damageMesg.Damage;
        tempHp = Mathf.Clamp(tempHp, 0, 100);
        SetLocalHp(tempHp);
        hpDto.Change(localAcc, localHp);
        smg.Change(OpCode.GAME, GameCode.GAME_SYNC_STATE_HP_CERQ,hpDto);
        Dispatch(AreaCode.NET, 0, smg);
        if (localHp == 0)
        {
            LocalPlayerDeath(damageMesg);
        }
    }
    /// <summary>
    /// 增加本地玩家HP并发送给服务器
    /// </summary>
    private void AugmentHp(int value)
    {
        int tempHp = localHp + value;
        tempHp = Mathf.Clamp(tempHp, 0, 100);
        SetLocalHp(tempHp);
        hpDto.Change(localAcc, localHp);
        smg.Change(OpCode.GAME, GameCode.GAME_SYNC_STATE_HP_CERQ, hpDto);
        Dispatch(AreaCode.NET, 0, smg);
    }
    /// <summary>
    /// 减少本地玩家hg并发给服务器
    /// </summary>
    private void ReduceHg(int value)
    {
        int tempHg = localHg - value;
        tempHg = Mathf.Clamp(tempHg, 0, 200);
        SetLocalHg(tempHg);
        hgDto.Change(localAcc, localHg);
        smg.Change(OpCode.GAME, GameCode.GAME_SYNC_STATE_HG_CERQ, hgDto);
        Dispatch(AreaCode.NET, 0, smg);
    }
    /// <summary>
    /// 增加本地玩家hg并发给服务器
    /// </summary>
    private void AugmentHg(int value)
    {
        int tempHg = localHg + value;
        tempHg = Mathf.Clamp(tempHg, 0, 200);
        SetLocalHg(tempHg);
        hgDto.Change(localAcc, localHg);
        smg.Change(OpCode.GAME, GameCode.GAME_SYNC_STATE_HG_CERQ, hgDto);
        Dispatch(AreaCode.NET, 0, smg);
    }
    /// <summary>
    /// 同步玩家释放技能
    /// </summary>
    private void SyncSkill(SkillDto dto)
    {
        if (dto == null)
        {
            return;
        }
        SkillMesg skillMesg = new SkillMesg(dto.Account, (SkillType)dto.SkillType, userGameObjDict[dto.Account]);
        Dispatch(AreaCode.SKILL, SkillEvents.SKILL_SYNC_SKILL, skillMesg);
    }

    private void SyncSkillStop(SkillDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.SKILL, SkillEvents.SKILL_SYNC_STOP, dto);
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    private void PlayerDeath(DeathDto deathDto)
    {
        string msg = "<color=##ffa502>" + userDtoDict[deathDto.KillerAccount].Name + "</color>" + deathDto.Reason + "<color=##ffa502>" + userDtoDict[deathDto.VictimAccount].Name + "</color>\n";
        Dispatch(AreaCode.UI, UIEvent.UI_SET_GAME_INFO, msg);
        DestroyPlayer(deathDto.VictimAccount);
    }
    /// <summary>
    /// 本地玩家死亡
    /// </summary>
    private void LocalPlayerDeath(DamageMesg mesg)
    {
        Debug.Log("AWSL");
        DestroyPlayer(localAcc);
        DeathDto deathDto = new DeathDto(mesg.Account,localAcc,"戳死了");//TODO 枚举定义原因
        smg.Change(OpCode.GAME, GameCode.GAME_DEATH_CERQ, deathDto);
        Dispatch(AreaCode.NET, 0, smg);
        string msg = "<color=#ff4757>" + userDtoDict[deathDto.KillerAccount].Name+ "</color>" + deathDto.Reason + "<color=##ffa502>" + userDtoDict[deathDto.VictimAccount].Name+ "</color>\n";
        Dispatch(AreaCode.UI, UIEvent.UI_SET_GAME_INFO,msg);
        Dispatch(AreaCode.UI, UIEvent.UI_SHOWHIDE_ETC, false);
    }
    /// <summary>
    /// 销毁玩家角色
    /// </summary>
    private void DestroyPlayer(string acc)
    {
        GameObject obj = userGameObjDict[acc];
        userGameObjDict.Remove(acc);
        EffectMesg effectMesg = new EffectMesg(obj,EffectType.DeathEffect);
        Dispatch(AreaCode.EFFECT, EffectsEvents.SHOW_EFFECTS, effectMesg);
        Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_DEATH_AUDIO, obj.transform.position);
        Destroy(obj);
    }

    /// <summary>
    /// 处理其他玩家退出
    /// </summary>
    /// <param name="acc"></param>
    private void PlayerExit(string acc)
    {
        userDtoDict.Remove(acc);
        GameObject obj = userGameObjDict[acc];
        userGameObjDict.Remove(acc);
        Destroy(obj);
    }
    /// <summary>
    /// 生成道具
    /// </summary>
    private void CreatProps(CreatPropsDto dto)
    {
        if (dto == null)
        {
            return;
        }
        RemoveAllProps();
        foreach(var item in dto.idPropsTypeDict)
        {
            GameObject go = ExcuteCreat(item.Value);
            idPropsDict.Add(item.Key, go);
        }
    }
    /// <summary>
    /// 创建道具
    /// </summary>
    private GameObject ExcuteCreat(PropsDto dto)
    {
        PropsType type;
        type = (PropsType)dto.type;
        Vector3 pos = new Vector3(dto.posX, dto.posY, dto.posZ);
        GameObject go=null;
        switch (type)
        {
            case PropsType.Cure:
                 go = Resources.Load("Props/Cure") as GameObject;
                 go = Instantiate(go, pos, Quaternion.identity);
                break;
            case PropsType.Food:
                go = Resources.Load("Props/Food") as GameObject;
                go = Instantiate(go, pos, Quaternion.identity);
                break;
            case PropsType.Arms:
                go= Resources.Load("Props/Arms/"+((FireType)dto.subType).ToString()) as GameObject;
                go = Instantiate(go, pos, Quaternion.identity);
                break;
            default:
                break;
        }
        return go;
    }
    /// <summary>
    /// 删除所有道具
    /// </summary>
    private void RemoveAllProps()
    {
        if (idPropsDict.Count > 0)
        {
            foreach(var item in idPropsDict)
            {
                Destroy(item.Value);
            }
            idPropsDict.Clear();
        }
    }
    /// <summary>
    /// 从字典中移除道具
    /// </summary>
    private void RemoveProps(int id,bool isSend=true)
    {
        if (idPropsDict.ContainsKey(id))
        {
            GameObject go = idPropsDict[id];
            if (go != null)
            {
                Destroy(go);
            }
            idPropsDict.Remove(id);
        }
        if (isSend)
        {
            //向服务器发送某个道具被删除的信息
            smg.Change(OpCode.GAME, GameCode.GAME_REMOVE_PROPS_CERQ, id);
            Dispatch(AreaCode.NET, 0, smg);
        }
    }

    /// <summary>
    /// 退出游戏房间时(包括关闭游戏
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        //存储玩家的击杀数
        PlayerPrefs.SetInt("Kill", PlayerPrefs.GetInt("Kill") + localKill);
    }



}
