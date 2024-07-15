namespace WebApi.Dtos
{
    public class MakeCartDto
    {
        public List<int> ProdukIds { get; set; } = new List<int>();


        public string schedule { get; set; } = string.Empty;

    }
}
