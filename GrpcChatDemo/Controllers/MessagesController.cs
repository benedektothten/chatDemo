using AutoMapper;
using GrpcChatDemo.Hubs;
using GrpcChatDemo.Models;
using GrpcChatDemo.Persistance;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GrpcChatDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IMapper _mapper;

    public MessagesController(ApplicationDbContext context, IHubContext<ChatHub> hubContext, IMapper mapper)
    {
        _context = context;
        _hubContext = hubContext;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Models.MessageDto>> SendMessage(Models.MessageDto message)
    {
        message.CreatedAt = DateTime.UtcNow;
        _context.Messages.Add(_mapper.Map<Message>(message));
        await _context.SaveChangesAsync();

        // Broadcast the message to all clients in the chat session
        await _hubContext.Clients
            .Group(message.ChatSessionId.ToString())
            .SendAsync("ReceiveMessage", message);

        return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Models.MessageDto>> GetMessage(int id)
    {
        var message = await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.ChatSession)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (message == null) return NotFound();
        return _mapper.Map<MessageDto>(message);
    }

    [HttpGet("session/{sessionId}")]
    public async Task<ActionResult<IEnumerable<Models.MessageDto>>> GetMessagesForSession(int sessionId)
    {
        var messages = await _context.Messages
            .Where(m => m.ChatSessionId == sessionId)
            .Include(m => m.Sender)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<MessageDto>>(messages));
    }
}