using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Products.Data;
using Products.dto;
using Products.model;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
namespace Products.Mapendpoints
{
    public static class ProductsEndpoint
    {
        private static List<Product> Products = new List<Product>();

        static ProductsEndpoint()
        {
            Products.Add(new Product
            {
                pid = 1,
                name = "Laptop",
                prize = 60000
            });

            Products.Add(new Product
            {
                pid = 2,
                name = "Refrigerator",
                prize = 400000
            });

            Products.Add(new Product
            {
                pid = 3,
                name = "TV",
                prize = 900000
            });
        }
            
            public static void MapProductsEndpoints(this WebApplication app)
        {

            app.MapGet("/", () => "HELLO");

            app.MapGet("/Products", () => Products);

            app.MapGet("/Products/{id}", (int id) => Products.Find(Product => Product.pid == id))
                .WithName("getproducts");

            app.MapPost("/Products", (CreateProducts newpr) =>
            {
                Product product = new Product
                {
                    pid = Products.Count + 1,
                    name = newpr.name,
                    prize = newpr.prize
                };


                Products.Add(product);

                return Results.CreatedAtRoute("getproducts", new { id = product.pid }, product);
            });

            app.MapPut("/Products/{id}", (int id, UpdateProducts Updatepr) =>
            {
                var index = Products.FindIndex(Product => Product.pid == id);
                Products[index] = new Product
                {
                    pid = id,
                    name = Updatepr.name,
                    prize = Updatepr.prize
                };
            });

            app.MapDelete("/Products/{id}", (int id) =>
            {
                Products.RemoveAll(Product => Product.pid == id);

                return Results.NoContent();

            });

            app.MapPost("/Login", (Logindto  login) => 
            {
                var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("this-is-my-secret-key-for-jwt-123456789")
            );

                var credentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );

                 var claims = new[]
               {
                new Claim(ClaimTypes.Role, "Admin")
                 };

                var token = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: credentials,
                    expires: DateTime.Now.AddMinutes(30)
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(tokenString);

            });

            app.MapPost("/Upload", async ([FromForm] UploadFile File, IWebHostEnvironment environment) =>
            {
                var uploadFolder = Path.Combine(environment.WebRootPath, "uploads");

                var filePath = Path.Combine(uploadFolder, File.File.FileName);

                using var stream = new FileStream(filePath, FileMode.Create);

                await File.File.CopyToAsync(stream);

                return Results.Ok("File uploaded successfully");
            }).DisableAntiforgery();
            app.MapGet("/retrieveFile/{filename}", (string filename, IWebHostEnvironment environment) =>
            {
                var filePath = Path.Combine(environment.WebRootPath, "uploads", filename);

                if (!File.Exists(filePath))
                {
                    return Results.NotFound();
                }

                return Results.File(filePath, "application/octet-stream");
            });

            app.MapPost("/sendEmail", (sendEmail email, ProductContext context) =>
            {
                Random random = new Random();

                int otp = random.Next(100000, 1000000);

                DateTime expiryTime = DateTime.Now.AddMinutes(5);

                Otp newOtp = new Otp
                {
                    Email = email.Email,
                    Code = otp,
                    ExpiryTime = expiryTime
                };

                context.Otps.Add(newOtp);
                context.SaveChanges();

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;

                    smtp.Credentials = new NetworkCredential(
                        "your email",
                        "your code "
                    );

                    MailMessage message = new MailMessage();

                    message.From = new MailAddress("your email");
                    message.To.Add(email.Email);

                    message.Subject = "Your OTP";
                    message.Body = $"Your OTP is {otp}. It will expire in 5 minutes.";

                    smtp.Send(message);
                }

                return Results.Ok("OTP sent successfully");
            });

            app.MapPost("/verifyOtp", (verifyOtp data, ProductContext context) =>
            {
                var otpRecord = context.Otps
                    .Where(x => x.Email == data.Email && x.Code == data.OTP)
                    .FirstOrDefault();

                if (otpRecord == null)
                {
                    return Results.BadRequest("Invalid OTP");
                }

                if (otpRecord.ExpiryTime < DateTime.Now)
                {
                    return Results.BadRequest("OTP has expired");
                }

                return Results.Ok("OTP verified successfully");
            });
        }

    }
}
