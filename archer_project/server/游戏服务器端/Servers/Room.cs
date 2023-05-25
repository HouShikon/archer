using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Threading;

namespace GameServer.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }
    class Room
    {
        public  int MAX_HP = 200;//玩家最大HP
        private List<Client> clientRoom = new List<Client>();//用户列表
        private RoomState state = RoomState.WaitingJoin;//房间状态信息初始化为等待加入
        private Server server;

        public Room(Server server)
        {
            this.server = server;
        }

        public bool IsWaitingJoin()
        {
            return state == RoomState.WaitingJoin;
        }
        public bool IsWaitingBattle()
        {
            return state == RoomState.WaitingBattle;
        }
        public void AddClient(Client client)//初始化房间内对局要素、设置状态
        {
            client.HP = MAX_HP;
            clientRoom.Add(client);
            client.Room = this;
            if (clientRoom.Count>= 2)
            {
                state = RoomState.WaitingBattle;
            }
        }
        public void RemoveClient(Client client)
        {
            client.Room = null;
            clientRoom.Remove(client);//将玩家从房间剔除的方法

            if (clientRoom.Count >= 2)
            {
                state = RoomState.WaitingBattle;
            }
            else
            {
                state = RoomState.WaitingJoin;
            }
        }
        public string GetHouseOwnerData()
        {
            return clientRoom[0].GetUserData();
        }
        
        public int GetId()
        {
            if (clientRoom.Count > 0)
            {
                return clientRoom[0].GetUserId();
            }
            return -1;
        }
        public String GetRoomData()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Client client in clientRoom)
            {
                sb.Append(client.GetUserData() + "|");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
        public void BroadcastMessage(Client excludeClient,ActionCode actionCode,string data)
        {
            foreach(Client client in clientRoom)
            {
                if (client != excludeClient)
                {
                    server.SendResponse(client, actionCode, data);
                }
            }
        }
        public bool IsHouseOwner(Client client)
        {
            return client == clientRoom[0];
        }
        public void QuitRoom(Client client)
        {
            if (client == clientRoom[0])
            {
                Close();
            }
            else
                clientRoom.Remove(client);
        }
        public void Close()
        {
            foreach(Client client in clientRoom)
            {
                client.Room = null;
            }
            server.RemoveRoom(this);
        }
        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }
        private void RunTimer()//3秒计时器
        {
            Thread.Sleep(1000);
            for (int i = 3; i > 0; i--)
            {
                BroadcastMessage(null, ActionCode.ShowTimer, i.ToString());
                Thread.Sleep(1000);
            }
            BroadcastMessage(null, ActionCode.StartPlay, "r");
        }

        public void Heal(int damage, Client excludeClient)
        {
            foreach (Client client in clientRoom)
            {
                if (client == excludeClient)
                {
                    client.TakeDamage(damage);
                }
            }
        }
        public void TakeDamage(int damage,Client excludeClient)
        {
            bool isDie = false;
            foreach (Client client in clientRoom)
            {
                if (client != excludeClient)
                {
                    if (client.TakeDamage(damage))
                    {
                        isDie = true;
                    }
                }
            }
            if (isDie == false) return;
            //如果其中一个角色死亡，要结束游戏
            foreach (Client client in clientRoom)
            {
                if (client.IsDie())
                {
                    client.UpdateResult(false);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                }
                else
                {
                    client.UpdateResult(true);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                }
            }
            Close();
        }
    }
}
