//using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Repositories.Interface
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        void Create(string name, string description, string imageUrl);
        bool Update(int id, string name, string description, string imageUrl); // Memiliki parameter yang sama
        void SetActive(int id);
        Category GetById(int id);

        Category GetCategoryById(int id);
        //void Create(string nama_kategori, string deskripsi, string fileUrlPath);
    }
}
