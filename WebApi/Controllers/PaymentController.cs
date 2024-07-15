using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories.Interface;
using WebApi.Models;
using WebApi.Dtos;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _paymentRepository = paymentRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        //[Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetPayment()
        {
            List<Payment> payments = _paymentRepository.GetAll();
            return Ok(payments);
        }

        [HttpGet("User")]
        public IActionResult GetUserPayment()
        {
            List<Payment> payments = _paymentRepository.GetUserPayment();
            return Ok(payments);
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromForm] Payment_AddDto data)
        {
            /*================== upload image ==================*/
            if (data.ImageUrl == null || data.ImageUrl.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            try
            {
                var image = data.ImageUrl;
                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                string fileName = Guid.NewGuid().ToString() + ext;
                string uploadDir = "wwwroot/payment";

                string physicalPath = Path.Combine(_webHostEnvironment.ContentRootPath, uploadDir);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                string filePath = Path.Combine(physicalPath, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await image.CopyToAsync(stream);
                } // Memastikan path file terdiri dari direktori dan nama file

                _paymentRepository.Create(data.Nama_metode, fileName);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception

                // Clean up: Delete the partially written file if exists

                return StatusCode(500, "Internal Server Error");
            }
        }

        //[Authorize(Roles = "admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromForm] Payment_AddDto data)
        {
            var existingItem = _paymentRepository.GetById(id);

            if (existingItem == null)
            {
                return NotFound("Payment not found for the given ID.");
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "payment", existingItem.ImageUrl);

            try
            {
                Console.WriteLine($"Old Image Path: {oldImagePath}");

                if (System.IO.File.Exists(oldImagePath))
                {
                    Console.WriteLine("Old Image Exists. Deleting...");

                    System.IO.File.Delete(oldImagePath);

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        Console.WriteLine("File still exists after deletion attempt.");
                    }
                    else
                    {
                        Console.WriteLine("File deleted successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("Old file does not exist.");
                }

                if (data.ImageUrl != null)
                {
                    var ext = Path.GetExtension(data.ImageUrl.FileName).ToLowerInvariant();
                    string fileName = Guid.NewGuid().ToString() + ext;
                    string uploadDir = "wwwroot/payment";
                    string physicalPath = Path.Combine(_webHostEnvironment.ContentRootPath, uploadDir);
                    var filePath = Path.Combine(physicalPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await data.ImageUrl.CopyToAsync(stream);
                    }

                    string fileUrlPath = Path.Combine(uploadDir, fileName);

                    Console.WriteLine($"New Image Path: {fileUrlPath}");

                    _paymentRepository.Update(id, data.Nama_metode, fileName);
                }
                else
                {
                    _paymentRepository.Update(id, data.Nama_metode, existingItem.ImageUrl);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting or updating file: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        //[Authorize(Roles = "admin")]
        [HttpPatch("{id}/Activate")]
        public IActionResult Activate(int id)
        {
            _paymentRepository.SetActive(id);
            return Ok();
        }

        //[Authorize(Roles = "admin")]
        [HttpPatch("{id}/Deactive")]
        public IActionResult Deactive(int id)
        {
            _paymentRepository.SetActive(id);
            return Ok();
        }
    }
}