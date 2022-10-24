using AvansGreen.WebApp.Models;
using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AvansGreen.WebApp.Controllers
{
    public class PacketController : Controller
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
        public IActionResult PacketOverview()
        {
            return View(_packetRepository.GetPackets().ToList());
        }

        public IActionResult MyPackets()
        {
            return View(_packetRepository.GetPackets().ToList());
        }

        public IActionResult PacketDetail(int Id)
        {
            return View(_packetRepository.GetById(Id));
        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        [HttpGet]
        public IActionResult AddPacket()
        {
            var model = new NewPacketViewModel();

            PrefillSelectOptions(model);

            return View(model);
        }

        private void PrefillSelectOptions(NewPacketViewModel vm)
        {
            var list = new SelectList(Enum.GetValues(typeof(MealType)).Cast<MealType>().Select(mt => new SelectListItem
            {
                Text = mt.ToString(),
                Value = ((int)mt).ToString(),
            }), "Value", "Text");
            ViewBag.MealTypes = list;

            vm.AllProducts = _productRepository.GetProducts().ToList();

        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddPacket(NewPacketViewModel vm, List<int> ProductIdList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int? canteenEmployeeId = HttpContext.Session.GetInt32("CanteenEmployeeId");
                    if (canteenEmployeeId != null)
                    {
                        Packet newPacket = new(vm.Name!, vm.PickUpTimeStart, vm.PickUpTimeEnd, vm.IsAlcoholic, vm.Price, vm.TypeOfMeal, (int)canteenEmployeeId);
                        await _packetRepository.AddPacket(newPacket);

                        foreach (int id in ProductIdList)
                        {
                            newPacket.Products.Add(new PacketProduct(newPacket.Id, id));
                        }
                        await _packetRepository.AddProductsToPacket(newPacket.Products);
                        return RedirectToAction("PacketOverview");
                    }

                }
                catch (Exception e)
                {
                    ModelState.AddModelError(
                        "Error creating packet",
                        e.Message);
                }
            }

            PrefillSelectOptions(vm);

            return View(vm);
        }
    }
}
