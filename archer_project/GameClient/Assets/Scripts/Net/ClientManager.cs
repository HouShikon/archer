using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ClientManager:BaseManager
{
    private const string IP = "127.0.0.1";
    private const int PORT = 6688;
    private Socket clientSocket;
    private Message message;
    public override void OnInit()
    {
        base.OnInit();
        message = new Message();
        clientSocket= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);
            Start();
        }
        catch(Exception e)
        {
            Debug.Log("无法连接服务器"+e);
        }
    }
    public ClientManager(GameFacade gameFacede) : base(gameFacede) { }

    private void Start()
    {
        clientSocket.BeginReceive(message.Data, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveCallBack, null);
    }
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false)
                return;
            int cont = clientSocket.EndReceive(ar);
            message.ReadMessage(cont, OnProcessDataCallBack);
            Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    private void OnProcessDataCallBack(ActionCode actionCode, string data)
    {
        facede.HandleResponse(actionCode, data);
    }
    public void SendRequest(RequestCode resquestCode,ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PackData(resquestCode, actionCode, data);
        clientSocket.Send(bytes);
    }
    public override void OnDestory()
    {
        base.OnDestory();
        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.Log("无法关闭服务器" + e);
        }
    }
}
