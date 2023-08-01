using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService, IMapper mapper)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<VillaNumberDTO> listOfVillas = new();

            var response = await _villaNumberService.GetAllAsync<APIResponse>();

            if (response.IsSuccess)
            {

                listOfVillas = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(response.Result.ToString());
            }

            return View(listOfVillas);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {

            VillaNumberCreateVM villNumberVM = new();

            var response = await _villaService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSuccess)
            {

                villNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(response.Result.ToString())
                    .Select(e => new SelectListItem()
                    {
                        Text = e.Name,
                        Value = e.Id.ToString()
                    });
            }

            return View(villNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM villaNumberInput)
        {

            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(villaNumberInput.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(villaNumberInput);
        }
    }
}
