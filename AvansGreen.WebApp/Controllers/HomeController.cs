using AvansGreen.WebApp.Models;
using Core.DomainServices.IRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AvansGreen.WebApp.Controllers
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
        public IActionResult Index()
        {
            //if (currentUserViewModel.TypeOfUser is TypeOfUser.Student or TypeOfUser.Admin)
            //{
            //    currentUserViewModel.Student = _studentRepository.GetByEmail(currentUserViewModel.Email);
            //}
            //if (currentUserViewModel.TypeOfUser is TypeOfUser.CanteenEmployee or TypeOfUser.Admin)
            //{
            //    currentUserViewModel.CanteenEmployee = _canteenEmployeeRepository.GetByEmail(currentUserViewModel.Email);
            //}

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