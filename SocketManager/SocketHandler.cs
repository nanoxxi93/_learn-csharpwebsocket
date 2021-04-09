using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZyxMeSocket.SocketManager
{
    public abstract class SocketHandler
    {
        public ConnectionManager Connections { get; set; }

        public SocketHandler(ConnectionManager connections)
        {
            Connections = connections;
        }

        public virtual async Task OnConnected(string roomId, WebSocket socket, HttpContext context = null, dynamic data = null)
        {
            await Task.Run(() => { Connections.AddSocket(roomId, socket, context, data); });
        }

        public virtual async Task OnDisconnected(string roomId, WebSocket socket)
        {
            await Connections.RemoveSocketAsync(roomId, Connections.GetId(roomId, socket));
        }

        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetByteCount(message)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task SendMessage(string roomId, string id, string message)
        {
            await SendMessage(Connections.GetWebSocket(roomId, id), message);
        }

        public async Task SendMessageToAll(string roomId, string message)
        {
            foreach (var con in Connections.GetAllConnections(roomId))
                await SendMessage(con.Socket, message);
        }

        public async Task SendMessageToAllOthers(string roomId, string socketId, string message)
        {
            foreach (var con in Connections.GetAllOtherConnections(roomId, socketId))
                await SendMessage(con.Socket, message);
        }

        public abstract Task Receive(string roomId, WebSocket socket, WebSocketReceiveResult result, byte[] buffer, HttpContext context, JObject data);
    }
}
