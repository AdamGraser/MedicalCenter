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
        #region Public properties

        /// <summary>
        /// Prezenter obsługujący m.in. zdarzenia użytkownika.
        /// </summary>
        public LogInPresenter Presenter { get; set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor inicjalizujący elementy interfejsu.
        /// </summary>
        public ConfigureConnection()
        {
            InitializeComponent();

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
            Presenter.ConfigureAddressChanged();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // wyczyszczenie pola na adres serwera, ukrycie tego widoku
            Presenter.ConfigureBack();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Zapisz".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // zapisanie adresu serwera do plików konfiguracyjnych, powrót do formularza logowania
            Presenter.ConfigureSave();
        }

        #endregion // Events handlers
    }
}
