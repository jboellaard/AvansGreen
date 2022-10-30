using Core.Domain;
using Core.DomainServices.IRepos;
using Core.DomainServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.AvansGreenApp.Models;

namespace UI.AvansGreenApp.Controllers
{
    public class PacketController : Controller
    {
        private readonly ILogger<PacketController> _logger;
        private readonly IPacketRepository _packetRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICanteenEmployeeRepository _canteenEmployeeRepository;
        private readonly IPacketService _packetService;

        public PacketController(ILogger<PacketController> logger,
            IPacketRepository packetRepository,
            IProductRepository productRepository,
            IStudentRepository studentRepository,
            ICanteenEmployeeRepository canteenEmployeeRepository,
            IPacketService packetService)
        {
            _logger = logger;
            _packetRepository = packetRepository;
            _productRepository = productRepository;
            _studentRepository = studentRepository;
            _canteenEmployeeRepository = canteenEmployeeRepository;
            _packetService = packetService;
        }

        public void PrefillMealTypes()
        {
            var mealTypesList = new SelectList(Enum.GetValues(typeof(MealTypeId)).Cast<MealTypeId>().Select(mt => new SelectListItem
            {
                Text = new MealType() { MealTypeId = mt }.ToString(),
                Value = ((int)mt).ToString(),
            }).Prepend(new SelectListItem() { Text = "All meal types", Value = "0" }), "Value", "Text");
            ViewBag.MealTypes = mealTypesList;
        }

        public IActionResult PacketOverview()
        {
            FilterPacketsViewModel vm = new()
            {
                Packets = _packetRepository.GetPacketsWithoutReservation().Where(p => p.PickUpTimeEnd >= DateTime.Now).ToList()
            };
            if (User.FindFirst("UserType")?.Value is "Student")
            {
                // Filter packets by city of student
                Student student = _studentRepository.GetByStudentNr(User.Identity.Name);
                vm.Packets = vm.Packets.Where(p => p.Canteen.City.ToUpper().Equals(student.CityOfSchool.ToUpper())).ToList();
                vm.CityList.Add(student.CityOfSchool);
            }
            vm.Packets.Sort((x, y) => (x.PickUpTimeEnd).CompareTo(y.PickUpTimeEnd));

            PrefillMealTypes();
            return View(vm);
        }

        [HttpPost]
        public IActionResult FilterPackets(FilterPacketsViewModel vm)
        {
            vm.Packets = _packetRepository.GetPacketsWithoutReservation().Where(p => p.PickUpTimeEnd >= DateTime.Now).ToList();
            if (vm.CityList.Count != 0)
            {
                vm.Packets = vm.Packets.Where(p => vm.CityList.Contains(p.Canteen.City, StringComparer.OrdinalIgnoreCase)).ToList();
            }
            if (vm.TypeOfMeal != 0)
            {
                vm.Packets = vm.Packets.Where(p => p.MealTypeId.Equals(vm.TypeOfMeal)).ToList();
            }

            PrefillMealTypes();
            return View("PacketOverview", vm);
        }

        public IActionResult UndoFilters()
        {
            FilterPacketsViewModel vm = new()
            {
                Packets = _packetRepository.GetPacketsWithoutReservation().Where(p => p.PickUpTimeEnd >= DateTime.Now).ToList()
            };

            PrefillMealTypes();
            return View("PacketOverview", vm);
        }


        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        public IActionResult CanteenPackets()
        {
            CanteenPacketsViewModel vm = new();
            CanteenEmployee canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(User.Identity.Name);
            int? canteenId = canteenEmployee.CanteenId;

            if (canteenId != null)
            {
                vm.Canteen = Canteen.FromId<Canteen>((int)canteenId);
                vm.Packets = _packetRepository.GetPacketsFromCanteen((int)canteenId).ToList();
                vm.Packets.Sort((x, y) => (x.PickUpTimeEnd).CompareTo(y.PickUpTimeEnd));

                return View(vm);
            }
            return View(vm);
        }

        [Authorize(Policy = "OnlyStudentsAndUp")]
        public IActionResult MyReservations()
        {
            Student student = _studentRepository.GetByStudentNr(User.Identity.Name);
            int? studentId = student.Id;

            if (studentId != null)
            {
                List<Packet> packets = _packetRepository.GetPacketsReserverdByStudentWithId((int)studentId).ToList();
                packets.Sort((x, y) => (x.PickUpTimeEnd).CompareTo(y.PickUpTimeEnd));

                return View(packets);
            }
            return View(new List<Packet>());
        }

        public IActionResult PacketDetail(int Id)
        {
            PacketDetailViewModel vm = new() { Packet = _packetRepository.GetById(Id) };
            if (User.FindFirst("UserType")?.Value is "CanteenEmployee")
            {
                CanteenEmployee canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(User.Identity.Name);
                if (canteenEmployee != null)
                {
                    vm.CanEdit = canteenEmployee.CanteenId == vm.Packet.CanteenId;
                }
            }
            return View(vm);
        }

        [Authorize(Policy = "OnlyStudentsAndUp")]
        public IActionResult AddReservation(int Id)
        {
            try
            {
                Packet packet = _packetService.AddReservation(_studentRepository.GetByStudentNr(User.Identity.Name), Id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("ReservationError", e.Message);
                return View("PacketDetail", new PacketDetailViewModel() { Packet = _packetRepository.GetById(Id) });
            }
            return RedirectToAction("MyReservations");
        }

        [Authorize(Policy = "OnlyStudentsAndUp")]
        public IActionResult DeleteReservation(int Id)
        {
            try
            {
                _packetService.DeleteReservation(_studentRepository.GetByStudentNr(User.Identity.Name), Id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("ReservationNotRemoved", e.Message);
                return View("PacketDetail", new PacketDetailViewModel() { Packet = _packetRepository.GetById(Id) });
            }

            return RedirectToAction("MyReservations");
        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        [HttpGet]
        public IActionResult AddPacket()
        {
            CanteenEmployee canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(User.Identity.Name);
            var model = new NewPacketViewModel() { CanteenId = canteenEmployee.CanteenId };

            PrefillSelectOptions(model);

            return View(model);
        }

        private void PrefillSelectOptions(NewPacketViewModel vm)
        {
            Canteen canteen = CanteenEnumerable.FromId<Canteen>(vm.CanteenId);
            var list = Enum.GetValues(typeof(MealTypeId)).Cast<MealTypeId>();
            if (!canteen.HasWarmMeals)
            {
                list = list.Where(mt => !canteen.HasWarmMeals && mt != MealTypeId.WarmMeal);
            }
            var mealTypesList = new SelectList(list.Select(mt => new SelectListItem
            {
                Text = new MealType() { MealTypeId = mt }.ToString(),
                Value = ((int)mt).ToString(),
            }), "Value", "Text"); ;
            ViewBag.MealTypes = mealTypesList;

            var days = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Today", Value = "0"},
                new SelectListItem { Text = "Tomorrow", Value = "1"},
                new SelectListItem { Text = "Day after tomorrow", Value = "2"}
            };
            ViewBag.Days = days;

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
                    int daysToAdd = 0;
                    if (vm.PickUpDaysFromNow != null) daysToAdd = int.Parse(vm.PickUpDaysFromNow);
                    Packet packet = await _packetService.AddPacket(vm.PacketName, daysToAdd, vm.PickUpTimeStart, vm.PickUpTimeEnd, vm.IsAlcoholic, vm.Price, vm.TypeOfMeal, vm.CanteenId, ProductIdList);

                    if (packet != null) return RedirectToAction("CanteenPackets");
                    ModelState.AddModelError("PacketCreationError", "Could not add packet, please try again later.");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("PacketCreationError", e.Message);
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
                return View("PacketDetail", new PacketDetailViewModel() { Packet = _packetRepository.GetById(Id) });
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
                CanteenId = packet.CanteenId
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
                    Packet updatedPacket = _packetService.UpdatePacket(Id, vm.PacketName, int.Parse(vm.PickUpDaysFromNow), vm.PickUpTimeStart, vm.PickUpTimeEnd, vm.IsAlcoholic, vm.Price, vm.TypeOfMeal, vm.CanteenId, ProductIdList);
                    if (updatedPacket != null) return RedirectToAction("CanteenPackets");
                    else ModelState.AddModelError("Error updating packet", "Could not update this packet, please try again later.");
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
            try
            {
                Packet packet = _packetService.DeletePacket(Id);
                if (packet != null) return RedirectToAction("CanteenPackets");
                else ModelState.AddModelError("DeletionError", "Could not delete this packet, please try again later.");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("DeletionError", e.Message);
            }
            return View("PacketDetail", new PacketDetailViewModel() { Packet = _packetRepository.GetById(Id) });
        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        [HttpGet]
        public IActionResult RenewPacket(int Id)
        {
            Packet packet = _packetRepository.GetById(Id)!;
            var model = new NewPacketViewModel()
            {
                PacketName = packet.Name,
                PickUpDaysFromNow = "0",
                PickUpTimeStart = packet.PickUpTimeStart,
                PickUpTimeEnd = packet.PickUpTimeEnd,
                IsAlcoholic = packet.IsAlcoholic,
                Price = packet.Price,
                TypeOfMeal = packet.MealTypeId,
                CanteenId = packet.CanteenId,
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
        public IActionResult RenewPacket(NewPacketViewModel vm, List<int> ProductIdList, int Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Packet updatedPacket = _packetService.RenewPacket(Id, vm.PacketName, int.Parse(vm.PickUpDaysFromNow!), vm.PickUpTimeStart, vm.PickUpTimeEnd, vm.IsAlcoholic, vm.Price, vm.TypeOfMeal, vm.CanteenId, ProductIdList);
                    if (updatedPacket != null) return RedirectToAction("CanteenPackets");
                    else ModelState.AddModelError("Error updating packet", "Could not renew this packet, please try again later.");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error updating packet", e.Message);
                }
            }

            PrefillSelectOptions(vm);

            return View(vm);
        }
    }
}
