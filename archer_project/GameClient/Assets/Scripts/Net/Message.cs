using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Message 
{
    private byte[] data = new byte[1024];
    private int startIndex = 0;//存储的字节数
    public byte[] Data
    {
        get
        {
            return data;
        }
    }
    //public void AddCount(int count)
    //{
    //    startIndex += count;
    //}
    public int RemainSize
    {
        get
        {
            return data.Length - startIndex;
        }
    }
    public int StartIndex { get => startIndex; set => startIndex = value; }
    public void ReadMessage(int Amount, Action<ActionCode, string> processDataCallack)
    {
        startIndex += Amount;
        while (true)
        {
            if (startIndex <= 4)
                return;
            int count = BitConverter.ToInt32(data, 0);
            if (startIndex - 4 >= count)
            {
                //string s = Encoding.UTF8.GetString(data, 4, count);
                //Console.WriteLine("解析数据:" + s);
                ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
                //ActionCode actioncode = (ActionCode)BitConverter.ToInt32(data, 8);
                string s = Encoding.UTF8.GetString(data, 8, count - 4);
                processDataCallack(actionCode,  s);
                Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                startIndex -= (count + 4);
            }
            else
            {
                break;
            }
        }
    }
    public static byte[] PackData(ActionCode actionCode, string data)
    {
        byte[] requestCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        int dataAmount = requestCodeBytes.Length + dataBytes.Length;
        byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
        dataAmountBytes.Concat(requestCodeBytes).Concat(dataBytes);
        return dataAmountBytes.ToArray<byte>();
    }
    public static byte[] PackData(RequestCode resquestCode, ActionCode actionCode, string data)
    {
        byte[] requestCodeBytes = BitConverter.GetBytes((int)resquestCode);
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        int dataAmount = requestCodeBytes.Length + dataBytes.Length + actionCodeBytes.Length;
        byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
        //byte[] newBytes = dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>();//Concat(dataBytes);
        //return newBytes.Concat(dataBytes).ToArray<byte>();
        return dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>()
            .Concat(actionCodeBytes).ToArray<byte>()
            .Concat(dataBytes).ToArray<byte>();
    }
}
