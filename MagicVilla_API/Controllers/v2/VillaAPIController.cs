using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Extentions;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace MagicVilla_API.Controllers.v2
{
    //[Route("api/[controller]")] In that case, Controller Name will automatically populated. 
    // We will avoid that approach, In case of change in controller name. we have to update all
    // the client apps which are consuming that endpoint


    // [MapToApiVersion("2.0")] this attribute is used to sepcify version number at method level

    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiVersion("2.0")]
    //[ApiVersion("1.0",Deprecated =true)] // States that this control version is deprecated and no longer maintained
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        #region Fields

        private readonly ILogger<VillaAPIController> _logger;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _villaRepository;
        protected APIResponse _response;

        #endregion

        #region Ctor
        public VillaAPIController(ILogger<VillaAPIController> logger
          , IMapper mapper, IVillaRepository villaRepository
          )
        {
            _logger = logger;
            _mapper = mapper;
            _villaRepository = villaRepository;
            _response = new();
        }
        #endregion

        #region GetVillas

        [HttpGet]
        //[ResponseCache(Duration = 30)] // This will cache response for 30 seconds

        // this will use the cache profile that we have 
        // configured in settings
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name = "occupancyFilter")] int? occupancy,
         [FromQuery] string? searchKey)
        {
            try
            {

                _logger.LogInformation("Getting all villas");

                IEnumerable<Villa> villasList = await _villaRepository.GetAllAsync();

                if (occupancy > 0)
                {
                    villasList = await _villaRepository.GetAllAsync(e => e.Occupancy > occupancy);
                }
                else
                {
                    villasList = await _villaRepository.GetAllAsync();
                }

                if (!searchKey.IsNullOrEmpty())
                {
                    // Using OrdinalIgnorecase to avoid case sensitve match

                    villasList = villasList.Where(e => e.Name.Contains(searchKey, StringComparison.OrdinalIgnoreCase));
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<List<VillaDTO>>(villasList);

                // Adding pagination info in response header
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new { PageNumber = 1, PageSize = 3 }));

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }


        #endregion



        [HttpGet("{id:int}", Name = "GetVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ResponseCache(NoStore =true,Location =ResponseCacheLocation.None)] In this case response will not be cached, if we have this parameters in response cache attribute
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error while retrieving data!");
                    return BadRequest();
                }

                var villa = await _villaRepository.GetAsync(e => e.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Villa not found");
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<VillaDTO>(villa);

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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
        {
            try
            {
                if (await _villaRepository.GetAsync(e => e.Name.ToLower() == villaCreateDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Name already exists");
                    return BadRequest(ModelState);
                }

                if (villaCreateDTO == null)
                {
                    return BadRequest();
                }

                //if (villaDTO.Id > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError);
                //}

                //var maxId = VillaStore.villaList.OrderByDescending(e => e.Id).FirstOrDefault().Id + 1;
                //villaDTO.Id = maxId;

                var villa = _mapper.Map<Villa>(villaCreateDTO);

                //Villa villa = new()
                //{
                //    Amenity = villaCreateDTO.Amenity,
                //    Details = villaCreateDTO.Details,
                //    ImageUrl = villaCreateDTO.ImageUrl,
                //    Name = villaCreateDTO.Name,
                //    Occupancy = villaCreateDTO.Occupancy,
                //    Rate = villaCreateDTO.Rate,
                //    Sqft = villaCreateDTO.Sqft
                //};

                await _villaRepository.CreateAsync(villa);
                //await _db.SaveChangesAsync();
                //VillaStore.villaList.Add(villaDTO);

                //return Ok(villaDTO);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = villa;

                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villa = await _villaRepository.GetAsync(e => e.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }

                await _villaRepository.RemoveAsync(villa);
                //await _db.SaveChangesAsync();

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

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                if (villaUpdateDTO == null || id != villaUpdateDTO.Id)
                {
                    return BadRequest();
                }

                //var villaToUpdate = VillaStore.villaList.FirstOrDefault(e => e.Id == id);

                //if (villaToUpdate == null)
                //{
                //    return NotFound();
                //}

                //villaToUpdate.Name = villaDTO.Name;
                //villaToUpdate.Sqft = villaDTO.Sqft;
                //villaToUpdate.Occupancy = villaDTO.Occupancy;

                var villa = _mapper.Map<Villa>(villaUpdateDTO);

                //Villa villa = new()
                //{
                //    Amenity = villaUpdateDTO.Amenity,
                //    Details = villaUpdateDTO.Details,
                //    ImageUrl = villaUpdateDTO.ImageUrl,
                //    Name = villaUpdateDTO.Name,
                //    Occupancy = villaUpdateDTO.Occupancy,
                //    Rate = villaUpdateDTO.Rate,
                //    Sqft = villaUpdateDTO.Sqft,
                //    Id = villaUpdateDTO.Id
                //};

                await _villaRepository.UpdateAsync(villa);
                //await _db.SaveChangesAsync();

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


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null || id == 0)
                {
                    return BadRequest();
                }

                var villaToUpdate = await _villaRepository.GetAsync(e => e.Id == id, false);

                if (villaToUpdate == null)
                {
                    return NotFound();
                }

                var villaDTO = _mapper.Map<VillaUpdateDTO>(villaToUpdate);

                //VillaUpdateDTO villaDTO = new()
                //{
                //    Amenity = villaToUpdate.Amenity,
                //    Details = villaToUpdate.Details,
                //    ImageUrl = villaToUpdate.ImageUrl,
                //    Name = villaToUpdate.Name,
                //    Occupancy = villaToUpdate.Occupancy,
                //    Rate = villaToUpdate.Rate,
                //    Sqft = villaToUpdate.Sqft,
                //    Id = villaToUpdate.Id,
                //};

                patchDTO.ApplyTo(villaDTO, ModelState);

                var villa = _mapper.Map<Villa>(villaDTO);

                //Villa villa = new()
                //{
                //    Amenity = villaDTO.Amenity,
                //    Details = villaDTO.Details,
                //    ImageUrl = villaDTO.ImageUrl,
                //    Name = villaDTO.Name,
                //    Occupancy = villaDTO.Occupancy,
                //    Rate = villaDTO.Rate,
                //    Sqft = villaDTO.Sqft,
                //    Id = villaDTO.Id,
                //};

                await _villaRepository.UpdateAsync(villa);
                //await _db.SaveChangesAsync();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

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
