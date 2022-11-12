using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UI.AvansGreenPortal.Models;

namespace UI.AvansGreenPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPacketRepository _packetRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICanteenEmployeeRepository _canteenEmployeeRepository;

        public HomeController(ILogger<HomeController> logger,
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

        [Authorize]
        public IActionResult Index(CurrentUserViewModel currentUserVM)
        {
            if (currentUserVM.UserType is "CanteenEmployee" or "Admin")
            {
                CanteenEmployee canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(currentUserVM.Nr);
                if (canteenEmployee != null)
                {
                    HttpContext.Session.SetString("CanteenName", canteenEmployee.Canteen.Name);
                }
            }
            return RedirectToAction("PacketOverview", "Packet", new FilterPacketsViewModel() { CurrentUser = currentUserVM });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}