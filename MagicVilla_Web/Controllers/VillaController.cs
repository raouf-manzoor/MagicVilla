using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        public VillaController(IVillaService villaService)
        {
            _villaService = villaService;
        }
        public async Task<IActionResult> Index()
        {
            List<VillaDTO> listOfVillas = new();

            var response = await _villaService.GetAllAsync<APIResponse>();

            if (response.IsSuccess)
            {

                listOfVillas = JsonConvert.DeserializeObject<List<VillaDTO>>(response.Result.ToString());
            }

            return View(listOfVillas);
        }

        [HttpGet]
        public IActionResult CreateVilla()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO villaInput)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(villaInput);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(villaInput);
        }
    }
}
