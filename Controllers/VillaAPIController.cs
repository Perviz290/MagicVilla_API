using AutoMapper;
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
        private readonly IMapper _mapper;
        public VillaAPIController(AppDbContext db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
        }

        //FindAll
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVilllas()
        {
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync(); 
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
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
            return Ok(_mapper.Map<VillaDTO>(villa));       
        }

        //save to Villa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>>createVilla([FromBody]VillaCreateDTO createDTO)
        {
            //custom validation-adlar eyni olmasin diye;
            if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                //validation message;
                ModelState.AddModelError("CustomError", "Villa var");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return BadRequest();
            }
            //if (villaDTO.Id > 0)
            //{
            //    return   StatusCode(StatusCodes.Status500InternalServerError);
            //}

            Villa model = _mapper.Map<Villa>(createDTO);

            //Qeyd AutoMapper istifade edilenden sonra artiq veriableri bir bir yazmag lazim deyil!
            //Villa model = new()
            //{
            //    Name = createDTO.Name,
            //    Details = createDTO.Details,
            //    Rate = createDTO.Rate,
            //    Sqft = createDTO.Sqft,
            //    Occupancy = createDTO.Occupancy,
            //    ImageUrl = createDTO.ImageUrl,
            //    Amenity = createDTO.Amenity,   
            //};

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

            Villa model = _mapper.Map<Villa>(updateVillaDTO);

             _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }














         




    }
}
