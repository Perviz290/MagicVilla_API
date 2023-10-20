using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Dto
{
    public class VillaDTO
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]

        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }


           public VillaDTO(int Id, string Name)
        {
            this.Id = Id;
            this.Name=Name;
        }

        public VillaDTO()
        {

        }








    }
}
