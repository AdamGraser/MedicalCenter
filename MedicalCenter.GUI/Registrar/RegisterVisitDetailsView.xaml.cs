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
    /// Reprezentuje widok listy wizyt w wybranym dniu, dla danego lekarza, przy rejestrowaniu nowej wizyty.
    /// </summary>
    public partial class RegisterVisitDetailsView : UserControl
    {
        #region Private fields

        /// <summary>
        /// Prezenter obsługujący m.in. zdarzenia użytkownika.
        /// </summary>
        RegisterVisitDetailsPresenter registerVisitDetailsPresenter;

        #endregion // Private fields

        #region Public properties

        /// <summary>
        /// Przechowuje następujące informacje o rejestrowanej wizycie: ID pacjenta, ID lekarza, datę wizyty i flagę nagłego przypadku.
        /// </summary>
        public Visit VisitData;

        /// <summary>
        /// Lista wizyt z wybranego dnia u wskazanego lekarza.
        /// </summary>
        public List<DailyVisitsListItem> DailyVisits;

        /// <summary>
        /// Przechowuje nazwisko i imię (jeden string, oddzielone spacją) lekarza, do którego ma zostać zarejestrowana wizyta.
        /// </summary>
        public string DoctorName;

        /// <summary>
        /// Przechowuje nazwisko i imię (jeden string, oddzielone spacją) pacjenta, dla którego ma zostać zarejestrowana wizyta.
        /// </summary>
        public string PatientName;

        /// <summary>
        /// Przechowuje tekstowy zapis godzin pracy wybranego lekarza w danym dniu.
        /// </summary>
        public string Hours;

        /// <summary>
        /// Okno główne, którego treść stanowi ten widok.
        /// </summary>
        public MainWindow ParentWindow;

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor inicjalizujący pola i właściwości.
        /// </summary>
        /// <param name="parentWindow">Okno główne, którego treść stanowi ten widok.</param>
        public RegisterVisitDetailsView(MainWindow parentWindow)
        {
            InitializeComponent();

            // zapisanie referencji do nadrzędnego okna
            this.ParentWindow = parentWindow;

            // utworzenie prezentera
            registerVisitDetailsPresenter = new RegisterVisitDetailsPresenter(this);

            // ustawienie kontekstu danych na potrzeby podpięcia danych do elementów interfejsu
            DataContext = this;

            // inicjalizacja właściwości
            VisitData = new Visit();
            DailyVisits = new List<DailyVisitsListItem>();
            DoctorName = string.Empty;
        }

        #endregion // Ctors

        #region Events handlers

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Dodaj pacjenta".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Obsługa zdarzenia zmiany wybranej daty wizyty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TheDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Wybierz pacjenta".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoosePatient_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Zarejestruj".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zaznaczenia na liście wizyt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DailyVisitsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion // Events handlers
    }
}
