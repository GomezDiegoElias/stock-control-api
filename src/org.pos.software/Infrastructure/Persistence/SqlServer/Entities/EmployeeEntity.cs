using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace org.pos.software.Infrastructure.Persistence.SqlServer.Entities
{
    [Table("tbl_employee")]
    public class EmployeeEntity
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

        [MaxLength(50)]
        [Column("last_name")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("workstation")]
        public string WorkStation { get; set; }

        [Required]
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        [Required]
        [Column("create_at")]
        public DateTime CreateAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") != null ? DateTime.Now : DateTime.Now;

        [Required]
        [Column("update_at")]
        public DateTime UpdateAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") != null ? DateTime.Now : DateTime.Now;

        public EmployeeEntity() { }

        public EmployeeEntity(string id, long dni, string firstName, string lastName, string workStation)
        {
            Id = id;
            Dni = dni;
            FirstName = firstName;
            LastName = lastName;
            WorkStation = workStation;
        }

    }
}
