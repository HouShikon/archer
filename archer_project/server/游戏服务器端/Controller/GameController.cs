using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
namespace GameServer.Controller
{
    class GameController:BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }
        public string StartGame(string data, Client client, Server server)
        {
            if (client.IsHouseOwner())
            {
                Room room =  client.Room;
                if(!room.IsWaitingBattle())
                    return ((int)ReturnCode.Fail).ToString();
                room.BroadcastMessage(client, ActionCode.StartGame, ((int)ReturnCode.Success).ToString());//发送开始游戏的广播
                room.StartTimer();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        public string Move(string data, Client client, Server server)//移动同步
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.Move, data);
            return null;
        }
        public string Shoot(string data, Client client, Server server)//射击同步
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.Shoot, data);
            return null;
        }
        public string Attack(string data, Client client, Server server)//伤害同步
        {
            int damage = int.Parse(data);
            Room room = client.Room;
            if (room == null) return null;
            room.TakeDamage(damage, client);
            return null;
        }
        public string Heal(string data, Client client, Server server)
        {
            int damage = int.Parse(data);
            Room room = client.Room;
            if (room == null) return null;
            room.Heal(damage, client);
            return null;
        }
        public string QuitBattle(string data, Client client, Server server)
        {
            Room room = client.Room;

            if (room != null)
            {
                room.BroadcastMessage(null, ActionCode.QuitBattle, "r");//退出房间，直接将房间关闭
                room.Close();
            }
            return null;
        }
    }
}
