using WorkDays.Api.Interfaces;
using WorkDays.Api.Models;
using WorkDays.Data;
using WorkDays.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkDays.Api.Services
{
    public class WorkDayService : IWorkDayService
    {
        private readonly AppDbContext _dbContext;

        public WorkDayService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<WorkDayDto>> GetAllWorkDaysAsync()
        {
            return await _dbContext.WorkDays
                .Select(w => new WorkDayDto
                {
                    WorkDayId = w.WorkDayId,
                    Date = w.Date,
                    StartTime = w.StartTime,
                    EndTime = w.EndTime,
                    Break = w.Break,
                    IsHoliday = w.IsHoliday,
                    Type = (Models.DayType)w.Type,
                    TotalHours = w.TotalHours
                })
                .ToListAsync();
        }

        public async Task<WorkDayDto> GetWorkDayByIdAsync(int id)
        {
            var w = await _dbContext.WorkDays.FindAsync(id);
            if (w == null) return null;
            return new WorkDayDto
            {
                WorkDayId = w.WorkDayId,
                Date = w.Date,
                StartTime = w.StartTime,
                EndTime = w.EndTime,
                Break = w.Break,
                IsHoliday = w.IsHoliday,
                Type = (Models.DayType)w.Type,
                TotalHours = w.TotalHours
            };
        }

        public async Task<WorkDayDto> AddWorkDayAsync(WorkDayDto dto)
        {
            var specify = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc);
            var entity = new WorkDay
            {
                
                Date = specify,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Break = dto.Break,
                IsHoliday = dto.IsHoliday,
                Type = (Data.Models.DayType)dto.Type,
                TotalHours = dto.TotalHours
            };
            _dbContext.WorkDays.Add(entity);
            await _dbContext.SaveChangesAsync();
            dto.WorkDayId = entity.WorkDayId;
            return dto;
        }

        public async Task<WorkDayDto> UpdateWorkDayAsync(WorkDayDto dto)
        {
            var entity = await _dbContext.WorkDays.FindAsync(dto.WorkDayId);
            if (entity == null) return null;
            dto.Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc);
            entity.Date =  dto.Date;
            entity.StartTime = dto.StartTime;
            entity.EndTime = dto.EndTime;
            entity.Break = dto.Break;
            entity.IsHoliday = dto.IsHoliday;
            entity.Type = (Data.Models.DayType)dto.Type;
            entity.TotalHours = dto.TotalHours;
            await _dbContext.SaveChangesAsync();
            return dto;
        }

        public async Task DeleteWorkDayAsync(int id)
        {
            var entity = await _dbContext.WorkDays.FindAsync(id);
            if (entity != null)
            {
                _dbContext.WorkDays.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
