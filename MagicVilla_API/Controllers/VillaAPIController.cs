using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    //[Route("api/[controller]")] In that case, Controller Name will automatically populated. 
    // We will avoid that approach, In case of change in controller name. we have to update all
    // the client apps which are consuming that endpoint
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDbContext _db;

        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error while retrieving data!");
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(e => e.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            if (_db.Villas.FirstOrDefault(e => e.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Name already exists");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                return BadRequest();
            }

            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //var maxId = VillaStore.villaList.OrderByDescending(e => e.Id).FirstOrDefault().Id + 1;
            //villaDTO.Id = maxId;

            Villa villa = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

            _db.Villas.Add(villa);
            _db.SaveChanges();
            //VillaStore.villaList.Add(villaDTO);

            //return Ok(villaDTO);

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villaDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(e => e.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
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

            Villa villa = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Id = villaDTO.Id
            };

            _db.Villas.Update(villa);
            _db.SaveChanges();

            return NoContent();
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villaToUpdate = _db.Villas.AsNoTracking().FirstOrDefault(e => e.Id == id);

            if (villaToUpdate == null)
            {
                return NotFound();
            }

            VillaDTO villaDTO = new()
            {
                Amenity = villaToUpdate.Amenity,
                Details = villaToUpdate.Details,
                ImageUrl = villaToUpdate.ImageUrl,
                Name = villaToUpdate.Name,
                Occupancy = villaToUpdate.Occupancy,
                Rate = villaToUpdate.Rate,
                Sqft = villaToUpdate.Sqft,
                Id = villaToUpdate.Id,
            };

            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa villa = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Id = villaDTO.Id,
            };

            _db.Villas.Update(villa);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}
