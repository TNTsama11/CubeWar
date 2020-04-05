using CommunicationProtocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class NetManager:ManagerBase
    {
    private static string localIp = "127.0.0.1";
    private static string serverIp = "47.94.8.47";
    private static string address = serverIp;
    [HideInInspector]
    public static NetManager Instance = null;
    //private ClientSocket client = new ClientSocket("47.94.8.47", 2333);
    private ClientSocket client = new ClientSocket(address, 2333);
    void Awake()
    {
        Instance = this;
        Add(0,this); 
    }

    void Start()
    {
        client.Connect();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case 0:
                client.Send(message as SocketMessage);
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (client==null)
        {
            return;
        }
        while (client.SmgQueue.Count > 0) //大于0说明有待处理的消息
        {
            SocketMessage smg = client.SmgQueue.Dequeue();
          //  Debug.Log("收到服务器消息:" + smg.value);
            ReceiveSocketMessage(smg);


        }
    }

    public string GetIP()
    {
        return address;
    }

    HandlerBase accountHandler = new AccountHandler();
    HandlerBase userHandler = new UserHandler();
    HandlerBase matchHandeler = new MatchHandler();
    HandlerBase gameHandler = new GameHandler();

    #region 处理服务器发来的消息
    /// <summary>
    /// 接收来自服务器的消息
    /// </summary>
    private void ReceiveSocketMessage(SocketMessage smg)
    {
        switch (smg.opCode)
        {           
            case OpCode.ACCOUNT:
                accountHandler.OnReceive(smg.subCode, smg.value);
                break;
            case OpCode.USER:
                userHandler.OnReceive(smg.subCode, smg.value);
                break;
            case OpCode.MATCH:
                matchHandeler.OnReceive(smg.subCode, smg.value);
                break;
            case OpCode.GAME:
                gameHandler.OnReceive(smg.subCode, smg.value);
                break;

            default: break;
        }
    }

    public void OffLine()
    {
        client.DisConnect();
    }


    #endregion

}

