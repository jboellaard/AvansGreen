using Core.Domain;
using Core.DomainServices.IRepos;
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

        public List<Packet> GetPacketListByUser(FilterPacketsViewModel vm)
        {
            List<Packet> packets = new List<Packet>();
            if (User.FindFirst("UserType")?.Value is "CanteenEmployee" or "Admin")
            {
                packets = _packetRepository.GetPackets().Where(p => p.PickUpTimeEnd >= DateTime.Now.AddDays(-1)).ToList();
            }
            else if (User.FindFirst("UserType")?.Value is "Student")
            {
                Student student = _studentRepository.GetByStudentNr(User.Identity.Name);
                // Filter list by location of student
                packets = _packetRepository.GetPacketsWithoutReservation().Where(p => p.PickUpTimeEnd >= DateTime.Now && p.Canteen.City.ToUpper().Equals(student.CityOfSchool.ToUpper())).ToList();
                // Add select options of city to the list
                foreach (Canteen canteen in CanteenEnumerable.GetAll<Canteen>())
                {
                    if (canteen.City.ToUpper().Equals(student.CityOfSchool.ToUpper())) vm.CanteenIdList.Add(canteen.Id);
                }
            }
            else packets = _packetRepository.GetPacketsWithoutReservation().Where(p => p.PickUpTimeEnd >= DateTime.Now).ToList();

            packets.Sort((x, y) => (x.PickUpTimeEnd).CompareTo(y.PickUpTimeEnd));
            return packets;
        }

        public void PrefillPacketOverview()
        {
            var mealTypesList = new SelectList(Enum.GetValues(typeof(MealTypeId)).Cast<MealTypeId>().Select(mt => new SelectListItem
            {
                Text = System.Text.RegularExpressions.Regex.Replace(mt.ToString(), "([A-Z])", " $1").Trim(),
                Value = ((int)mt).ToString(),
            }).Prepend(new SelectListItem() { Text = "All meal types", Value = "0" }), "Value", "Text");
            ViewBag.MealTypes = mealTypesList;
        }

        public IActionResult PacketOverview()
        {
            FilterPacketsViewModel vm = new();
            vm.Packets = GetPacketListByUser(vm);
            PrefillPacketOverview();
            return View(vm);
        }

        [HttpPost]
        public IActionResult FilterPackets(FilterPacketsViewModel vm)
        {

            foreach (int id in vm.CanteenIdList)
            {

            }
            return View();
        }


        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        public IActionResult CanteenPackets()
        {
            CanteenPacketsViewModel vm = new CanteenPacketsViewModel();
            CanteenEmployee? canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(User.Identity.Name);
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
            Student? student = _studentRepository.GetByStudentNr(User.Identity.Name);
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
            CanteenEmployee? canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(User.Identity.Name);
            Packet? packet = _packetRepository.GetById(Id);
            PacketDetailViewModel vm = new() { Packet = packet };
            if (canteenEmployee.CanteenId == packet.CanteenId) vm.CanEdit = true;
            return View(vm);
        }

        [Authorize(Policy = "OnlyStudentsAndUp")]
        public IActionResult AddReservation(int Id)
        {
            _logger.LogInformation("Inside addreservation");
            Student? student = _studentRepository.GetByStudentNr(User.Identity.Name);
            if (student != null)
            {

                Packet packet = _packetRepository.GetById(Id)!;
                if (!(packet.PickUpTimeEnd.Date < student.DateOfBirth.AddYears(18).Date && packet.IsAlcoholic))
                {
                    _logger.LogInformation("Made student and packet: " + student.Id + ", " + packet.Id);
                    foreach (Packet reservedPacket in student.ReservedPackets)
                    {
                        if (reservedPacket.PickUpTimeStart.Date.Equals(packet.PickUpTimeStart.Date))
                        {
                            _logger.LogInformation("Already has a reservation on this day");
                            ModelState.AddModelError("OneReservationPerDay", "You cannot make more than one reservation per day.");
                            return View("PacketDetail", new PacketDetailViewModel() { Packet = _packetRepository.GetById(Id) });
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("18PlusPackage", "You are too young to reserve a packet with alcohol.");
                }

                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Attempting to add reservation to db");
                    packet.StudentId = student.Id;
                    Packet? updatedPacket = _packetRepository.AddReservationToPacket(packet);
                    if (updatedPacket == null) ModelState.AddModelError("UnableToAddReservation", "Could not add reservation, please try again later.");
                    else return RedirectToAction("MyReservations");
                }
            }
            _logger.LogInformation("Something went wrong, returning view again");
            return View("PacketDetail", new PacketDetailViewModel() { Packet = _packetRepository.GetById(Id) });
        }

        [Authorize(Policy = "OnlyStudentsAndUp")]
        public IActionResult DeleteReservation(int Id)
        {
            _logger.LogInformation("Inside deletereservation");
            Student? student = _studentRepository.GetByStudentNr(User.Identity.Name);
            if (student != null)
            {
                Packet? packet = _packetRepository.DeleteReservation(Id);
                if (packet != null)
                {
                    List<Packet> packets = _packetRepository.GetPacketsReserverdByStudentWithId(student.Id).ToList();
                    packets.Sort((x, y) => (x.PickUpTimeEnd).CompareTo(y.PickUpTimeEnd));
                    return View("MyReservations", packets);
                }
            }
            ModelState.AddModelError("ReservationNotRemoved", "Reservation could not be cancelled, please try again later");
            return View("PacketDetail", new PacketDetailViewModel() { Packet = _packetRepository.GetById(Id) });
        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        [HttpGet]
        public IActionResult AddPacket()
        {
            CanteenEmployee? canteenEmployee = _canteenEmployeeRepository.GetByEmployeeNr(User.Identity.Name);
            var model = new NewPacketViewModel() { CanteenId = canteenEmployee.CanteenId };

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
                canteen = CanteenEnumerable.FromId<Canteen>((int)canteenId);
                //canteen = _canteenRepository.GetById((int)canteenId)!;
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
                    vm.PickUpTimeStart = vm.PickUpTimeStart.AddDays(int.Parse(vm.PickUpDaysFromNow!));
                    vm.PickUpTimeEnd = vm.PickUpTimeEnd.AddDays(int.Parse(vm.PickUpDaysFromNow!));

                    Packet newPacket = new(vm.PacketName!, vm.PickUpTimeStart, vm.PickUpTimeEnd, vm.IsAlcoholic, vm.Price, vm.TypeOfMeal, vm.CanteenId);
                    newPacket.Id = Id;
                    _logger.LogInformation("empty list? : " + newPacket.Products.Count);
                    foreach (int productId in ProductIdList)
                    {
                        _logger.LogInformation("product" + productId);
                        newPacket.Products.Add(new PacketProduct(newPacket.Id, productId));
                    }
                    _logger.LogInformation("canteenid" + vm.CanteenId);
                    Packet? updatedPacket = _packetRepository.UpdatePacket(newPacket);
                    return RedirectToAction("CanteenPackets");
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
            if (packet.StudentId.HasValue && packet.PickUpTimeEnd >= DateTime.Now)
            {
                ModelState.AddModelError("AlreadyReservedError", "This packet has a reservation and cannot be removed");
                return View("PacketDetail", new PacketDetailViewModel() { Packet = _packetRepository.GetById(Id) });
            }
            else
            {
                packet = _packetRepository.DeletePacket(packet.Id);
                if (packet == null)
                {
                    ModelState.AddModelError("AlreadyReservedError", "Packet could not be removed, please try again later");
                    return View("PacketDetail", new PacketDetailViewModel() { Packet = _packetRepository.GetById(Id), CanEdit = true });
                }
            }
            return RedirectToAction("CanteenPackets");
        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        [HttpGet]
        public IActionResult RenewPacket(int Id)
        {
            Packet packet = _packetRepository.GetById(Id)!;
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
        public IActionResult RenewPacket(NewPacketViewModel vm, List<int> ProductIdList, int Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    vm.PickUpTimeStart = vm.PickUpTimeStart.AddDays(int.Parse(vm.PickUpDaysFromNow!));
                    vm.PickUpTimeEnd = vm.PickUpTimeEnd.AddDays(int.Parse(vm.PickUpDaysFromNow!));

                    Packet newPacket = new(vm.PacketName!, vm.PickUpTimeStart, vm.PickUpTimeEnd, vm.IsAlcoholic, vm.Price, vm.TypeOfMeal, vm.CanteenId);
                    newPacket.Id = Id;
                    foreach (int productId in ProductIdList)
                    {
                        newPacket.Products.Add(new PacketProduct(newPacket.Id, productId));
                    }
                    Packet? updatedPacket = _packetRepository.UpdatePacket(newPacket);
                    return RedirectToAction("CanteenPackets");

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
