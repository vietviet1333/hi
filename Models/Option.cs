using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Manegement.Models
{
    [Keyless]
    public class Option
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
