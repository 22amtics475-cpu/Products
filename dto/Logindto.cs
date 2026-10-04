using System.ComponentModel.DataAnnotations;

namespace Products.dto
{
    public class Logindto
    {
       [Required] public string username { get; set; }

       [Required] public string password { get; set; }
    }
}
