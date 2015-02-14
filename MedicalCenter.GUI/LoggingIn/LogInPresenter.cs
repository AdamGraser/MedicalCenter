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
    /// Obsługa zdarzeń użytkownika dla widoku logowania (działania na modelach i serwisach pod wpływem zdarzeń z widoku).
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
        /// zapisujący referencję do zarządzanego widoku.
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
            // zahashowanie hasła i zapisanie hashu w obiekcie danych użytkownika
            view.UserData.Password = view.Password.Password;

            // sprawdzenie poprawności podanych poświadczeń
            userBusinessService.LogIn(view.UserData);

            // jeśli były poprawne
            if (view.UserData.Id > 0)
            {
                // wyczyszczenie pól z loginem, hasłem i hashem hasła oraz etykiety z komunikatem
                view.Login.Clear();
                view.UserData.Password = string.Empty;
                view.Password.Clear();
                view.Message.Content = string.Empty;

                // zmiana tytułu okna głównego
                view.ParentWindow.Title = view.UserData.Title;

                // zapisanie ID aktualnie zalogowanej osoby
                view.ParentWindow.Id = view.UserData.Id;

                // zmiana ekranu logowania na menu główne
                if (view.UserData.JobTitleCode.StartsWith("REJ"))
                {
                    // jeśli widok menu głównego nie był dotychczas wyświetlany, należy go najpierw utworzyć
                    if (view.ParentWindow.RegistrarMainMenuView == null)
                        view.ParentWindow.RegistrarMainMenuView = new Registrar.MainMenuView(view.ParentWindow);

                    // zmiana ekranu logowania na menu główne
                    view.ParentWindow.ContentArea.Content = view.ParentWindow.RegistrarMainMenuView;
                }
            }
            else
            {
                // wyświetlenie komunikatu o nieprawidłowych poświadczeniach
                if (view.UserData.Id == 0)
                    view.Message.Content = "Nieprawidłowy login i/lub hasło";
                // wyświetlenie komunikatu o dezaktywacji konta
                else
                {
                    view.Message.Content = "Wskazane konto zostało dezaktywowane";
                    
                    // przywrócenie wartości domyślnej
                    view.UserData.Id = 0;
                }

                // zaznaczenie zawartości pola na login
                view.Login.Focus();
                view.Login.SelectAll();
            }
        }

        /// <summary>
        /// Sprawdza czy login i hasło zostały podane. Jeśli tak, aktywuje przycisk "Zaloguj". W przeciwnym razie dezaktywuje go.
        /// </summary>
        public void FormCompleted()
        {
            // jeśli wpisano login i hasło, możliwe jest podjęcie próby zalogowania się
            if (view.Login.Text.Length > 0 && view.Password.Password.Length > 0)
                view.Logon.IsEnabled = true;
            else
            // jeśli nie, przycisk "Zaloguj" jest dezaktywowany
                view.Logon.IsEnabled = false;
        }

        #endregion // Public methods
    }
}
