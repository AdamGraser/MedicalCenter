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

namespace MedicalCenter.GUI.LoggingIn
{
    /// <summary>
    /// Interaction logic for ConfigureConnection.xaml
    /// </summary>
    public partial class ConfigureConnection : UserControl
    {
        #region Private fields

        /// <summary>
        /// Prezenter obsługujący m.in. zdarzenia użytkownika.
        /// </summary>
        LogInPresenter logInPresenter;

        #endregion // Private fields

        #region Public properties

        /// <summary>
        /// Przechowuje wpisany w odpowiednim polu adres serwera bazy danych.
        /// </summary>
        public string ServerAddress { get; set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący referencję do prezentera i inicjalizujący elementy interfejsu.
        /// </summary>
        /// <param name="logInPresenter">Referencja do prezentera formularza logowania.</param>
        public ConfigureConnection(LogInPresenter logInPresenter)
        {
            InitializeComponent();

            // zapisanie referencji do prezentera
            this.logInPresenter = logInPresenter;

            // inicjalizacja pola na adres serwera bazy danych pustym stringiem
            ServerAddress = string.Empty;

            // ukrycie tego widoku (stan domyślny)
            Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion // Ctors

        #region Events handlers

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola na adres serwera bazy danych.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Address_TextChanged(object sender, TextChangedEventArgs e)
        {
            // aktywacja/dezaktywacja przycisku "Zapisz" jeśli pole jest niepuste/puste
            logInPresenter.ConfigureAddressChanged();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // wyczyszczenie pola na adres serwera, ukrycie tego widoku
            logInPresenter.ConfigureBack();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Zapisz".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // zapisanie adresu serwera do plików konfiguracyjnych, powrót do formularza logowania
            logInPresenter.ConfigureSave();
        }

        #endregion // Events handlers
    }
}
