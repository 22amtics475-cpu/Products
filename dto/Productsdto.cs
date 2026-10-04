namespace Products.dto
{
    public class Products
    {
        public int pid {  get; set; }
        public string name { get; set; }
        public int prize { get; set; }

        public Products(int pid, string name, int prize)
        {
            this.pid = pid;
            this.name = name;
            this.prize = prize;
        }
    }

}
