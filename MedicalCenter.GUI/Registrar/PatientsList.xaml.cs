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
using MedicalCenter.Models.Registrar;

namespace MedicalCenter.GUI.Registrar
{
    /// <summary>
    /// Reprezentuje widok listy pacjentów do wyboru podczas rejestracji wizyty, nakładany na widok listy wizyt u danego lekarza w wybranym dniu.
    /// </summary>
    public partial class PatientsList : UserControl
    {
        #region Private fields

        /// <summary>
        /// Prezenter obsługujący m.in. zdarzenia użytkownika.
        /// </summary>
        RegisterVisitDetailsPresenter registerVisitDetailsPresenter;

        #endregion // Private fields

        #region Public properties

        /// <summary>
        /// Źródło elementów wyświetlanych na liście pacjentów.
        /// </summary>
        public List<Patient> Patients { get; set; }

        /// <summary>
        /// Lista pacjentów.
        /// </summary>
        public List<Patient> SourcePatients { get; set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor ustawiający kontekst danych dla listy pacjentów.
        /// </summary>
        public PatientsList(RegisterVisitDetailsPresenter presenter)
        {
            InitializeComponent();

            // zapisanie referencji do prezentera
            registerVisitDetailsPresenter = presenter;

            // ustawienie kontekstu danych dla listy pacjentów
            PatientsListBox.DataContext = this;

            // ukrycie tego widoku (stan domyślny)
            Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion // Ctors

        #region Events handlers

        /// <summary>
        /// Obsługa zdarzenia załadowania listy pacjentów (element interfejsu).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientsList_Loaded(object sender, RoutedEventArgs e)
        {
            // wypełnienie listy pacjentów
            registerVisitDetailsPresenter.GetPatientsList();
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola tekstowego filtru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // filtrowanie listy pacjentów
            registerVisitDetailsPresenter.FilterPatientName();
        }
        
        /// <summary>
        /// Obsługa zdarzenia zmiany zaznaczenia na liście pacjentów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // aktywacja/dezaktywacja przycisków "Szczegóły" i "Wybierz"
            registerVisitDetailsPresenter.PatientSelected();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // wyczyszczenie filtra i zaznaczenia na liście pacjentów, ukrycie widoku listy pacjentów
            registerVisitDetailsPresenter.BackPatientsList();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Szczegóły".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Details_Click(object sender, RoutedEventArgs e)
        {
            // wyświetlenie szczegółów wybranego pacjenta
            registerVisitDetailsPresenter.PatientDetails();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Wybierz".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            // przekazanie ID wybranego pacjenta do widoku głównego, ukrycie listy pacjentów
            registerVisitDetailsPresenter.ChoosePatient();
        }

        #endregion // Events handlers
    }
}
