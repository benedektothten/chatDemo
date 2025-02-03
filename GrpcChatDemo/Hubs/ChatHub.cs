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
    
    public async Task SendMessageToServer(string user, int sessionId, string message)
    {
        // Process the message (e.g., broadcast it to other clients)
        await Clients.Group(sessionId.ToString()).SendAsync("ReceiveMessage", user, message);
        //await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}