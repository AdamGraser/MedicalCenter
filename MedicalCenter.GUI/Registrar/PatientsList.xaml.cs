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
        #region Public properties

        /// <summary>
        /// Prezenter obsługujący m.in. zdarzenia użytkownika.
        /// </summary>
        public RegisterVisitDetailsPresenter Presenter { get; set; }

        /// <summary>
        /// Źródło elementów wyświetlanych na liście pacjentów.
        /// </summary>
        public List<Patient> Patients { get; set; }

        /// <summary>
        /// Lista pacjentów.
        /// </summary>
        public List<Patient> SourcePatients { get; set; }

        /// <summary>
        /// Określa czy lista pacjentów ma być sortowana malejąco.
        /// </summary>
        public bool SortDescending { get; set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor ustawiający kontekst danych dla listy pacjentów, zapisujący referencję do prezentera i ukrywający ten widok.
        /// </summary>
        public PatientsList()
        {
            InitializeComponent();

            // ustawienie kontekstu danych dla listy pacjentów
            PatientsListBox.DataContext = this;

            // ukrycie tego widoku (stan domyślny)
            Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion // Ctors

        #region Events handlers

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola tekstowego filtru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // filtrowanie listy pacjentów
            Presenter.ChoosePatientFilter();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku sortowania.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            // sortowanie listy pacjentów
            Presenter.ChoosePatientSort();
        }
        
        /// <summary>
        /// Obsługa zdarzenia zmiany zaznaczenia na liście pacjentów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // aktywacja/dezaktywacja przycisków "Szczegóły" i "Wybierz"
            Presenter.ChoosePatientSelected();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // wyczyszczenie filtra i zaznaczenia na liście pacjentów, ukrycie widoku listy pacjentów
            Presenter.ChoosePatientBack();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Szczegóły".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Details_Click(object sender, RoutedEventArgs e)
        {
            // wyświetlenie szczegółów wybranego pacjenta
            Presenter.ChoosePatientDetails();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Wybierz".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            // przekazanie ID wybranego pacjenta do widoku głównego, ukrycie listy pacjentów
            Presenter.ChoosePatientSelect();
        }

        #endregion // Events handlers
    }
}
