using System.Text.Json.Serialization;

namespace WebApi.Dtos
{
    public class Payment_AddDto
    {
        public string Nama_metode { get; set; } = string.Empty;
        //public string Rekening { get; set; } = string.Empty;
        //public string imageUrl { get; set; } = string.Empty;
        public IFormFile ImageUrl { get; set; }

    }
}
