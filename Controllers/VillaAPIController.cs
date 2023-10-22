using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Dto;
using MagicVilla_VillaAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        // Use AppDbContext in VillaController 
        private readonly AppDbContext _db;
        public VillaAPIController(AppDbContext db)
        {
            this._db = db;
        }

        //FindAll
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVilllas()
        {  
            return Ok(await _db.Villas.ToListAsync());
        }

        //FindById
        [HttpGet("{id:int}",Name="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id) {

            if (id == 0)
            {        
                return BadRequest();
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);       
        }

        //save to Villa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>>createVilla([FromBody]VillaCreateDTO villaDTO)
        {

            //custom validation-adlar eyni olmasin diye;
            if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                //validation message;
                ModelState.AddModelError("CustomError", "Villa var");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest();
            }
            //if (villaDTO.Id > 0)
            //{
            //    return   StatusCode(StatusCodes.Status500InternalServerError);
            //}
            Villa model = new()
            {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Occupancy = villaDTO.Occupancy,
                ImageUrl = villaDTO.ImageUrl,
                Amenity = villaDTO.Amenity,   
            };
            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVilla",new {id=model.Id},model);
        }

        // Delete to Villa ById
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        public async Task< IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa=await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);  

            if (villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            return NoContent();
        }


        //Put Villa Data
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name ="UpdateVilla")]
        public async Task< IActionResult> UpdateVilla(int id,[FromBody]VillaUpdateDTO updateVillaDTO)
        {
            if (updateVillaDTO == null || id != updateVillaDTO.Id)
            {
                return BadRequest();
            }

            Villa model = new()
            {
                Id = updateVillaDTO.Id,
                Name = updateVillaDTO.Name,
                Details = updateVillaDTO.Details,
                Rate = updateVillaDTO.Rate,
                Sqft = updateVillaDTO.Sqft,
                Occupancy = updateVillaDTO.Occupancy,
                ImageUrl = updateVillaDTO.ImageUrl,
                Amenity = updateVillaDTO.Amenity,

            };
             _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }














         




    }
}
