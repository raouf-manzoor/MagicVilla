using AutoMapper;
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
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
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

        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);

            if (response.IsSuccess)
            {

                var villaDto = JsonConvert.DeserializeObject<VillaDTO>(response.Result.ToString());
                return View(_mapper.Map<VillaUpdateDTO>(villaDto));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villaInput)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<APIResponse>(villaInput);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(villaInput);
        }

        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);

            if (response.IsSuccess)
            {

                if (response.IsSuccess)
                {

                    var villaDto = JsonConvert.DeserializeObject<VillaDTO>(response.Result.ToString());
                    return View(villaDto);
                }
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO villaInput)
        {

            var response = await _villaService.DeleteAsync<APIResponse>(villaInput.Id);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return View(villaInput);
        }
    }
}
