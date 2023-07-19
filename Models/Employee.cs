using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Manegement.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool Status { get; set; }
        public int Position { get; set; }
        [ForeignKey("Position")]
        public Position? EmployeePosition { get; set; }
        public int? OTP { get; set; }
        [NotMapped]
        public string PositionName { get; set; }

    }
}
