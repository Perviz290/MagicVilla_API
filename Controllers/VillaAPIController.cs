using AutoMapper;
using Azure;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Dto;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        // Use AppDbContext in VillaController 
        private readonly APIResponse _response;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            this._dbVilla = dbVilla;
            this._mapper = mapper;
            this._response = new();
        }

        //FindAll
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVilllas()
        {
            try
            {
                IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
                _response.Result = (_mapper.Map<List<VillaDTO>>(villaList));
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.InSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        //FindById
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.InSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        //save to Villa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> createVilla([FromBody] VillaCreateDTO createDTO)
        {

            try
            {
                //custom validation-adlar eyni olmasin diye;
                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
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

                //bu method asagidaki iki methodun isini gorur
                await _dbVilla.CreateAsync(model);
                //await _db.Villas.AddAsync(model);
                //await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<VillaDTO>(model);
                _response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.InSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // Delete to Villa ById
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }

                // Qeyd bu method asagidaki iki methodun isini gorur
                await _dbVilla.RemoveAsync(villa);
                //_db.Villas.Remove(villa);
                //await _db.SaveChangesAsync();

                _response.statusCode = HttpStatusCode.NoContent;
                _response.InSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.InSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        //Put Villa Data
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateVillaDTO)
        {
            try
            {
                if (updateVillaDTO == null || id != updateVillaDTO.Id)
                {
                    return BadRequest();
                }

                Villa model = _mapper.Map<Villa>(updateVillaDTO);

                // Qeyd bu method asagidaki iki methodun isini gorur
                await _dbVilla.UpdateAsync(model);
                //   _db.Villas.Update(model);
                //await _db.SaveChangesAsync();

                _response.statusCode = HttpStatusCode.NoContent;
                _response.InSuccess = true;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.InSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }



















    }
}
