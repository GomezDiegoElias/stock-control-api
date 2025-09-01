using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Entities
{

    [Table("tbl_client")]
    public class ClientEntity
    {

        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Required]
        [Column("dni")]
        public long Dni { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("address")]
        public string Address { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") != null ? DateTime.Now : DateTime.MinValue;

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") != null ? DateTime.Now : DateTime.MinValue;

        public ClientEntity() { }

        public ClientEntity(string id, long dni, string firstname, string address)
        {
            Id = id;
            Dni = dni;
            FirstName = firstname;
            Address = address;
        }

    }

}
