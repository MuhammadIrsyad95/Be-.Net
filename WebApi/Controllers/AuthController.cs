using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Dto;
using WebApi.Helper;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository _userRepository)
        {
            this._userRepository = _userRepository;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto data)
        {
            string hashedPassword = PasswordHelper.EncryptPassword(data.Password);

            User? user = _userRepository.GetByEmailAndPassword(data.Email, hashedPassword);

            if (user == null)
            {
                return NotFound();
            }

            //create token
            string token = JWTHelper.Generate(user.Id, user.Role);

            return Ok(token);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO data)
        {
            string hashedPassword = PasswordHelper.EncryptPassword(data.Password);

            User? user = _userRepository.GetByEmail(data.Email);

            if (user != null)
            {
                return BadRequest("Email already registered.");
            }
            

            _userRepository.CreateUser(data.Nama, data.Email, hashedPassword);

            string emailConfirmationLink = "http://localhost:5173/emailpage/?email=" + data.Email;

            await MailHelper.Send( data.Email, "Email Confirmation", "Hello " + data.Email + ", Your Email Confirmation link: " + emailConfirmationLink);


            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto data)
        {
            //check if email valid

            User? user = _userRepository.GetByEmail(data.Email);
            if (user == null)
            {
                return NotFound();
            }

            string KEY = DateTime.UtcNow.Ticks.ToString() + data.Email;
            string resetToken = PasswordHelper.EncryptPassword(KEY);

            //update reset token
            bool isSuccess = _userRepository.InsertResetPasswordToken(user.Id, resetToken);

            if (!isSuccess)
            {
                return Problem();
            }

            string resetLink = "http://localhost:5173/newpass/?email=" + data.Email + "&token=" + resetToken;

            await MailHelper.Send(data.Email, "Forgot Password", "Hello " + user.Email + ", Your reset password link: " + resetLink);

            return Ok();





        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto data)
        {
            //check if email valid
            User? user = _userRepository.GetByEmailAndResetToken(data.Email, data.Token);
            if (user == null)
            {
                return NotFound();
            }

            string hashedPassword = PasswordHelper.EncryptPassword(data.NewPassword);

            bool isResetSuccess = _userRepository.UpdatePassword(user.Id, hashedPassword);

            if (!isResetSuccess)
            {
                return Problem();
            }

            await MailHelper.Send(data.Email, "Reset Password Success", "Hello " + user.Email + ", Your new password is: " + data.NewPassword);

            return Ok();
        }

        [HttpGet]
        [Route("CheckEmail")]
        public IActionResult CheckEmail([FromQuery] string email)
        {
            // Lakukan pengecekan apakah email sudah terdaftar di database
            bool isEmailRegistered = _userRepository.Checkemail(email);

            return Ok(new { isEmailRegistered });
        }
    }
}
