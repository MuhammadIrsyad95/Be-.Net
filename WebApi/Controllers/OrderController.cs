using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories;
using WebApi.Dtos;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly OrderRepository orderRepository;
        public OrderController(OrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet("InvoiceUser")]
        public IActionResult GetInvoiceByUserId()
        {
            //diganti dengan auth user
            string userId = User.FindFirstValue(ClaimTypes.Sid);

            var invoices = orderRepository.GetInvoiceByUserId(Int32.Parse(userId));

            return Ok(invoices);
        }

        [HttpGet("DetailInvoice/{id}")]
        public IActionResult GetDetailInvoiceByUserId(int id)
        {
            //diganti dengan auth user
            //string userId = User.FindFirstValue(ClaimTypes.Sid);

            var invoices = orderRepository.GetDetailInvoiceByUserId(id);

            return Ok(invoices);
        }

        [HttpGet("DetailInvoiceItem/{id}")]
        public IActionResult GetDetailInvoiceItemeByUserId(int id)
        {
            //diganti dengan auth user
            //string userId = User.FindFirstValue(ClaimTypes.Sid);

            var invoices = orderRepository.GetDetailInvoiceItemeByUserId(id);

            return Ok(invoices);
        }

        [HttpGet("Mycalss")]
        public IActionResult GetDetailMyclass()
        {
            //diganti dengan auth user
            string userId = User.FindFirstValue(ClaimTypes.Sid);

            var invoices = orderRepository.GetDetailMyclass(Int32.Parse(userId));

            return Ok(invoices);
        }

        [HttpGet("InvoiceAllUser")]
        public IActionResult GetInvoiceAll()
        {
            //diganti dengan auth user
            //string userId = User.FindFirstValue(ClaimTypes.Sid);

            var invoices = orderRepository.GetInvoiceAll();

            return Ok(invoices);
        }

        [HttpPost]
        public IActionResult MakeOrder([FromBody] MakeOrderDto data)
        {
            //diganti dengan auth user
            //string userId = User.FindFirstValue(ClaimTypes.Sid);
            //string userId = "1";

            orderRepository.CreateOrderAndOrderDetail(data.metode_id, data.KeranjangIds);
            return Ok(data);
        }
    }
}
