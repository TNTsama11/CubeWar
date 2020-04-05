using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommunicationProtocol.Code;

public class UserHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case UserCode.USER_ONLIEN_SREP:
                OnlineRes((int)value);
                break;
            case UserCode.USER_GETUSERINFO_SREP:

                break;
            case UserCode.USER_UPLOADINFO_SREP:
                UpLoadInfoRes((int)value);
                break;
            default:
                break;
        }
    }

    private void UpLoadInfoRes(int value)
    {
        if (value == 0)
        {
            Debug.Log("本地玩家信息上传成功");
            SocketMessage smg = new SocketMessage(OpCode.USER, UserCode.USER_ONLINE_CREQ,null); 
            Dispatch(AreaCode.NET, 0, smg);
        }

    }

    private void OnlineRes(int value)
    {
        if (value == 0)
        {
            Debug.Log("玩家角色上线成功");
            Dispatch(AreaCode.UI, UIEvent.UI_START_MATCH, null);
        }
    }
}

