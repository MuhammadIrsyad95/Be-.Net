namespace WebApi.Dtos
{
    public class MakeOrderDto
    {
        public List<int> KeranjangIds { get; set; } = new List<int>();
        public int metode_id { get; set; }
    }
}
