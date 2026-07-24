using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopDomain.Models;

[Table("refresh_tokens")]
public class RefreshToken : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("token")]
    public string Token { get; set; } = string.Empty;

    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [Column("is_revoked")]
    public bool IsRevoked { get; set; } = false;

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
