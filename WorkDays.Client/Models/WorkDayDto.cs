


namespace WorkDays.Client.Models
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
    /// Represents a workday with details such as date, working hours, break duration, and type of day.
    /// </summary>
    /// <remarks>This data transfer object (DTO) is used to encapsulate information about a single workday, 
    /// including its unique identifier, start and end times, break duration, and whether it is a holiday. It also
    /// includes the total hours worked and the type of day (e.g., regular, holiday, etc.).</remarks>
    public class WorkDayDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the workday.
        /// </summary>
        public int WorkDayId { get; set; }
        /// <summary>
        /// Gets or sets the date associated with this instance.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the start time of the event.
        /// </summary>
        public TimeOnly StartTime { get; set; }
        /// <summary>
        /// Gets or sets the end time of the event or operation.
        /// </summary>
        public TimeOnly EndTime { get; set; }
        /// <summary>
        /// Gets or sets the time at which the break occurs.
        /// </summary>
        public TimeOnly Break { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the current date is a holiday.
        /// </summary>
        public bool IsHoliday { get; set; } = false;
        /// <summary>
        /// Gets or sets the type of day represented by this instance.
        /// </summary>
        public DayType Type { get; set; } = DayType.Regular;
        /// <summary>
        /// Gets or sets the total hours represented as a <see cref="TimeOnly"/> value.
        /// </summary>
        public TimeOnly TotalHours { get; set; }
    }
}
