using System;
using System.Collections.Generic;
using System.Text;

namespace Blogifier.Core.Data.Repositories
{
    public interface IStatsUniqueRepository : IRepository<StatsUnique>
    {
    }

    public class StatsUniqueRepository : Repository<StatsUnique>, IStatsUniqueRepository
    {
        AppDbContext _db;

        public StatsUniqueRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
