using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace ZyxMeSocket.SocketManager
{
    public class RoomSocket
    {
        public string RoomId { get; set; }
        public List<UserSocket> Sockets { get; set; }
        public RoomSocket(string roomId)
        {
            RoomId = roomId;
            Sockets = new List<UserSocket>();
        }
    }
    public class UserSocket
    {
        public string SockedId { get; set; }
        public WebSocket Socket { get; set; }
        public UserSocket(string socketId, WebSocket socket)
        {
            SockedId = socketId;
            Socket = socket;
        }
    }
    public class ConnectionManager
    {
        //private ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>> _connections = new ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>>();
        private List<RoomSocket> _connections = new List<RoomSocket>();

        public UserSocket GetUserSocket(string roomId, string id)
        {
            return GetAllConnections(roomId).FirstOrDefault(x => x.SockedId == id);
        }
        public WebSocket GetWebSocket(string roomId, string id)
        {
            return GetAllConnections(roomId).FirstOrDefault(x => x.SockedId == id)?.Socket;
        }

        public string ValidateWebSocket(string roomId, string id)
        {
            if (GetWebSocket(roomId, id) == null)
                return id;
            else
                return $"{id}({GetAllConnections(roomId).Count(x => x.SockedId.Contains(id)) + 1})";
        }

        public List<UserSocket> GetAllConnections(string roomId)
        {
            return _connections.FirstOrDefault(x => x.RoomId == roomId)?.Sockets;
        }

        public List<UserSocket> GetAllOtherConnections(string roomId, string id)
        {
            return _connections.FirstOrDefault(x => x.RoomId == roomId).Sockets.FindAll(x => x.SockedId != id);
        }

        public string GetId(string roomId, WebSocket webSocket)
        {
            return GetAllConnections(roomId).FirstOrDefault(x => x.Socket == webSocket).SockedId;
        }

        public async Task RemoveSocketAsync(string roomId, string id)
        {
            var socket = GetUserSocket(roomId, id);
            GetAllConnections(roomId).RemoveAt(GetAllConnections(roomId).FindIndex(x => x.SockedId == id));
            await socket.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket connection closed", CancellationToken.None);
        }

        public void CheckRoom(string roomId)
        {
            if (GetAllConnections(roomId) == null)
            {
                _connections.Add(new RoomSocket(roomId));
            }
        }

        public void AddSocket(string roomId, WebSocket socket, HttpContext context, dynamic data)
        {
            CheckRoom(roomId);
            GetAllConnections(roomId).Add(new UserSocket(ValidateWebSocket(roomId, context.Connection.RemoteIpAddress.ToString()), socket));
            //GetAllConnections(roomId).Add(new UserSocket(ValidateWebSocket(roomId, data), socket));
            //GetAllConnections(roomId).TryAdd(GetConnectionId(), socket);
        }
        private string GetConnectionId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
