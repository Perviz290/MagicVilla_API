using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Dto;
using MagicVilla_VillaAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        //FindAll
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVilllas()
        {
            return Ok(VillaStone.villaList);
        }


        //FindById
        [HttpGet("{id:int}",Name="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id) {

            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStone.villaList.FirstOrDefault(u => u.Id == id);
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
        public ActionResult<VillaDTO>createVilla([FromBody]VillaDTO villaDTO)
        {

            //custom validation-adlar eyni olmasin diye;
            if (VillaStone.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa var");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest();
            }
            if (villaDTO.Id > 0)
            {
                return   StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDTO.Id = VillaStone.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStone.villaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla",new {id=villaDTO.Id},villaDTO);
        }

        // Delete to Villa ById
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            VillaDTO villa=VillaStone.villaList.FirstOrDefault(u => u.Id == id);  

            if (villa == null)
            {
                return NotFound();
            }

            VillaStone.villaList.Remove(villa);
            return NoContent();
        }



        //Put Villa Data
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name ="UpdateVilla")]
        public IActionResult UpdateVilla(int id,[FromBody]VillaDTO updateVillaDTO)
        {
            if (updateVillaDTO == null || id != updateVillaDTO.Id)
            {
                return BadRequest();
            }
            VillaDTO villa = VillaStone.villaList.FirstOrDefault(i=>i.Id==id);
            villa.Name = updateVillaDTO.Name;
            villa.Occupancy = updateVillaDTO.Occupancy;
            villa.Sqft = updateVillaDTO.Sqft;
            return NoContent();
        }














         




    }
}
