using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkDays.Data.Models;

namespace WorkDays.Data.Models
{
    /// <summary>
    /// Helper class for department-related operations.
    /// </summary>
    public static class DepartmentExtensions
    {
        /// <summary>
        /// Gets the display name for the department.
        /// </summary>
        /// <param name="department">The department</param>
        /// <returns>Display name in Czech</returns>
        public static string GetDisplayName(this Department department)
        {
            return department switch
            {
                Department.Stavba => "Stavba",
                Department.PickUp => "Pick-up",
                Department.Sanita => "Sanita",
                Department.Pila => "Pila - Přířez",
                Department.None => "Bez oddělení",
                _ => department.ToString()
            };
        }

        /// <summary>
        /// Gets the Bootstrap badge class for the specified department.
        /// </summary>
        /// <param name="department">The department</param>
        /// <returns>Bootstrap CSS class for the badge</returns>
        public static string GetBadgeColor(this Department department)
        {
            return department switch
            {
                Department.Stavba => "success",    // zelená
                Department.PickUp => "primary",    // modrá
                Department.Sanita => "warning",    // žlutá
                Department.Pila => "danger",       // červená
                Department.None => "secondary",    // šedá
                _ => "secondary"
            };
        }

        /// <summary>
        /// Gets the hex color code for the specified department.
        /// </summary>
        /// <param name="department">The department</param>
        /// <returns>Hex color code</returns>
        public static string GetHexColor(this Department department)
        {
            return department switch
            {
                Department.Stavba => "#28a745",    // zelená
                Department.PickUp => "#007bff",    // modrá
                Department.Sanita => "#ffc107",    // žlutá
                Department.Pila => "#dc3545",      // červená
                Department.None => "#6c757d",      // šedá
                _ => "#6c757d"
            };
        }
    }
}