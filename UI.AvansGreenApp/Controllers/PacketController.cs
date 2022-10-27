using UI.AvansGreenApp.Models;
using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.AvansGreenApp.Controllers
{
    public class PacketController : Controller
    {
        private readonly ILogger<PacketController> _logger;
        private readonly IPacketRepository _packetRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICanteenEmployeeRepository _canteenEmployeeRepository;
        private readonly ICanteenRepository _canteenRepository;

        public PacketController(ILogger<PacketController> logger,
            IPacketRepository packetRepository,
            IProductRepository productRepository,
            IStudentRepository studentRepository,
            ICanteenEmployeeRepository canteenEmployeeRepository,
            ICanteenRepository canteenRepository)
        {
            _logger = logger;
            _packetRepository = packetRepository;
            _productRepository = productRepository;
            _studentRepository = studentRepository;
            _canteenEmployeeRepository = canteenEmployeeRepository;
            _canteenRepository = canteenRepository;
        }
        public IActionResult PacketOverview()
        {
            return View(_packetRepository.GetPacketsWithoutReservation().ToList());
        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        public IActionResult CanteenPackets()
        {
            CanteenEmployee? canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(User.Identity.Name);
            int? canteenId = canteenEmployee.CanteenId;
            if (canteenId != null) return View("PacketOverview", _packetRepository.GetPacketsFromCanteen((int)canteenId).ToList());
            return View("PacketOverview", new List<Packet>());
        }

        [Authorize(Policy = "OnlyStudentsAndUp")]
        public IActionResult MyReservations()
        {
            Student? student = _studentRepository.GetByStudentNr(User.Identity.Name);
            int? studentId = student.Id;
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
            Student? student = _studentRepository.GetByStudentNr(User.Identity.Name);
            if (student != null)
            {
                Packet packet = _packetRepository.GetById(Id)!;
                _logger.LogInformation("Made student and packet: " + student.Id + ", " + packet.Id);
                foreach (Packet reservedPacket in student.ReservedPackets)
                {
                    if (reservedPacket.PickUpTimeStart.Date.Equals(packet.PickUpTimeStart.Date))
                    {
                        _logger.LogInformation("Already has a reservation on this day");
                        ModelState.AddModelError("OneReservationPerDay", "You cannot make more than one reservation per day.");
                        return View("PacketDetail", _packetRepository.GetById(Id));
                    }
                }
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Attempting to add reservation to db");
                    packet.StudentId = student.Id;
                    _packetRepository.AddReservationToPacket(packet);
                    return RedirectToAction("MyReservations");
                }
            }
            _logger.LogInformation("Something went wrong, returning view again");
            return View("PacketDetail", _packetRepository.GetById(Id));
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
            Canteen? canteen = null;
            try
            {
                CanteenEmployee? canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(User.Identity.Name);
                int? canteenId = canteenEmployee.CanteenId;
                canteen = _canteenRepository.GetById((int)canteenId)!;
                _logger.LogInformation("Canteen name: " + canteen.Name);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("CanteenNotFound", "Could not get your canteen location");
            }

            var list = Enum.GetValues(typeof(MealTypeId)).Cast<MealTypeId>();
            if (!canteen.HasWarmMeals) list = Enum.GetValues(typeof(MealTypeId)).Cast<MealTypeId>().Where(mt => !canteen.HasWarmMeals && mt != MealTypeId.WarmMeal);
            var mealTypesList = new SelectList(list.Select(mt => new SelectListItem
            {
                Text = System.Text.RegularExpressions.Regex.Replace(mt.ToString(), "([A-Z])", " $1").Trim(),
                Value = ((int)mt).ToString(),
            }), "Value", "Text");
            _logger.LogInformation("After select items." + mealTypesList.Count());
            ViewBag.MealTypes = mealTypesList;

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
                    CanteenEmployee? canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(User.Identity.Name);
                    int? canteenId = canteenEmployee.CanteenId;
                    _logger.LogInformation("id: " + canteenId);
                    //check if canteen can serve warm meals -> before populating?
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


        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        [HttpGet]
        public IActionResult EditPacket(int Id)
        {
            Packet packet = _packetRepository.GetById(Id)!;
            if (packet.StudentId.HasValue)
            {
                ModelState.AddModelError("AlreadyReservedError", "This packet has a reservation and cannot be edited");
                return View("PacketDetail", _packetRepository.GetById(Id));
            }
            var model = new NewPacketViewModel()
            {
                PacketName = packet.Name,
                PickUpDaysFromNow = ((packet.PickUpTimeStart.Date - DateTime.Now.Date).Days).ToString(),
                PickUpTimeStart = packet.PickUpTimeStart,
                PickUpTimeEnd = packet.PickUpTimeEnd,
                IsAlcoholic = packet.IsAlcoholic,
                Price = packet.Price,
                TypeOfMeal = packet.MealTypeId,

            };
            foreach (PacketProduct packetProduct in packet.Products)
            {
                model.ProductIdList.Add(packetProduct.ProductId);
            }

            PrefillSelectOptions(model);

            return View(model);
        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditPacket(NewPacketViewModel vm, List<int> ProductIdList, int Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CanteenEmployee? canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(User.Identity.Name);
                    int? canteenId = canteenEmployee.CanteenId;
                    _logger.LogInformation("id: " + canteenId);
                    if (canteenId != null)
                    {
                        vm.PickUpTimeStart = vm.PickUpTimeStart.AddDays(int.Parse(vm.PickUpDaysFromNow!));
                        vm.PickUpTimeEnd = vm.PickUpTimeEnd.AddDays(int.Parse(vm.PickUpDaysFromNow!));

                        Packet newPacket = new(vm.PacketName!, vm.PickUpTimeStart, vm.PickUpTimeEnd, vm.IsAlcoholic, vm.Price, vm.TypeOfMeal, (int)canteenId);
                        newPacket.Id = Id;
                        foreach (int productId in ProductIdList)
                        {
                            newPacket.Products.Add(new PacketProduct(newPacket.Id, productId));
                        }
                        Packet? updatedPacket = _packetRepository.UpdatePacket(newPacket);
                        return RedirectToAction("CanteenPackets");
                    }

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error updating packet", e.Message);
                }
            }

            PrefillSelectOptions(vm);

            return View(vm);
        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        public IActionResult DeletePacket(int Id)
        {
            Packet packet = _packetRepository.GetById(Id)!;
            if (packet.StudentId.HasValue)
            {
                ModelState.AddModelError("AlreadyReservedError", "This packet has a reservation and cannot be removed");
                return View("PacketDetail", _packetRepository.GetById(Id));
            }
            _logger.LogInformation("After if");
            return RedirectToAction("CanteenPackets");
        }
    }
}
