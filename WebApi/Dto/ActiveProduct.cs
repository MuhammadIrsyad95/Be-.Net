namespace WebApi.Dto
{
    public class ActiveProduct
    {
        public string Nama_produk { get; set; } = String.Empty;
        public string Deskripsi_produk { get; set; } = String.Empty;

        public int Harga { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public bool Status { get; set; }

        public int Kategori_id { get; set; }
    }
}
