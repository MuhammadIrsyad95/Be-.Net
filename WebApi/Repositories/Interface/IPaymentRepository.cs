// IPaymentRepository.cs
using WebApi.Models;

namespace WebApi.Repositories.Interface
{
    public interface IPaymentRepository
    {
        List<Payment> GetAll();
        List<Payment> GetUserPayment();
        void Create(string name, string image);
        bool Update(int id, string name, string image); // Memiliki parameter yang sama
        void SetActive(int metodeId);
        Payment GetById(int id);
    }

}