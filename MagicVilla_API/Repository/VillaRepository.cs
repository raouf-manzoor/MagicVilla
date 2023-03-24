using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_API.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }




        //public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null)
        //{

        //    var query = _db.Villas;

        //    if (filter != null)
        //        query.Where(filter);

        //    return await query.ToListAsync();
        //}
        //public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        //{
        //    var query = _db.Villas.Where(filter);

        //    if (!tracked)
        //        query = query.AsNoTracking();

        //    return await query.FirstOrDefaultAsync();
        //    //throw new NotImplementedException();
        //}

        //public async Task CreateAsync(Villa entity)
        //{
        //    await _db.Villas.AddAsync(entity);
        //    await SaveAsync();
        //}
        public async Task UpdateAsync(Villa entity)
        {
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            //  await SaveAsync();
        }
        //public async Task RemoveAsync(Villa entity)
        //{
        //    _db.Villas.Remove(entity);
        //    await SaveAsync();
        //}

        //public async Task SaveAsync()
        //{
        //    await _db.SaveChangesAsync();
        //}
    }
}
