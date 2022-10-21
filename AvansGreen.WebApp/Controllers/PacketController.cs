using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.AspNetCore.Mvc;

namespace AvansGreen.WebApp.Controllers
{
    public class PacketController : Controller
    {
        private readonly ILogger<PacketController> _logger;
        private readonly IPacketRepository _packetRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICanteenEmployeeRepository _canteenWorkerRepository;

        public PacketController(ILogger<PacketController> logger,
            IPacketRepository packetRepository,
            IProductRepository productRepository,
            IStudentRepository studentRepository,
            ICanteenEmployeeRepository canteenWorkerRepository)
        {
            _logger = logger;
            _packetRepository = packetRepository;
            _productRepository = productRepository;
            _studentRepository = studentRepository;
            _canteenWorkerRepository = canteenWorkerRepository;
        }
        public IActionResult PacketOverview()
        {
            return View(_packetRepository.GetPackets().ToList());
        }

        public IActionResult PacketDetail(Packet packet)
        {
            return View(_packetRepository.GetById(packet.Id));
        }
    }
}
