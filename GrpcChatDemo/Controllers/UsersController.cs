using AutoMapper;
using GrpcChatDemo.Models;
using GrpcChatDemo.Persistance;
using GrpcChatDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrpcChatDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICryptoService _cryptoService;

    public UsersController(ApplicationDbContext context, IMapper mapper, ICryptoService cryptoService)
    {
        _context = context;
        _mapper = mapper;
        _cryptoService = cryptoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.UserDto>>> GetUsers()
    {
        return Ok(_mapper.Map<IEnumerable<Models.UserDto>>(await _context.Users.ToListAsync())) ;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Models.UserDto>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        return _mapper.Map<UserDto>(user);
    }

    [HttpPost("create")]
    public async Task<ActionResult<Models.UserDto>> CreateUser([FromBody] UserDtoRequest request)
    {
        var encryptedPassword = await _cryptoService.EncryptData(request.Password);
        var newUser = new User
        {
            Name = request.Name,
            Password = encryptedPassword
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<UserDto>(newUser));
    }
    
    [HttpPost("authenticate")]
    public async Task<ActionResult<Models.UserDto>> AuthenticateUser([FromBody] UserDtoRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == request.Name);
        if (user == null) return NotFound();
        
        var decryptedPassword = await _cryptoService.DecryptData(user.Password);
        if (decryptedPassword != request.Password) return Unauthorized();
        
        return Ok(_mapper.Map<UserDto>(user));
    }
}