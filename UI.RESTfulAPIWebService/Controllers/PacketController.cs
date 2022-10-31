using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.RESTfulAPIWebService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PacketController : ControllerBase
    {
        private readonly ILogger<PacketController> _logger;
        private readonly IPacketRepository _packetRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICanteenEmployeeRepository _canteenEmployeeRepository;

        public PacketController(ILogger<PacketController> logger,
            IPacketRepository packetRepository,
            IProductRepository productRepository,
            IStudentRepository studentRepository,
            ICanteenEmployeeRepository canteenEmployeeRepository)
        {
            _logger = logger;
            _packetRepository = packetRepository;
            _productRepository = productRepository;
            _studentRepository = studentRepository;
            _canteenEmployeeRepository = canteenEmployeeRepository;
        }

        [HttpGet(Name = "packets")]
        public ActionResult<List<Packet>> GetAll()
        {

            return Ok(_packetRepository.GetPackets());
        }

        //[HttpPost]
        //public ActionResult<Packet> AddReservation(int studentId, int packetId)
        //{

        //}

    }
}
