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

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Dodaj pacjenta" przy liście wizyt (rejestracja wizyty).
        /// </summary>
        public void AddPatient()
        {
            // jeśli widok szczegółów pacjenta nie był dotychczas używany, należy go utworzyć
            if (view.ParentWindow.RegistrarPatientDetailsView == null)
                view.ParentWindow.RegistrarPatientDetailsView = new PatientDetailsView(view.ParentWindow);

            // zmiana zawartości okna głównego z menu na szczegóły pacjenta
            view.ParentWindow.ContentArea.Content = view.ParentWindow.RegistrarPatientDetailsView;

            // włączenie trybu edycji formularza na ekranie ze szczegółami pacjenta
            view.ParentWindow.RegistrarPatientDetailsView.EnableEditing(true);

            // zapisanie w historii referencji do tego widoku
            view.ParentWindow.History.Push(view);
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót" w widoku listy wizyt na dany dzień przy rejestracji wizyty.
        /// Czyści zaznaczenie i zmienia zawartość okna głównego aplikacji na menu główne.
        /// </summary>
        public void Back()
        {
            // wyczyszczenie zaznaczenia na liście
            view.DailyVisitsList.SelectedIndex = -1;

            // czyszczenie listy
            view.DailyVisits.Clear();

            // czyszczenie i dezaktywacja pola "Nagły przypadek"
            view.IsEmergency.IsChecked = false;
            view.IsEmergency.IsEnabled = false;

            // przywracanie domyślnej daty
            view.TheDate.SelectedDate = DateTime.Today;

            // wyczyszczenie ID pacjenta i ID lekarza
            view.VisitData.PatientId = 0;
            view.VisitData.DoctorId = 0;

            // przywrócenie widoku listy lekarzy
            view.ParentWindow.ContentArea.Content = view.ParentWindow.History.Pop();
        }

        #endregion // Public methods
    }
}
