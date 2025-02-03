using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcChatDemo.Persistance;
    [Table("user", Schema = "chat")]
    public class User
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }
        
        [Column("password")]
        public string? Password { get; set; }

        public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }

    [Table("chatsession", Schema = "chat")]
    public class ChatSession
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Required]
        [Column("owner_id")]
        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
    
    [Table("message", Schema = "chat")]
    public class Message
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("message")]
        public string? Content { get; set; }

        [Required]
        [Column("create")]
        public DateTime CreatedAt { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? Sender { get; set; }

        [Required]
        [Column("chat_session_id")]
        public int ChatSessionId { get; set; }

        [ForeignKey("ChatSessionId")]
        public ChatSession ChatSession { get; set; } = null!;
    }