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
    /// <summary>
    /// Represents different departments/roles in the workplace.
    /// </summary>
    public enum Department
    {
        None,           // Žádné oddìlení
        Stavba,         // Stavba - zelená
        PickUp,         // Pick-up - modrá
        Sanita,         // Sanita - žlutá (vaše nejoblíbenìjší)
        Pila            // Pila/Pøíøez - èervená
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
        public Department Department { get; set; } = Department.None;
        public TimeOnly TotalHours { get; set; }
    }
}
