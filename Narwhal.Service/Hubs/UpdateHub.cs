using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

using Narwhal.Service.Services;

namespace Narwhal.Service.Hubs
{
    public class UpdateHub : Hub
    {
        private readonly MessagingService _messagingService;

        public UpdateHub(MessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        public override async Task OnConnectedAsync()
        {
            IAsyncEnumerable<byte[]> messageEnumerable = await _messagingService.Subscribe("narwhal/navwarnings/update");

            messageEnumerable.ForEachAsync(async m =>
            {
                await Clients.All.SendAsync("NavWarnings");
            });

            await base.OnConnectedAsync();
        }
    }
}
