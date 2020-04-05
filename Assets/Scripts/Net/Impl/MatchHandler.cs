using CommunicationProtocol.Code;
using CommunicationProtocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MatchHandler : HandlerBase
    {
        public override void OnReceive(int subCode, object value)
        {
            switch (subCode)
            {
                case MatchCode.MATCH_ENTER_BROA:
                Debug.Log("执行MatchHandler.ReceiveNewPlayer");
                ReceiveNewPlayer(value as UserDto);
                    break;
                case MatchCode.MATCH_ENTER_SREP:
                ReceiveMatchDto(value as MatchDto);
                    break;
                case MatchCode.MATCH_EXIT_BROA:
                Debug.Log("执行MatchHandler.ReveieExit");
                ReceiveExit(value.ToString());
                    break;
            case MatchCode.MATCH_READY_BROA:
                ReceiveReadyPlayer(value.ToString());
                break;
            case MatchCode.MATCH_NOTREADY_BROA:
                Debug.Log("执行MatchHandler.ReceiveNotReadyPlayer");
                ReceiveNotReadyPlayer(value.ToString());
                break;
            case MatchCode.MATCH_START_BROA:
               // StartGame();
                break;
                default:
                    break;
            }
        }
    /// <summary>
    /// 处理服务器返回的匹配房间数据
    /// </summary>
    /// <param name="dto"></param>
    private void ReceiveMatchDto(MatchDto dto)
    {
        //根据房间数据在匹配面板显示房间内的玩家信息面板
        Debug.Log("执行ReceiveMatchDto");
        Dispatch(AreaCode.UI, MatchEvent.MATCH_SHOW_PANEL,true);
        foreach(var i in dto.accUserDict.Values)
        {
            Dispatch(AreaCode.UI, MatchEvent.MATCH_ADD_PLAYER, i);
        }
        foreach(var i in dto.ReadyUserList)
        {
            Dispatch(AreaCode.UI, MatchEvent.MATCH_READY_TRUE,i);
        }
    }
    /// <summary>
    /// 处理服务器广播的新进入匹配房间的玩家数据
    /// </summary>
    private void ReceiveNewPlayer(UserDto dto)
    {
        //根据玩家数据在匹配面板显示新加入的玩家的信息面板
        Dispatch(AreaCode.UI, MatchEvent.MATCH_ADD_PLAYER, dto);
    }
    /// <summary>
    /// 处理玩家离开
    /// </summary>
    private void ReceiveExit(string acc)
    {
        Debug.Log("执行MatchHandler.ReveieExit");
        Dispatch(AreaCode.UI, MatchEvent.MATCH_READY_FALSE, acc);
        Dispatch(AreaCode.UI, MatchEvent.MATCH_REMOVE_PLAYER, acc);
    }
    /// <summary>
    /// 处理玩家准备
    /// </summary>
    private void ReceiveReadyPlayer(string acc)
    {
        Dispatch(AreaCode.UI, MatchEvent.MATCH_READY_TRUE, acc);
    }
    /// <summary>
    /// 处理取消准备
    /// </summary>
    /// <param name="acc"></param>
    private void ReceiveNotReadyPlayer(string acc)
    {
        Dispatch(AreaCode.UI, MatchEvent.MATCH_READY_FALSE, acc);
    }
    /// <summary>
    /// 开始游戏
    /// </summary>
    private void StartGame()
    {
        Debug.Log("开始游戏");
        SceneMesg sm = new SceneMesg(2,()=> 
        {
            Debug.Log("游戏场景加载完毕");
        });
        Dispatch(AreaCode.SCENE, SceneEvent.SCENE_LOAD, sm);
        //TODO
        //加载完场景向服务端发一个消息告知服务端，服务端再发送房间数据,或者把开始游戏挪到Game里（不出问题就不弄了
    }

    }

