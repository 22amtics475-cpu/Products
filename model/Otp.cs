namespace Products.model
{
    public class Otp

    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int Code { get; set; }
        public DateTime ExpiryTime { get; set; }
    }

}
