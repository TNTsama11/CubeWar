using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunicationProtocol.Code;
using CommunicationProtocol.Dto;
using UnityEngine;


public class GameHandler : HandlerBase
{
    private SocketMessage smg = new SocketMessage();

    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case GameCode.GAME_ENTER_SREP:
                Enter(value as GameRoomDto);
                break;
            case GameCode.GAME_ENTER_BROA:
                ReceiveNewPlayer(value as UserDto);
                break;
            case GameCode.GAME_EXIT_BROA:
                ReceiveExit(value.ToString());
                break;
            case GameCode.GAME_SYNC_TRANSFORM_BROA:
                ReceiveSyncTrans(value as TransformDto);
                break;
            case GameCode.GAME_START_BROA:
                StartGame(value as GameRoomDto);
                break;
            case GameCode.GAME_SPAWN_SREP:
                ReceivePlayerReSpawn(value as TransformDto);
                break;
            case GameCode.GAME_SPAWN_BROA:
                ReceivePlayerReSpawn(value as TransformDto);
                break;
            case GameCode.GAME_SYNC_STATE_HP_BROA:
                ReceiveSyncHp(value as HpDto);
                break;
            case GameCode.GAME_SYNC_STATE_HG_BROA:
                ReceiveSyncHg(value as HgDto);
                break;
            case GameCode.GAME_SYNC_STATE_KILL_BROA:
                ReceiveSyncKill(value as KillDto);
                break;
            case GameCode.GAME_SYNC_INFO_BROA:
                ReceiveInfo(value as InfoDto);
                break;
            case GameCode.GAME_DEATH_BROA:
                ReceivePlayerDeath(value as DeathDto);
                break;
            case GameCode.GAME_RESPAWN_COUNTDOWN:
                ReceiveCountDown((int)value);
                break;
            case GameCode.GAME_REMOVE_PROPS_BROA:
                ReceiveRemoveProps((int)value);
                break;
            case GameCode.GAME_CREAT_PROPS_BROA:
                ReceiveCreatProps(value as CreatPropsDto);
                break;
            case GameCode.GAME_CREAT_PROPS_SREP:
                ReceiveCreatProps(value as CreatPropsDto);
                break;
            case GameCode.GAME_DO_ATTACK_BROA:
                ReceivePlayerAttack(value as ShootDto);
                break;
            case GameCode.GAME_DO_SKILLS_BROA:
                ReceiveDoSkill(value as SkillDto);
                break;
            case GameCode.GAME_STOP_SKILL_BROA:
                ReceiveStopSkill(value as SkillDto);
                break;
            case GameCode.GAME_SYNC_ATTACKTYPE_BROA:
                ReceiveSyncArmsType(value as ArmsDto);
                break;
            case GameCode.GAME_SYNC_ATTACKTYPE_SREP:
                ReceiveArmsType(value as ArmsDto);
                break;
            default:
                break;
        }
    }

    private void ReceiveRemoveProps(int id)
    {
        Dispatch(AreaCode.GAME, GameEvent.GAME_REMOVE_PROPS, id);
    }

    private void ReceiveCreatProps(CreatPropsDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.GAME, GameEvent.GAME_CREAT_PROPS,dto);
    }

    private void Enter(GameRoomDto dto)
    {       
        if (dto == null)
        {
            smg.Change(OpCode.MATCH, MatchCode.MATCH_ENTER_CREQ, null);
            Dispatch(AreaCode.NET, 0, smg);
            return;
        }
        SceneMesg sm = new SceneMesg(2, () =>
          {
              foreach (var item in dto.UserAccDtoDict.Keys)
              {
                  Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_ADD, dto.UserAccDtoDict[item]);
                  Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_SPAWN, dto.UserTransDto[item]);
                  Dispatch(AreaCode.GAME, GameEvent.GAME_SYNC_HP, dto.UserHpDict[item]);
                  Dispatch(AreaCode.GAME, GameEvent.GAME_SYNC_HG, dto.UserHgDict[item]);
                  Dispatch(AreaCode.GAME, GameEvent.GAME_SYNC_KILL, dto.UserKillDict[item]);
                  //ArmsDto armsDto = new ArmsDto(item, dto.UserArmsDict[item]);
                  //Dispatch(AreaCode.FIGHT, FightEvent.FIGHT_SYNC_ARMSTYPS, armsDto);

              }
          });
        
        Dispatch(AreaCode.SCENE, SceneEvent.SCENE_LOAD, sm);
    }

    

    private void ReceiveNewPlayer(UserDto dto)
    {
        if (dto == null)
        {
            return;
        }
        //将新玩家存到游戏控制器的字典里
        Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_ADD, dto);
        TransformDto transformDto = new TransformDto();
        transformDto.Account = dto.Account;
        Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_SPAWN, transformDto);
    }   

    private void ReceivePlayerReSpawn(TransformDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_SPAWN, dto);
    }

    private void ReceiveSyncTrans(TransformDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.GAME, GameEvent.GAME_SYNC_TRANS, dto);
    }

    private void ReceiveSyncHp(HpDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.GAME, GameEvent.GAME_SYNC_HP, dto);
    }

    private void ReceiveSyncHg(HgDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.GAME,GameEvent.GAME_SYNC_HG,dto);
    }

    private void ReceiveSyncKill(KillDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.GAME, GameEvent.GAME_SYNC_KILL, dto);
    }

    private void ReceiveInfo(InfoDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.GAME, GameEvent.GAME_SYNC_INFO, dto);
    }

    private void ReceivePlayerDeath(DeathDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_DEATH, dto);
    }

    private void ReceivePlayerAttack(ShootDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.FIGHT,FightEvent.FIGHT_DO_ATTACK, dto.Account);
    }

    private void ReceiveDoSkill(SkillDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.GAME, GameEvent.GAME_DOSKILL, dto);
    }

    private void ReceiveStopSkill(SkillDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.GAME, GameEvent.GAME_STOPSKILL, dto);
    }

    private void ReceiveArmsType(ArmsDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.FIGHT, FightEvent.FIGHT_SYNC_ARMSTYPS, dto);
    }

    private void ReceiveSyncArmsType(ArmsDto dto)
    {
        if (dto == null)
        {
            return;
        }
        Dispatch(AreaCode.FIGHT, FightEvent.FIGHT_SYNC_ARMSTYPS, dto);
    }

    private void ReceiveCountDown(int count)
    {
        Dispatch(AreaCode.UI, UIEvent.UI_SET_COUNTDOWN, count);
    }

    private void ReceiveExit(string acc)
    {
        Debug.Log("有玩家离开了");
        Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_EXIT, acc);
        
    }
    
    private void StartGame(GameRoomDto dto)
    {
        Debug.Log("开始游戏");
        SceneMesg sm = new SceneMesg(2, () =>
        {
            Debug.Log("游戏场景加载完毕");
            //将传来的房间数据里的玩家保存并根据位置信息生成对应角色
            //foreach (var item in dto.UserAccDtoDict.Values)
            //{
            //    Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_ADD, item);
            //}
            //foreach (var item in dto.UserTransDto.Values)
            //{
            //    Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_SPAWN, item);
            //}
            foreach(var item in dto.UserAccDtoDict.Keys)
            {
                Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_ADD, dto.UserAccDtoDict[item]);
                Dispatch(AreaCode.GAME, GameEvent.GAME_PLAYER_SPAWN, dto.UserTransDto[item]);
                Dispatch(AreaCode.GAME, GameEvent.GAME_SYNC_HP, dto.UserHpDict[item]);
                Dispatch(AreaCode.GAME, GameEvent.GAME_SYNC_HG, dto.UserHgDict[item]);
                Dispatch(AreaCode.GAME, GameEvent.GAME_SYNC_KILL, dto.UserKillDict[item]);
            }

        });
        Dispatch(AreaCode.SCENE, SceneEvent.SCENE_LOAD, sm);      
    }
}

