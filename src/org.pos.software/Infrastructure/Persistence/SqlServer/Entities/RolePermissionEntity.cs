using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Entities
{

    [Table("tbl_role_permission")]
    public class RolePermissionEntity
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Role")]
        [Column("role_id")]
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

        [ForeignKey("Permission")]
        [Column("permission_id")]
        public int PermissionId { get; set; }
        public PermissionEntity Permission { get; set; }

    }
}
