using System.Text.Json.Serialization;

namespace WebApi.Dtos
{
    public class PaymentUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Rekening { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
