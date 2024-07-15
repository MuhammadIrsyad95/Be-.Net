namespace WebApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        //public int UserId { get; set; }
        public string TglBeli { get; set; } = string.Empty;
    }

    public class Invoice
    {
        public int Id { get; set; }
        public string NoInvoice { get; set; } = string.Empty;
        public int JumlahKursus { get; set; }
        public string TanggalBeli { get; set; } = string.Empty;
        public int TotalHarga { get; set; }
    }

    public class DetInvoice
    {
        public string NamaCourse { get; set; } = string.Empty;
        public string NamaKategori { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public int Harga { get; set; }


    }

    public class Myclass : DetInvoice
    {
        public int IdProduk { get; set; }
        public string imageUrl { get; set; } = string.Empty;
    }

    public class InvoiceAdm : Invoice
    {
        public int IdPemesan { get; set; }
        public string NamaPemesan { get; set; } = string.Empty;

    }


}
