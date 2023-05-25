using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
namespace GameServer.Controller
{
    class RoomController:BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }
        public string CreateRoom(string data, Client client, Server server)//创建房间，在服务器中创建一个list
        {
            server.CreateRoom(client);
            return ((int)ReturnCode.Success).ToString()+","+ ((int)RoleType.Blue).ToString();
        }
        public string ListRoom(string data, Client client, Server server)//搜索当前服务器内所有房间
        {
            StringBuilder sb = new StringBuilder();
            foreach(Room room in server.GetRoomList())
            {
                if (room.IsWaitingJoin())//只搜索可以加入的房间
                {
                    sb.Append(room.GetHouseOwnerData()+"|");
                }
            }
            if (sb.Length == 0)
            {
                sb.Append("0");
            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
        public string JoinRoom(string data, Client client, Server server)//加入房间
        {
            int id = int.Parse(data);
            Room room = server.GetRoomById(id);
            if(room == null)
            {
                return ((int)ReturnCode.NotFound).ToString();
            }
            else if (room.IsWaitingJoin() == false)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                room.AddClient(client);//加入房间
                string roomData = room.GetRoomData();//"returncode,roletype-id,username,tc,wc|id,username,tc,wc"
                room.BroadcastMessage(client, ActionCode.UpdateRoom, roomData);
                return ((int)ReturnCode.Success).ToString() + "," + ((int)RoleType.Red).ToString()+ "-" + roomData;//发回客户端
            }
        }
        public string QuitRoom(string data, Client client, Server server)
        {
            bool isHouseOwner = client.IsHouseOwner();
            Room room = client.Room;
            if (isHouseOwner)
            {
                room.BroadcastMessage(client, ActionCode.QuitRoom, ((int)ReturnCode.Success).ToString());
                room.Close();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                client.Room.RemoveClient(client);
                room.BroadcastMessage(client, ActionCode.UpdateRoom, room.GetRoomData());
                return ((int)ReturnCode.Success).ToString();
            }
        }
    }
}
