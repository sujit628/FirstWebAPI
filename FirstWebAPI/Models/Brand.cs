using System.ComponentModel.DataAnnotations;

namespace FirstWebAPI.Models
{
    public class Brand
    {
        [Key]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public int IsActive { get; set; } = 1;
    }
}
