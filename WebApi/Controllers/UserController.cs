using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Helper;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserRepository usersRepository;
        public UserController(UserRepository usersRepository)
        {
            this.usersRepository = usersRepository;

        }

        //[Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetAllproducts()
        {
            List<User> users = usersRepository.GetAll();
            return Ok(users);
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult CreateforAdmin([FromBody] CreateUserDTO data)
        {
            string hashedPassword = PasswordHelper.EncryptPassword(data.Password);

            usersRepository.CreateAdmin(data.Nama, data.Email, hashedPassword);
            //usersRepository.CreateAdmin(data.Nama, data.Email, data.Password, data.Role, data.Status);
            return Ok();
        }

        //[Authorize(Roles = "admin")]
        [HttpPatch("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] CreateUserDTO data)
        {
            usersRepository.Update(id, data.Nama, data.Email, data.Password);

            return Ok(new
            {
                Id = id,
                Data = data
            });
        }

        //[HttpPatch("{id}/status")] cek video
        //public IActionResult UpdateProduct2(int id, [FromBody] CreateProductDto data)
        //{
        // productsRepository.Update(id, data.status);

        // return Ok(new
        //{
        //Id = id,
        // Status = StatusCode,
        // Data = data
        // });
        // }

        //[Authorize(Roles = "admin")]
        [HttpPatch("{id}/Activate")]
        public IActionResult Activate(int id)
        {
            usersRepository.SetActive(id);

            return Ok();
        }

        //[Authorize(Roles = "admin")]
        [HttpPatch("{id}/Deactive")]
        public IActionResult Deactive(int id)
        {
         usersRepository.SetActive(id);

            return Ok();
        }
    }
}
