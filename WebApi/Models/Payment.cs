namespace WebApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Nama_metode { get; set; } = string.Empty;
        //public string Rekening { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        //public string imageUrl { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
