using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Data;
using MedicalCenter.Models.Registrar;
using MedicalCenter.Services;

namespace MedicalCenter.GUI.Registrar
{
    /// <summary>
    /// Obsługa zdarzeń użytkownika dla widoku listy wizyt w danym dniu, u danego lekarza, przy rejestracji wizyty.
    /// </summary>
    public class RegisterVisitDetailsPresenter
    {
        #region Private fields

        /// <summary>
        /// Widok listy wizyt w danym dniu (przy rejestracji wizyty) zarządzany przez tego prezentera.
        /// </summary>
        RegisterVisitDetailsView view;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący referencję do zarządzanego widoku i tworzący obiekty usług logiki biznesowej.
        /// </summary>
        /// <param name="view">Widok listy wizyt w danym dniu (przy rejestracji wizyty) zarządzany przez tego prezentera.</param>
        public RegisterVisitDetailsPresenter(RegisterVisitDetailsView view)
        {
            this.view = view;
        }

        #endregion // Ctors

        #region Public methods

        public void GetVisitsList()
        {

        }

        #endregion // Public methods
    }
}
