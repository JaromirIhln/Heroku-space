using System;

namespace WorkDays.Api.Models
{
    public enum DayType
    {
        Regular,
        ShortWorkDay,
        Holiday,
        SickLeave,
        Vacation
    }

    public class WorkDayDto
    {
        public int WorkDayId { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public TimeOnly Break { get; set; }
        public bool IsHoliday { get; set; } = false;
        public DayType Type { get; set; } = DayType.Regular;
        public TimeOnly TotalHours { get; set; }
    }
}
