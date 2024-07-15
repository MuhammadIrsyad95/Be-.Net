namespace WebApi.Dto
{
    public class CreateCategoryDto
    {
        public string Nama_kategori { get; set; } = string.Empty;
        public string Deskripsi { get; set; } = string.Empty;
        public IFormFile ImageUrl { get; set; }


    }
}
