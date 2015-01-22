using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.GUI.Registrar
{
    /// <summary>
    /// Obsługa zdarzeń użytkownika dla widoku szczegółów pacjenta (działania na modelach i serwisach pod wpływem zdarzeń z widoku).
    /// </summary>
    public class PatientDetailsPresenter
    {
        #region Private fields

        /// <summary>
        /// Widok szczegółów pacjenta zarządzany przez tego prezentera.
        /// </summary>
        PatientDetailsView view;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący referencję do zarządzanego widoku.
        /// </summary>
        /// <param name="view">Widok menu głównego rejestratorki zarządzany przez tego prezentera.</param>
        public PatientDetailsPresenter(PatientDetailsView view)
        {
            this.view = view;
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót" w widoku szczegółów pacjenta.
        /// </summary>
        public void Back()
        {
            // należy zapytać użytkownika czy jest pewien chęci powrotu do menu głównego;
            // tylko jeśli użytkownik kliknął przycisk "Tak", należy wyczyścić wszystkie pola i wrócić do menu głównego
            if (System.Windows.Forms.MessageBox.Show("Czy na pewno chcesz wrócić do menu głównego? Wszelkie zmiany nie zostaną zapisane.",
                                                     "Pytanie",
                                                     System.Windows.Forms.MessageBoxButtons.YesNo,
                                                     System.Windows.Forms.MessageBoxIcon.Question,
                                                     System.Windows.Forms.MessageBoxDefaultButton.Button2)
                == System.Windows.Forms.DialogResult.Yes)
            {
                // ustawienie wartości domyślnych w widoku szczegółów pacjenta
                view.LastName.Clear();
                view.FirstName.Clear();
                view.SecondName.Clear();
                view.BirthDate.SelectedDate = null;
                view.Gender.SelectedIndex = -1;
                view.Pesel.Clear();
                view.Street.Clear();
                view.BuildingNumber.Clear();
                view.Apartment.Clear();
                view.PostalCode.Clear();
                view.City.Clear();
                view.Post.Clear();
                view.IsInsured.IsChecked = true;

                // powrót do menu głównego
                view.ParentWindow.ContentArea.Content = view.ParentWindow.RegistrarMainMenuView;
            }
        }

        #endregion // Public methods
    }
}
