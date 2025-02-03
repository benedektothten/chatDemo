namespace GrpcChatDemo.Hubs;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task JoinChatSession(int chatSessionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatSessionId.ToString());
    }

    public async Task LeaveChatSession(int chatSessionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatSessionId.ToString());
    }
}