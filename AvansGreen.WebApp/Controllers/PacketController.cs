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
            return View(_packetRepository.GetPacketsWithoutReservation().ToList());
        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        public IActionResult CanteenPackets()
        {
            int? canteenId = HttpContext.Session.GetInt32("CanteenId");
            if (canteenId != null) return View("PacketOverview", _packetRepository.GetPacketsFromCanteen((int)canteenId).ToList());
            return View("PacketOverview", new List<Packet>());
        }

        [Authorize(Policy = "OnlyStudentsAndUp")]
        public IActionResult MyReservations()
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId != null) return View("PacketOverview", _packetRepository.GetPacketsReserverdByStudentWithId((int)studentId).ToList());
            return View("PacketOverview", new List<Packet>());
        }

        public IActionResult PacketDetail(int Id)
        {
            return View(_packetRepository.GetById(Id));
        }

        [Authorize(Policy = "OnlyStudentsAndUp")]
        public IActionResult AddReservation(int Id)
        {
            _logger.LogInformation("Inside addreservation");
            int? studentId = HttpContext.Session.GetInt32("StudentId");
            //int Id = packet.Id;
            if (studentId != null)
            {
                Student student = _studentRepository.GetById((int)studentId)!;
                Packet packet = _packetRepository.GetById(Id)!;
                _logger.LogInformation("Made student and packet: " + student.Id + ", " + packet.Id);
                foreach (Packet reservedPacket in student.ReservedPackets)
                {
                    if (reservedPacket.PickUpTimeStart.Date.Equals(packet.PickUpTimeStart.Date))
                    {
                        _logger.LogInformation("Already has a reservation on this day");
                        ModelState.AddModelError("", "You cannot make more than one reservation per day.");
                    }
                }
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Attempting to add reservation to db");
                    packet.StudentId = studentId;
                    _packetRepository.AddReservationToPacket(packet);
                    return RedirectToAction("MyReservations");
                }
            }
            _logger.LogInformation("Something went wrong, returning view again");
            return RedirectToAction("PacketDetail", _packetRepository.GetById(Id));
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
            var list = new SelectList(Enum.GetValues(typeof(MealTypeId)).Cast<MealTypeId>().Select(mt => new SelectListItem
            {
                Text = mt.ToString(),
                Value = ((int)mt).ToString(),
            }), "Value", "Text");
            ViewBag.MealTypes = list;

            ViewBag.Days = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Today", Value = "0"},
                new SelectListItem { Text = "Tomorrow", Value = "1"},
                new SelectListItem { Text = "Day after tomorrow", Value = "2"}
            };

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
                    int? canteenId = HttpContext.Session.GetInt32("CanteenId");
                    _logger.LogInformation("id: " + canteenId);
                    if (canteenId != null)
                    {
                        vm.PickUpTimeStart = vm.PickUpTimeStart.AddDays(int.Parse(vm.PickUpDaysFromNow!));
                        vm.PickUpTimeEnd = vm.PickUpTimeEnd.AddDays(int.Parse(vm.PickUpDaysFromNow!));

                        Packet newPacket = new(vm.PacketName!, vm.PickUpTimeStart, vm.PickUpTimeEnd, vm.IsAlcoholic, vm.Price, vm.TypeOfMeal, (int)canteenId);
                        await _packetRepository.AddPacket(newPacket);

                        foreach (int id in ProductIdList)
                        {
                            newPacket.Products.Add(new PacketProduct(newPacket.Id, id));
                        }
                        await _packetRepository.AddProductsToPacket(newPacket.Products);
                        return RedirectToAction("CanteenPackets");
                    }

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error creating packet", e.Message);
                }
            }

            PrefillSelectOptions(vm);

            return View(vm);
        }
    }
}
