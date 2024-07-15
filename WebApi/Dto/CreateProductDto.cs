namespace WebApi.Dto
{
    public class CreateProductDto
    {
        public string Nama_produk { get; set; } = String.Empty;   
        public string Deskripsi_produk { get; set; } = String.Empty;

        public int Harga { get; set; }

        public IFormFile ImageUrl { get; set; }

        //public bool Status { get; set; }

        public int Kategori_id { get; set; }

    }
}
