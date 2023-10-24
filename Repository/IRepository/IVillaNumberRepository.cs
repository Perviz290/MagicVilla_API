using MagicVilla_VillaAPI.Model;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaNumberRepository : IGenericRepository<VillaNumber>
    {

        Task<VillaNumber> UpdateAsync(VillaNumber entity);


    }
}
