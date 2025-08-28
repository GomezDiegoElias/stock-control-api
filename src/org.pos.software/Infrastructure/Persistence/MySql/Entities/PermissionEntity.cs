using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace org.pos.software.Infrastructure.Persistence.MySql.Entities
{
    [Table("tbl_permission")]
    public class PermissionEntity
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }

        public ICollection<RolePermissionEntity> RolePermissions { get; set; } = new List<RolePermissionEntity>();

    }
}
