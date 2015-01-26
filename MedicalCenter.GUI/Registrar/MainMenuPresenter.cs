using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.GUI.Registrar
{
    /// <summary>
    /// Obsługa zdarzeń użytkownika dla widoku menu głównego rejestratorki (działania na modelach i serwisach pod wpływem zdarzeń z widoku).
    /// </summary>
    public class MainMenuPresenter
    {
        #region Private fields

        /// <summary>
        /// Widok menu głównego rejestratorki zarządzany przez tego prezentera.
        /// </summary>
        MainMenuView view;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący referencję do zarządzanego widoku.
        /// </summary>
        /// <param name="view">Widok menu głównego rejestratorki zarządzany przez tego prezentera.</param>
        public MainMenuPresenter(MainMenuView view)
        {
            this.view = view;
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Wyloguj" w menu głównym rejestratorki.
        /// </summary>
        public void LogOut()
        {
            // należy zapytać użytkownika czy jest pewien chęci wylogowania się z systemu;
            // tylko jeśli użytkownik kliknął przycisk "Tak", należy wykonać procedurę wylogowania
            if (System.Windows.Forms.MessageBox.Show("Czy na pewno chcesz się wylogować?",
                                                     "Pytanie",
                                                     System.Windows.Forms.MessageBoxButtons.YesNo,
                                                     System.Windows.Forms.MessageBoxIcon.Question,
                                                     System.Windows.Forms.MessageBoxDefaultButton.Button2)
                == System.Windows.Forms.DialogResult.Yes)
            {
                // ustawienie wartości domyślnych
                view.ParentWindow.Id = 0;
                view.ParentWindow.Title = "Nazwa placówki medycznej";

                // wyświetlenie ekranu logowania
                view.ParentWindow.ContentArea.Content = new LoggingIn.LogInView(view.ParentWindow);

                // wyczyszczenie stosu z ostatnio wyświetlanymi widokami
                view.ParentWindow.History.Clear();
            }
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Dodaj pacjenta" w menu głównym rejestratorki.
        /// </summary>
        public void AddPatient()
        {
            // jeśli widok szczegółów pacjenta nie był dotychczas używany, należy go utworzyć
            if (view.ParentWindow.RegistrarPatientDetailsView == null)
                view.ParentWindow.RegistrarPatientDetailsView = new PatientDetailsView(view.ParentWindow);

            // zmiana zawartości okna głównego z menu na szczegóły pacjenta
            view.ParentWindow.ContentArea.Content = view.ParentWindow.RegistrarPatientDetailsView;

            // zapisanie w historii referencji do widoku menu głównego
            view.ParentWindow.History.Push(view);
        }

        #endregion // Public methods
    }
}
