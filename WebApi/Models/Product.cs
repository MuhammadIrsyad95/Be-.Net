namespace WebApi.Models
{
    //contain all product properties
    public class Product
    {
        public int Id { get; set; }

        public string Nama_produk { get; set; } = string.Empty;

        public string Deskripsi_produk { get; set; } = string.Empty;

        public int Harga { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public int Status { get; set; }

        public int Kategori_id { get; set; }
        public string NamaKategori { get; set; } = string.Empty;
    }

    public class ProductLanding
    {
        public int Id { get; set; }
        public string NamaCourse { get; set; } = string.Empty;
        public string Deskripsi { get; set; } = string.Empty;
        public int Harga { get; set; }
        public string imageUrl { get; set; } = string.Empty;
        public int IdKategori { get; set; }
        public string NamaKategori { get; set; } = string.Empty;
        //public string NamaPembeli { get; set; } = string.Empty;
        //public string Schedule { get; set; } = string.Empty;
    }
}
