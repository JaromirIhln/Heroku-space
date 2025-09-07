using WorkDays.Data.Models;
using WorkDays.Data.Interfaces;

namespace WorkDays.Data.Repositories
{
    public class WorkDayRepository : BaseRepository<WorkDay>, IWorkDayRepository
    {
        public WorkDayRepository(AppDbContext context) : base(context) { }
    }
}
