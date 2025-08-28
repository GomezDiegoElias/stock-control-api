using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using org.pos.software.Domain.Entities;

namespace org.pos.software.Infrastructure.Persistence.MySql.Entities
{
    [Table("tbl_user")]
    public class UserEntity
    {

        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Required]
        [Column("dni")]
        public long Dni { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("hash")]
        public string Hash { get; set; }

        [Required]
        [Column("salt")]
        public string Salt { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("firstname")]
        public string FirstName { get; set; }

        [Required]
        public Status Status { get; set; } = Status.ACTIVE;

        [ForeignKey("Role")]
        [Column("role_id")]
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") != null ? DateTime.Now : DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") != null ? DateTime.Now : DateTime.Now;

        public UserEntity() { }

        public UserEntity(string id, long dni, string email, string hash, string salt, string firstName, Status status, int roleId)
        {
            Id = id;
            Dni = dni;
            Email = email;
            Hash = hash;
            Salt = salt;
            FirstName = firstName;
            Status = status;
            RoleId = roleId;
        }

    }
}
