using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ZyxMeSocket.SocketManager
{
    public class WebSocketMessageHandler : SocketHandler
    {
        public WebSocketMessageHandler(ConnectionManager connections): base(connections)
        {
        }
        public override async Task OnConnected(string roomId, WebSocket socket, HttpContext context = null, dynamic data = null)
        {
            await base.OnConnected(roomId, socket, context);
            var socketId = Connections.GetId(roomId, socket);
            var message = JsonConvert.SerializeObject(new
            {
                userid = socketId,
                message = $"***** {socketId} se unió al chat *****"
            });
            await SendMessageToAllOthers(roomId, socketId, message);
        }
        public override async Task Receive(string roomId, WebSocket socket, WebSocketReceiveResult result, byte[] buffer, HttpContext context, JObject data)
        {
            var socketId = Connections.GetId(roomId, socket);
            var message = JsonConvert.SerializeObject(new { 
                userid = socketId,
                message = Encoding.UTF8.GetString(buffer, 0, result.Count)
            });
            await SendMessageToAllOthers(roomId, socketId, message);
        }
    }
}
