using AutoMapper;
using MagicVilla_VillaAPI.Dto;
using MagicVilla_VillaAPI.Model;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {

        public MappingConfig() 
        {
            CreateMap<Villa,VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            CreateMap<Villa,VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();



            CreateMap<VillaNumber, VillaNumberDTO>();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();


        }




    }
}
