using WorkDays.Api.Models;
using WorkDays.Data.Models;

namespace WorkDays.Api.Helpers
{
    public static class AppMappingHelper
    {
    public static WorkDayDto MapToDto(WorkDay entity)
        {
            if (entity == null) return null;
            return new WorkDayDto
            {
                WorkDayId = entity.WorkDayId,
                Date = entity.Date,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                Break = entity.Break,
                IsHoliday = entity.IsHoliday,
                Type = (Models.DayType)entity.Type,
                TotalHours = entity.TotalHours
            };
        }

    public static WorkDay MapToEntity(WorkDayDto dto)
        {
            if (dto == null) return null;
            return new WorkDay
            {
                WorkDayId = dto.WorkDayId,
                Date = dto.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Break = dto.Break,
                IsHoliday = dto.IsHoliday,
                Type = (Data.Models.DayType)dto.Type,
                TotalHours = dto.TotalHours
            };
        }
    }
}
