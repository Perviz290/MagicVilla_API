using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : GenericRepository<VillaNumber>, IVillaNumberRepository
    {
        private readonly AppDbContext _db;
        public VillaNumberRepository(AppDbContext db) : base(db) 
        {
            _db = db;
        }



        public async Task<VillaNumber>UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }











    }
}
