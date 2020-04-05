using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommunicationProtocol.Code;
using CommunicationProtocol.Dto;
/// <summary>
/// Account 模块处理服务器发来数据的类
/// </summary>
public class AccountHandler : HandlerBase 
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccCode.ACC_LOGIN_SREP:               
                SceneMesg sMesg = new SceneMesg(1,()=> {
                    Debug.Log("场景加载成功");
                    UserDto userDto = value as UserDto;
                    if (userDto == null)
                    {
                        return;
                    }
                    Dispatch(AreaCode.UI, UIEvent.UI_CHANGE_ID, userDto.Account);
                    //TODO 接受返回的数据刷新视图
                    Dispatch(AreaCode.UI, UIEvent.UI_REFRESH, userDto);
                });
                Dispatch(AreaCode.SCENE, SceneEvent.SCENE_LOAD, sMesg);             
                break;
            case AccCode.ACC_LOGIN_NOACC:
                Dispatch(AreaCode.UI, UIEvent.UI_LOGIN_NOACC, null);
                break;
            case AccCode.ACC_CREATE_SREP:
                SceneMesg sceneMesg = new SceneMesg(1, () => {
                    UserDto userDto = value as UserDto;
                    if (userDto == null)
                    {
                        return;
                    }
                    if (userDto.Account == "0")
                    {
                        return;
                    }
                    ShowToast.MakeToast("创建成功");
                    Dispatch(AreaCode.UI, UIEvent.UI_CHANGE_ID, userDto.Account);
                    Dispatch(AreaCode.UI, UIEvent.UI_REFRESH, userDto);
                });
                Dispatch(AreaCode.SCENE, SceneEvent.SCENE_LOAD, sceneMesg);
                break;
            case AccCode.ACC_RELOAD_SREP:
                SceneMesg Mesg = new SceneMesg(1, () => {
                    Debug.Log("场景加载成功");
                    UserDto userDto = value as UserDto;
                    if (userDto == null)
                    {
                        return;
                    }
                    Dispatch(AreaCode.UI, UIEvent.UI_CHANGE_ID, userDto.Account);
                    //TODO 接受返回的数据刷新视图
                    Dispatch(AreaCode.UI, UIEvent.UI_REFRESH, userDto);
                });
                Dispatch(AreaCode.SCENE, SceneEvent.SCENE_LOAD, Mesg);
                break;
            default:
                break;
        }
    }

   
}
