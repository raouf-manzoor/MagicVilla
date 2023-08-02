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
        #region Fields
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        #endregion

        #region Ctor
        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService, IMapper mapper)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;
            _mapper = mapper;
        }
        #endregion

        #region VillaNo List
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
        #endregion

        #region VillaNo Create
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
                    TempData["success"] = "VillaNumber successfully created";
                    return RedirectToAction("Index");
                }
                else if (response.ErrorMessages?.Count > 0)
                {
                    TempData["error"] = "Error in VillaNumber Creation";
                    ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                }
            }
            var villaResponse = await _villaService.GetAllAsync<APIResponse>();
            if (villaResponse != null && villaResponse.IsSuccess)
            {
                villaNumberInput.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(villaResponse.Result.ToString())
                        .Select(e => new SelectListItem()
                        {
                            Text = e.Name,
                            Value = e.Id.ToString()
                        });
            }


            return View(villaNumberInput);
        }
        #endregion

        #region VillaNo Update
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {

            VillaNumberUpdateVM villNumberUpdateVM = new();
            var villaNumberResponse = await _villaNumberService.GetAsync<APIResponse>(villaNo);

            if (villaNumberResponse.IsSuccess)
            {

                var villaDto = JsonConvert.DeserializeObject<VillaNumberDTO>(villaNumberResponse.Result.ToString());
                villNumberUpdateVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(villaDto);
            }

            var villaResponse = await _villaService.GetAllAsync<APIResponse>();

            if (villaResponse != null && villaResponse.IsSuccess)
            {

                villNumberUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(villaResponse.Result.ToString())
                    .Select(e => new SelectListItem()
                    {
                        Text = e.Name,
                        Value = e.Id.ToString()
                    });
            }

            //return NotFound();

            //var response = await _villaService.GetAllAsync<APIResponse>();

            //if (response != null && response.IsSuccess)
            //{

            //    villNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(response.Result.ToString())
            //        .Select(e => new SelectListItem()
            //        {
            //            Text = e.Name,
            //            Value = e.Id.ToString()
            //        });
            //}

            return View(villNumberUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM villaNumberInput)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(villaNumberInput.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(villaNumberInput);
        }
        #endregion

        #region VillaNo Delete
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {

            VillaNumberDeleteVM villNumberDeleteVM = new();
            var villaNumberResponse = await _villaNumberService.GetAsync<APIResponse>(villaNo);

            if (villaNumberResponse.IsSuccess)
            {

                villNumberDeleteVM.VillaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(villaNumberResponse.Result.ToString());
            }

            var villaResponse = await _villaService.GetAllAsync<APIResponse>();

            if (villaResponse != null && villaResponse.IsSuccess)
            {

                villNumberDeleteVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(villaResponse.Result.ToString())
                    .Select(e => new SelectListItem()
                    {
                        Text = e.Name,
                        Value = e.Id.ToString()
                    });
            }

            return View(villNumberDeleteVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM villaNumberInput)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.DeleteAsync<APIResponse>(villaNumberInput.VillaNumber.VillaNo);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(villaNumberInput);
        }
        #endregion


    }
}
