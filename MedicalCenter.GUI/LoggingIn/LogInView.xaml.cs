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
    /// Interaction logic for LogInView.xaml
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
        public User UserData;

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor inicjalizujący pola i właściwości.
        /// </summary>
        public LogInView()
        {
            InitializeComponent();

            UserData = new User();
            UserData.Title = "Nazwa placówki medycznej";

            logInPresenter = new LogInPresenter(this);
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
            System.Security.Cryptography.HashAlgorithm sha = System.Security.Cryptography.HashAlgorithm.Create("SHA512");

            // zahashowanie hasła i zapisanie hashu w obiekcie danych użytkownika
            UserData.Password = System.Text.Encoding.ASCII.GetString(sha.ComputeHash(System.Text.Encoding.ASCII.GetBytes(Password.Password)));

            // próba zalogowania się
            logInPresenter.Logon();
        }

        #endregion // Event handlers
    }
}
