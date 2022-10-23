﻿using AvansGreen.WebApp.Models;
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
            var model = new NewPacketViewModel() { AllProducts = _productRepository.GetProducts().ToList() };

            PrefillSelectOptions();

            return View(model);
        }

        private void PrefillSelectOptions()
        {
            var list = new SelectList(Enum.GetValues(typeof(MealType)).Cast<MealType>().Select(mt => new SelectListItem
            {
                Text = mt.ToString(),
                Value = ((int)mt).ToString(),
            }), "Value", "Text");
            ViewBag.MealTypes = list;
        }

        [Authorize(Policy = "OnlyCanteenEmployeesAndUp")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddPacket(NewPacketViewModel vm)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    CanteenEmployee? canteenEmployee = _canteenEmployeeRepository.GetByEmail(User.Identity!.Name!.ToLower());
                    if (canteenEmployee != null)
                    {
                        Packet newPacket = new(vm.Name!, vm.PickUpTimeStart, vm.PickUpTimeEnd, vm.IsAlcoholic, vm.Price, vm.TypeOfMeal, canteenEmployee);

                        await _packetRepository.AddPacket(newPacket);
                        _logger.LogInformation("Id " + newPacket.Id);
                        //TODO: get products from checkboxes
                        Product product = _productRepository.GetById(1)!;
                        newPacket.Products.Add(new PacketProduct(newPacket.Id, product.Id));
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

            PrefillSelectOptions();

            return View(vm);
        }
    }
}
