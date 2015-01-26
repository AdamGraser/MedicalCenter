using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Services;

namespace MedicalCenter.GUI.Registrar
{
    /// <summary>
    /// Obsługa zdarzeń użytkownika dla widoku szczegółów pacjenta (działania na modelach i serwisach pod wpływem zdarzeń z widoku).
    /// </summary>
    public class RegisterVisitPresenter
    {
        #region Private fields

        /// <summary>
        /// Widok listy lekarzy (przy rejestracji wizyty) zarządzany przez tego prezentera.
        /// </summary>
        RegisterVisitView view;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący referencję do zarządzanego widoku i tworzący obiekty usług logiki biznesowej.
        /// </summary>
        /// <param name="view">Widok listy lekarzy (przy rejestracji wizyty) zarządzany przez tego prezentera.</param>
        public RegisterVisitPresenter(RegisterVisitView view)
        {
            this.view = view;
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót" w widoku listy lekarzy przy rejestracji wizyty.
        /// Czyści filtry, zaznaczenie i zmienia zawartość okna głównego aplikacji na menu główne.
        /// </summary>
        public void Back()
        {
            // wyczyszczenie filtrów
            view.FilterClinicName.SelectedIndex = -1;
            view.FilterDoctorName.Clear();

            // wyczyszczenie zaznaczenia w tabeli
            view.DoctorsListTable.SelectedIndex = -1;

            // wyczyszczenie ID pacjenta
            view.PatientId = 0;

            // ustawienie dzisiejszej daty
            view.TheDate.SelectedDate = DateTime.Today;

            // przywrócenie widoku menu głównego
            view.ParentWindow.ContentArea.Content = view.ParentWindow.History.Pop();
        }

        #endregion // Public methods
    }
}
