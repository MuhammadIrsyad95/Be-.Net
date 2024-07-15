//using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Repositories.Interface
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        void Create(string name, string description, int price, string imageUrl, int kategori_id);
        bool Update(int id, string name, string description, int price, string imageUrl, int kategori_id); // Memiliki parameter yang sama
        void SetActive(int id);
        Product GetById(int id);
        List<ProductLanding> GetCourseLimit(int userId);
        List<ProductLanding> GetCourseLimits();
        ProductLanding GetCourseById(int CourseId);
        List<ProductLanding> GetCourseByCategoryId(int categoryId);

    }
}
