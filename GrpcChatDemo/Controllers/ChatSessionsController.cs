using AutoMapper;
using GrpcChatDemo.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrpcChatDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatSessionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ChatSessionsController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.ChatSessionDto>>> GetChatSessions()
    {
        var sessions = await _context.ChatSessions
            .Include(cs => cs.Owner)
            .Include(cs => cs.Messages)
            .ToListAsync();
        
        return Ok(_mapper.Map<IEnumerable<Models.ChatSessionDto>>(sessions));
    }
    
    [HttpPost]
    public async Task<ActionResult<Models.ChatSessionDto>> CreateChatSession(Models.ChatSessionDto chatSession)
    {
        _context.ChatSessions.Add(_mapper.Map<ChatSession>(chatSession));
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetChatSession), new { id = chatSession.Id }, chatSession);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Models.ChatSessionDto>> GetChatSession(int id)
    {
        var chatSession = await _context.ChatSessions
            .Include(cs => cs.Owner)
            .Include(cs => cs.Messages)
            .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(cs => cs.Id == id);

        if (chatSession == null) return NotFound();
        return _mapper.Map<Models.ChatSessionDto>(chatSession);
    }
}