namespace WebApi.Models
{
    public class Getdata
    {
        public int Id { get; set; }
        public string NamaCourse { get; set; } = string.Empty;
        public string Deskripsi { get; set; } = string.Empty;
        public int Harga { get; set; }
        public string imageUrl { get; set; } = string.Empty;
        public string NamaKategori { get; set; } = string.Empty;
        public string NamaPembeli { get; set; } = string.Empty;
        public string Schedule {  get; set; } = string.Empty;
    }

}
