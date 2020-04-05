using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
public class ClientSocket
{
    private Socket socket;
    private string IP;
    private int PORT;

    public ClientSocket(string ip, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IP = ip;
            PORT = port;
        
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public void Connect()
    {
        ShowToast.MakeToast("尝试连接到服务器");
        try
        {
            socket.Connect(IP, PORT);
            ShowToast.MakeToast("连接服务器成功");
            GameObject.Find("LoginButton").GetComponent<Login>().isConnected = true;
            StartReceive();
        }
        catch (Exception ex)
        {

           // Debug.LogError(ex.Message);
            ShowToast.MakeToast("无法接到服务器");
        }

    }

    public void DisConnect()
    {
        socket.Disconnect(false);
    }

    #region 接收数据
    private byte[] receiveBuffer = new byte[1024]; //接收数据的缓存区

    private List<byte> dataCache = new List<byte>(); //数据的缓存区

    public Queue<SocketMessage> SmgQueue = new Queue<SocketMessage>(); //存储的待处理的消息体的队列

    /// <summary>
    /// 是否正在处理数据
    /// </summary>
    private bool isReceiveProcess = false;

    /// <summary>
    /// 开始接收数据
    /// </summary>
    private void StartReceive()
    {
        if (socket == null && socket.Connected==false)
        {
            Debug.LogError("连接失败，无法接收数据");
            return;
        }
        socket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, socket);
        
    }
    /// <summary>
    /// 收到数据后回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        
        try
        {
            int length = socket.EndReceive(ar);
            byte[] tmpByteArray = new byte[length];
            Buffer.BlockCopy(receiveBuffer, 0, tmpByteArray, 0, length);
            dataCache.AddRange(tmpByteArray);
            if (!isReceiveProcess)
            {
                ProcessReceive();
            }
            StartReceive();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            //断线报错
            //TODO 返回标题界面
            ShowToast.MakeToast("与服务器连接断开");
        }
    }
    /// <summary>
    /// 处理收到的数据
    /// </summary>
    private void ProcessReceive()
    {
        isReceiveProcess = true;
        byte[] data = EncodeTool.DecodePackage(ref dataCache);
        if (data == null)
        {
            isReceiveProcess = false;
            return;
        }
        SocketMessage smg = EncodeTool.DecodeMessage(data);
        SmgQueue.Enqueue(smg); //保存等待处理
      //  Debug.Log("收到服务器消息："+smg.value);
        ProcessReceive(); //递归调用
    }
    #endregion


    #region 发送数据
    public void Send(int opCode,int subCode,object value)
    {
        SocketMessage smg = new SocketMessage(opCode, subCode, value);
        Send(smg);
    }

    public void Send(SocketMessage smg)
    {
        byte[] data = EncodeTool.EncodeMessage(smg);
        byte[] package = EncodeTool.EncodePackage(data);
        try
        {
            socket.Send(package);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    #endregion
}
