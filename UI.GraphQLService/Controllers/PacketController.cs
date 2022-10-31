using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.AspNetCore.Mvc;

namespace UI.RESTfulAPIWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacketController : ControllerBase
    {
        private readonly ILogger<PacketController> _logger;
        private readonly IPacketRepository _packetRepository;

        public PacketController(ILogger<PacketController> logger,
            IPacketRepository packetRepository)
        {
            _logger = logger;
            _packetRepository = packetRepository;
        }

        [HttpGet(Name = "GetAllPackets")]
        public ActionResult<List<Packet>> GetAll()
        {
            return Ok(_packetRepository.GetPackets());
        }

    }
}
