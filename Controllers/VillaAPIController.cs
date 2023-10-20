using MagicVilla_VillaAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController: ControllerBase
    {

        public IEnumerable<Villa>getVilllas()
        {

            return new List<Villa>
            {
                new Villa {Id=1,Name="Miau"},
                new Villa {Id=2,Name="auye"}
            };

        }






    }
}
