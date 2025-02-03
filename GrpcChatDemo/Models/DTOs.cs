using System.Text.Json.Serialization;

namespace GrpcChatDemo.Models;

public class ChatSessionDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public UserDto Owner { get; set; } = null!;
    public List<MessageDto> Messages { get; set; } = new();
}

public class UserDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    [JsonIgnore]
    public string? Password { get; set; }
}

public class UserDtoRequest
{
    public string? Name { get; set; }
    public string? Password { get; set; }
}

public class MessageDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public int ChatSessionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserDto? Sender { get; set; }
}