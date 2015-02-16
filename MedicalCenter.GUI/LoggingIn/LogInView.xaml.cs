using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MedicalCenter.Models.LoggingIn;

namespace MedicalCenter.GUI.LoggingIn
{
    /// <summary>
    /// Reprezentuje widok logowania do systemu.
    /// </summary>
    public partial class LogInView : UserControl
    {
        #region Private fields

        /// <summary>
        /// Prezenter obsługujący zdarzenia użytkownika.
        /// </summary>
        LogInPresenter logInPresenter;

        #endregion // Private fields

        #region Public properties

        /// <summary>
        /// Dane użytkownika systemu (login i hasło lub imię, nazwisko i stanowisko zalogowanego użytkownika).
        /// </summary>
        public User UserData { get; private set; }

        /// <summary>
        /// Okno główne, którego treść stanowi ten widok.
        /// </summary>
        public MainWindow ParentWindow { get; private set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor inicjalizujący pola i właściwości.
        /// </summary>
        /// <param name="parentWindow">Okno główne, którego treść stanowi ten widok.</param>
        public LogInView(MainWindow parentWindow)
        {
            InitializeComponent();

            this.ParentWindow = parentWindow;

            // utworzenie obiektu danych użytkownika
            UserData = new User();
            UserData.Title = "Nazwa placówki medycznej";

            // utworzenie prezentera dla tego widoku
            logInPresenter = new LogInPresenter(this);

            // powiązanie obiektu z danymi użytkownika z polem na login
            // (w XAML'u zapisane zostało już konkretne powiązanie własciwości UserData.Login z właściwością Login.Text)
            Login.DataContext = UserData;

            // przekazanie referencji do prezentera dodatkowemu widokowi konfiguracji połączenia z serwerem bazy danych
            ConfigureConnectionView.Presenter = logInPresenter;
        }

        #endregion // Ctors

        #region Event handlers

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Zaloguj" w formularzu logowania.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logon_Click(object sender, RoutedEventArgs e)
        {
            // próba zalogowania się
            logInPresenter.Logon();
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola "Login".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            // jeśli wpisano login i hasło, możliwe jest podjęcie próby zalogowania się
            logInPresenter.FormCompleted();
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola "Password".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // jeśli wpisano login i hasło, możliwe jest podjęcie próby zalogowania się
            logInPresenter.FormCompleted();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Konfiguruj połączenie z bazą danych" pod formularzem logowania.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigureDBConnection_Click(object sender, RoutedEventArgs e)
        {
            // wyświetlenie widoku konfiguracji połączenia z serwerem bazy danych
            logInPresenter.Configure();
        }

        #endregion // Event handlers
    }
}
