using Microsoft.AspNetCore.SignalR;

namespace SignalRServer
{
    public class StatsHub : Hub
    {
        public void UpdateStats(Stats stats) =>
            Clients.All.SendAsync("updateStats", stats);
    }
}
