using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetdataController : Controller
    {
        private readonly GetdataRepository _getdataRepository;
        public GetdataController(GetdataRepository checkoutRepository)
        {
            _getdataRepository = checkoutRepository;
        }

        
    }
}
