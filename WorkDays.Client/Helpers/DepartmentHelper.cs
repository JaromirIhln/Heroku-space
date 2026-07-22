using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkDays.Client.Models;

namespace WorkDays.Client.Helpers
{
    /// <summary>
    /// Helper class for department-related operations.
    /// </summary>
    public static class DepartmentHelper
    {
        /// <summary>
        /// Gets the Bootstrap badge class for the specified department.
        /// </summary>
        /// <param name="department">The department</param>
        /// <returns>Bootstrap CSS class for the badge</returns>
        public static string GetBadgeClass(Department department)
        {
            return department switch
            {
                Department.Stavba => "bg-success",      // Zelená
                Department.PickUp => "bg-primary",      // Modrá
                Department.Sanita => "bg-warning",      // Žlutá
                Department.Pila => "bg-danger",         // Červená
                _ => "bg-secondary"                     // Šedá pro žádné oddělení
            };
        }

        /// <summary>
        /// Gets the text color class for the specified department badge.
        /// </summary>
        /// <param name="department">The department</param>
        /// <returns>Bootstrap text color class</returns>
        public static string GetTextClass(Department department)
        {
            return department switch
            {
                Department.Sanita => "text-dark",       // Tmavý text pro žlutou (lepší čitelnost)
                _ => "text-white"                       // Bílý text pro ostatní
            };
        }

        /// <summary>
        /// Gets the display name for the department.
        /// </summary>
        /// <param name="department">The department</param>
        /// <returns>Display name in Czech</returns>
        public static string GetDisplayName(Department department)
        {
            return department switch
            {
                Department.Stavba => "Stavba",
                Department.PickUp => "Pick-up",
                Department.Sanita => "Sanita ⭐", // Hvězdička pro vaše oblíbené
                Department.Pila => "Pila - Přířez",
                Department.None => "-",
                _ => department.ToString()
            };
        }

        /// <summary>
        /// Gets the icon class for the department.
        /// </summary>
        /// <param name="department">The department</param>
        /// <returns>Font Awesome icon class</returns>
        public static string GetIconClass(Department department)
        {
            return department switch
            {
                Department.Stavba => "fa fa-building",
                Department.PickUp => "fa fa-truck",
                Department.Sanita => "fa fa-medkit",
                Department.Pila => "fa fa-industry",
                _ => "fa fa-circle"
            };
        }
    }
}
