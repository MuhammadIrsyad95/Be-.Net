namespace WebApi.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Nama_kategori { get; set; } = string.Empty;

        public string Deskripsi { get; set; } = string.Empty;

 
        public string ImageUrl { get; set; } = string.Empty;

        public int Status { get; set; }

        

    }
}
