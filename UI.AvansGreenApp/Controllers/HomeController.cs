using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UI.AvansGreenApp.Models;
using UI.AvansGreenApp.Security;

namespace UI.AvansGreenApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPacketRepository _packetRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICanteenEmployeeRepository _canteenEmployeeRepository;
        private readonly UserManager<AvansGreenUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            IPacketRepository packetRepository,
            IProductRepository productRepository,
            IStudentRepository studentRepository,
            ICanteenEmployeeRepository canteenEmployeeRepository,
            UserManager<AvansGreenUser> userMgr)
        {
            _logger = logger;
            _packetRepository = packetRepository;
            _productRepository = productRepository;
            _studentRepository = studentRepository;
            _canteenEmployeeRepository = canteenEmployeeRepository;
            _userManager = userMgr;
        }

        [Authorize]
        public IActionResult Index(CurrentUserViewModel currentUserVM)
        {
            if (currentUserVM.TypeOfUser is TypeOfUser.Employee or TypeOfUser.Admin)
            {
                CanteenEmployee? canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(currentUserVM.Nr);
                if (canteenEmployee != null)
                {
                    HttpContext.Session.SetString("CanteenName", canteenEmployee.Canteen.Name);
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Profile()
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