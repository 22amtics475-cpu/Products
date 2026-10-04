using Microsoft.AspNetCore.Mvc;

namespace Products.dto
{
    public class UploadFile
    {
         public IFormFile File { get; set; }
    }
}
