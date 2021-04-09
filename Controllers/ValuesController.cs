using System;
using System.IO;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ZyxMeSocket.Entities;
using ZyxMeSocket.SocketManager;

namespace ZyxMeSocket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private AppSettings appSettings;
        IConfiguration configuration;
        ConnectionManager connections;

        public ValuesController(IOptionsSnapshot<AppSettings> appSettings, IConfiguration configuration, ConnectionManager connections)
        {
            this.appSettings = appSettings.Value;
            this.configuration = configuration;
            this.connections = connections;
        }

        // GET api/values
        [HttpGet]
        
        public IActionResult Get()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Stop();
            return Ok($"API is working Execution Time: {watch.ElapsedMilliseconds} ms");
        }

        // GET api/values
        [HttpPost]

        public IActionResult Profile()
        {
            var room = HttpContext.Request.Form["room"];
            var username = HttpContext.Request.Form["username"];
            var buffer = new byte[1024 * 4];
            Stream stream = new MemoryStream(buffer);
            WebSocket websocket = WebSocket.CreateFromStream(stream, true, Guid.NewGuid().ToString(), System.Threading.Timeout.InfiniteTimeSpan);
            connections.AddSocket(room, websocket, null, username);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Stop();
            return Ok($"API is working Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
