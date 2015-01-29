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
        /// Logika biznesowa w zakresie medycznej działalności placówki.
        /// </summary>
        MedicalBusinessService medicalBusinessService;

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
            medicalBusinessService = new MedicalBusinessService();
            this.view = view;
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Pobiera i przekazuje do widoku listę wizyt z danego dnia dla wybranego lekarza.
        /// Jeśli liczba wizyt osiągnęła bądź przekroczyła maksimum, uaktywniany jest checkbox "Nagły przypadek".
        /// </summary>
        public void GetVisitsList()
        {
            // pobranie listy wizyt
            view.DailyVisits = medicalBusinessService.GetTodaysVisits(view.VisitData.DoctorId, view.VisitData.DateOfVisit);

            // jeśli jest null, to lekarz nie przyjmuje w danym dniu (tworzenie nowej listy, bo null'a się nie poda jako ItemsSource)
            if (view.DailyVisits == null)
                view.DailyVisits = new List<DailyVisitsListItem>();

            // sprawdzenie liczby wizyt -> aktywacja lub dezaktywacja checkbox'a
            if (view.DailyVisits.Count >= medicalBusinessService.TodaysVisitsCount(view.VisitData.DoctorId, view.VisitData.DateOfVisit))
                view.IsEmergency.IsEnabled = true;
            else
                view.IsEmergency.IsEnabled = false;
        }

        #endregion // Public methods
    }
}
