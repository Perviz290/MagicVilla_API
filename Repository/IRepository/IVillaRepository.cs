using MagicVilla_VillaAPI.Model;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository : IGenericRepository<Villa>
    {

        Task<Villa>UpdateAsync(Villa entity);
    

    }
}
