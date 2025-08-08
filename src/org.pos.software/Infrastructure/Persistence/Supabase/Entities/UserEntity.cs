using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace org.pos.software.Infrastructure.Persistence.Supabase.Entities
{

    [Table("tbl_user")]
    public class UserEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("country")]
        public string Country { get; set; }

        public UserEntity() { }

        public UserEntity(int id, string firstName, string country)
        {
            Id = id;
            FirstName = firstName;
            Country = country;
        }

    }
}
