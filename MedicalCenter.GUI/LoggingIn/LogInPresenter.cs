using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Services;

namespace MedicalCenter.GUI.LoggingIn
{
    /// <summary>
    /// Obsługa zdarzeń użytkownika (działania na modelach i serwisach pod wpływem zdarzeń z widoku).
    /// </summary>
    public class LogInPresenter
    {
        #region Private fields

        /// <summary>
        /// Logika biznesowa w zakresie użytkowników systemu.
        /// </summary>
        UserBusinessService userBusinessService;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący obiekt logiki biznesowej w zakresie użytkowników systemu.
        /// </summary>
        public LogInPresenter()
        {
            userBusinessService = new UserBusinessService();
        }

        #endregion // Ctors
    }
}
