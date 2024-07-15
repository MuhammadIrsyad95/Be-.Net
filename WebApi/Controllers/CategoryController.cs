using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Repositories.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryRepository categoriesRepository;
        public CategoryController(ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment) 
        {
            categoriesRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;

        }
        //[Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> categories = categoriesRepository.GetAll();
            return Ok(categories);
        }

        //[Authorize]
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            Category categories = categoriesRepository.GetCategoryById(id);
            return Ok(categories);
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDto data)
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
                string uploadDir = "wwwroot/category";

                string physicalPath = Path.Combine(_webHostEnvironment.ContentRootPath, uploadDir);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                string filePath = Path.Combine(physicalPath, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await image.CopyToAsync(stream);
                }

                categoriesRepository.Create(data.Nama_kategori, data.Deskripsi, fileName);

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception

                // Clean up: Delete the partially written file if exists

                return StatusCode(500, "Internal Server Error");
            }
        }

        
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CreateCategoryDto data)
        {
            var existingItem = categoriesRepository.GetById(id);

            if (existingItem == null)
            {
                return NotFound("Category not found for the given ID.");
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "category", existingItem.ImageUrl);

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
                    string uploadDir = "wwwroot/category";
                    string physicalPath = Path.Combine(_webHostEnvironment.ContentRootPath, uploadDir);
                    var filePath = Path.Combine(physicalPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await data.ImageUrl.CopyToAsync(stream);
                    }

                    string fileUrlPath = Path.Combine(uploadDir, fileName);

                    Console.WriteLine($"New Image Path: {fileUrlPath}");

                    categoriesRepository.Update(id, data.Nama_kategori, data.Deskripsi, fileName);
                }
                else
                {
                    categoriesRepository.Update(id, data.Nama_kategori, data.Deskripsi, existingItem.ImageUrl);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting or updating file: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

      
        [HttpPatch("{id}/Activate")]
        public IActionResult Activate(int id)
        {
            categoriesRepository.SetActive(id);

            return Ok();
        }

        //[Authorize(Roles = "admin")]
        [HttpPatch("{id}/Deactive")]
        public IActionResult Deactive(int id)
        {
            categoriesRepository.SetActive(id);

            return Ok();
        }


    }
}
