using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_API.Controllers
{
    //[Route("api/[controller]")] In that case, Controller Name will automatically populated. 
    // We will avoid that approach, In case of change in controller name. we have to update all
    // the client apps which are consuming that endpoint
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaAPINumberController : ControllerBase
    {
        private readonly ILogger<VillaAPINumberController> _logger;
        private readonly IMapper _mapper;
        private readonly IVillaNumberRepository _villaNumberRepository;
        protected APIResponse _response;

        public VillaAPINumberController(ILogger<VillaAPINumberController> logger
            , IMapper mapper, IVillaNumberRepository villaNumberRepository
            )
        {
            _logger = logger;
            _mapper = mapper;
            _villaNumberRepository = villaNumberRepository;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {

                _logger.LogInformation("Getting all villas");

                var villasList = await _villaNumberRepository.GetAllAsync();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villasList);

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _logger.LogError("Error while retrieving data!");
                    return BadRequest();
                }

                var villa = await _villaNumberRepository.GetAsync(e => e.VillaNo == villaNo);

                if (villa == null)
                {
                    return NotFound();
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<VillaNumberDTO>(villa);

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberCreateDTO)
        {
            try
            {
                if (await _villaNumberRepository.GetAsync(e => e.VillaNo == villaNumberCreateDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa Number already exists");
                    return BadRequest(ModelState);
                }

                if (villaNumberCreateDTO == null)
                {
                    return BadRequest();
                }

                var villaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDTO);


                await _villaNumberRepository.CreateAsync(villaNumber);
                //await _db.SaveChangesAsync();
                //VillaStore.villaList.Add(villaDTO);

                //return Ok(villaDTO);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = villaNumber;

                return CreatedAtRoute("GetVillaNumber", new { villaNo = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpDelete("{villaNo:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    return BadRequest();
                }

                var villa = await _villaNumberRepository.GetAsync(e => e.VillaNo == villaNo);

                if (villa == null)
                {
                    return NotFound();
                }

                await _villaNumberRepository.RemoveAsync(villa);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPut("{villaNo:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int villaNo, [FromBody] VillaNumberUpdateDTO villaNumberUpdateDTO)
        {
            try
            {
                if (villaNumberUpdateDTO == null || villaNo != villaNumberUpdateDTO.VillaNo)
                {
                    return BadRequest();
                }

                var villaNumber = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);

                await _villaNumberRepository.UpdateAsync(villaNumber);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }


    }
}
