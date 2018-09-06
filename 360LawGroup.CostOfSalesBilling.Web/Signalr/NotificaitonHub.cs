using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace _360LawGroup.CostOfSalesBilling.Web.Signalr
{
    [HubName("notificaitonHub")]
    public class NotificationHub : Hub
    {
        public readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public void OnConnectDisconnect()
        {
            var keys = _connections.GetConnectionsKeys().ToArray();
            Clients.All.pushMessage(JsonConvert.SerializeObject(new { Type = "ConnectDisconnect", Message = keys }));
        }

        public override Task OnConnected()
        {
            string id = Context.User.Identity.GetUserId();
            _connections.Add(id, Context.ConnectionId);
            OnConnectDisconnect();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string id = Context.User.Identity.GetUserId();
            _connections.Remove(id, Context.ConnectionId);
            OnConnectDisconnect();
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string id = Context.User.Identity.GetUserId();
            if (!_connections.GetConnections(id).Contains(Context.ConnectionId))
            {
                _connections.Add(id, Context.ConnectionId);
            }
            OnConnectDisconnect();
            return base.OnReconnected();
        }
    }
}