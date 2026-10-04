using System.ComponentModel.DataAnnotations;

namespace Products.dto
{
    public class CreateProducts
    {
        public int pid { get; set; }
        [Required]
        [MaxLength(50)] public string name { get; set; }

        [Range(1, 100000)] public int prize { get; set; }
    }
}
