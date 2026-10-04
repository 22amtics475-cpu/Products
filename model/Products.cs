using System.ComponentModel.DataAnnotations;

namespace Products.model
{
    public class Product
    {
       
        [Key] public int pid { get; set; }
        public string name { get; set; }

        public int prize { get; set; }
    }
}
