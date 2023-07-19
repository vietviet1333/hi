
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Nexus_Manegement.Models
{
    [Table("Store")]
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? Name { get; set; }    
        public int? DistrictID { get; set; }

        [ForeignKey("DistrictID")]
        public District? StoreDistrict { get; set; }
        public string Ward { get; set; }
        public bool status { get; set; }
        [NotMapped]
        public string? Distric { get; set; }

    }
}
