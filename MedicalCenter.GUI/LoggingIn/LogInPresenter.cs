using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Services;
using MedicalCenter.Models.LoggingIn;

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

        /// <summary>
        /// Widok logowania zarządzany przez tego prezentera.
        /// </summary>
        LogInView view;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący obiekt logiki biznesowej w zakresie użytkowników systemu i
        /// zapisujący referencję do obiektu danych użytkownika.
        /// </summary>
        /// <param name="view">Widok logowania zarządzany przez tego prezentera.</param>
        public LogInPresenter(LogInView view)
        {
            userBusinessService = new UserBusinessService();
            this.view = view;
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Zaloguj" w formularzu logowania.
        /// </summary>
        public void Logon()
        {
            // sprawdzenie poprawności podanych poświadczeń
            userBusinessService.LogIn(view.UserData);

            // jeśli były poprawne
            if (view.UserData.Id > 0)
            {
                // wyczyszczenie pól z loginem, hasłem i hashem hasła
                view.UserData.Login = string.Empty;
                view.UserData.Password = string.Empty;
                view.Password.Password = string.Empty;

                MainWindow mainWindow = view.Parent as MainWindow;

                // zmiana tytułu okna głównego
                mainWindow.Title = view.UserData.Title;

                // zapisanie ID aktualnie zalogowanej osoby
                mainWindow.Id = view.UserData.Id;

                // zmiana ekranu logowania na menu główne
                switch(view.UserData.JobTitleCode)
                {
                    case "REJ":
                        mainWindow.ContentArea.Content = new Registrar.MainMenuView();
                        break;
                }
            }
        }

        #endregion // Public methods
    }
}
