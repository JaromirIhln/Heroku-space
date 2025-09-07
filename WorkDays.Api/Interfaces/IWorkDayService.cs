using WorkDays.Api.Models;

namespace WorkDays.Api.Interfaces
{
    public interface IWorkDayService
    {
        Task<IEnumerable<WorkDayDto>> GetAllWorkDaysAsync();
        Task<WorkDayDto> GetWorkDayByIdAsync(int id);
        Task<WorkDayDto> AddWorkDayAsync(WorkDayDto workDayDto);
        Task<WorkDayDto> UpdateWorkDayAsync(WorkDayDto workDayDto);
        Task DeleteWorkDayAsync(int id);
    }
}
