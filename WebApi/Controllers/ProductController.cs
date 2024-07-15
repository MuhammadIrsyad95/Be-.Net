using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Repositories.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductRepository productsRepository;

        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            productsRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;

        }

        //[Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetAllproducts()
        {
            List<Product> products = productsRepository.GetAll();
            return Ok(products);
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto data)
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
                string uploadDir = "wwwroot/product";

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

                productsRepository.Create(data.Nama_produk, data.Deskripsi_produk, data.Harga, fileName, data.Kategori_id);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception

                // Clean up: Delete the partially written file if exists

                return StatusCode(500, "Internal Server Error");
            }
            //productsRepository.Create(data.Nama_produk, data.Deskripsi_produk, data.Harga, data.ImageUrl, data.Kategori_id);
            //return Ok();
        }

        //[Authorize(Roles = "admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProductDto data)
        {
            var existingItem = productsRepository.GetById(id);

            if (existingItem == null)
            {
                return NotFound("Product not found for the given ID.");
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "product", existingItem.ImageUrl);

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
                    string uploadDir = "wwwroot/product";
                    string physicalPath = Path.Combine(_webHostEnvironment.ContentRootPath, uploadDir);
                    var filePath = Path.Combine(physicalPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await data.ImageUrl.CopyToAsync(stream);
                    }

                    string fileUrlPath = Path.Combine(uploadDir, fileName);

                    Console.WriteLine($"New Image Path: {fileUrlPath}");

                    productsRepository.Update(id, data.Nama_produk, data.Deskripsi_produk, data.Harga, fileName, data.Kategori_id);
                }
                else
                {
                    productsRepository.Update(id, data.Nama_produk, data.Deskripsi_produk, data.Harga, existingItem.ImageUrl, data.Kategori_id);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting or updating file: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
            //===================================================
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
            productsRepository.SetActive(id);

            return Ok();
        }

        //[Authorize(Roles = "admin")]
        [HttpPatch("{id}/Deactive")]
        public IActionResult Deactive(int id)
        {
            productsRepository.SetActive(id);

            return Ok();
        }

        //===================================== Client Side =============================================

        [Authorize]
        [HttpGet("CourseLimit")]
        public IActionResult GetCourseLimit()
        {
            //diganti dengan auth user
            string userId = User.FindFirstValue(ClaimTypes.Sid);

            var productLandings = productsRepository.GetCourseLimit(Int32.Parse(userId));

            return Ok(productLandings);
        }

        [HttpGet("CourseLimits")]
        public IActionResult GetCourseLimits()
        {
            //diganti dengan auth user
            //string userId = User.FindFirstValue(ClaimTypes.Sid);

            var productLandings = productsRepository.GetCourseLimits();

            return Ok(productLandings);
        }

        //[Authorize]
        [HttpGet("Course/{id}")]
        public IActionResult GetCourseById(int id)
        {
            //diganti dengan auth user
            //string userId = User.FindFirstValue(ClaimTypes.Sid);

            var productLandings = productsRepository.GetCourseById(id);

            return Ok(productLandings);
        }

        //[Authorize]
        [HttpGet("CourseByCategory/{id}")]
        public IActionResult GetCourseByCategoryId(int id)
        {
            //diganti dengan auth user
            //string userId = User.FindFirstValue(ClaimTypes.Sid);

            var getdatas = productsRepository.GetCourseByCategoryId(id);

            return Ok(getdatas);
        }
    }


}
