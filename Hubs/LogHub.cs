using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AppointmentWebApp.Hubs { 
public class LogHub : Hub
{
    public async Task SendLogUpdate(string log)
    {
        await Clients.All.SendAsync("ReceiveLogUpdate", log);
    }
}

}