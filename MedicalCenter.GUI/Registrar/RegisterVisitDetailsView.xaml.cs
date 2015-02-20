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
        public Visit VisitData { get; private set; }

        /// <summary>
        /// Lista wizyt z wybranego dnia u wskazanego lekarza.
        /// </summary>
        public List<DailyVisitsListItem> DailyVisits { get; set; }

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
        public RegisterVisitDetailsView(MainWindow parentWindow)
        {
            InitializeComponent();

            // zapisanie referencji do nadrzędnego okna
            this.ParentWindow = parentWindow;

            // utworzenie prezentera
            registerVisitDetailsPresenter = new RegisterVisitDetailsPresenter(this);

            // ustawienie minimalnej daty do wybrania na dzisiejszą
            TheDate.DisplayDateStart = DateTime.Today;

            // ustawienie kontekstu danych na potrzeby podpięcia danych do elementów interfejsu
            DataContext = this;

            // inicjalizacja właściwości
            VisitData = new Visit();

            // przekazanie widokowi listy pacjentów referencji do prezentera
            PatientsListView.Presenter = registerVisitDetailsPresenter;
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
            // przejście do formularza dodawania nowego pacjenta
            registerVisitDetailsPresenter.AddPatient();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // powrót do widoku listy lekarzy
            registerVisitDetailsPresenter.Back();
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany wybranej daty wizyty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TheDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // kontrola zakresu wybranej daty, wyświetlenie godzin przyjmowania lekarza w wybranym dniu i pobranie listy wizyt na wybrany dzień
            registerVisitDetailsPresenter.TheDateChanged();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Wybierz pacjenta".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoosePatient_Click(object sender, RoutedEventArgs e)
        {
            // wyświetlenie listy pacjentów i ew. zaznaczenie na niej wybranego dotychczas pacjenta
            registerVisitDetailsPresenter.ChoosePatient();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Zarejestruj".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // zarejestrowanie wizyty, powrót do menu głównego
            registerVisitDetailsPresenter.RegisterVisit();
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zaznaczenia na liście wizyt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DailyVisitsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // uwzględnienie zmiany zaznaczenia
            registerVisitDetailsPresenter.HourSelected();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "&lt;" przy polu datowym.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TheDateBackwards_Click(object sender, RoutedEventArgs e)
        {
            // zmiana daty na wcześniejszą o co najmniej 1 dzień
            registerVisitDetailsPresenter.ChangeDate(-1.0);
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "&gt;" przy polu datowym.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TheDateForward_Click(object sender, RoutedEventArgs e)
        {
            // zmiana daty na późniejszą o co najmniej 1 dzień
            registerVisitDetailsPresenter.ChangeDate(1.0);
        }

        /// <summary>
        /// Obsługa zdarzenia zaznaczenia pola "Nagły przypadek".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsEmergency_Checked(object sender, RoutedEventArgs e)
        {
            // dodanie do listy wizyt nowej pozycji (wolnej godziny) na samym końcu
            registerVisitDetailsPresenter.Emergency(true);
        }

        /// <summary>
        /// Obsługa zdarzenia odznaczenia pola "Nagły przypadek".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsEmergency_Unchecked(object sender, RoutedEventArgs e)
        {
            // usunięcie z końca listy wizyt nowej pozycji (wolnej godziny)
            registerVisitDetailsPresenter.Emergency(false);
        }

        #endregion // Events handlers

        #region Public methods

        /// <summary>
        /// Czyści i ponownie wypełnia danymi listę wizyt.
        /// </summary>
        public void RefreshVisitsList()
        {
            registerVisitDetailsPresenter.GetVisitsList();
        }

        /// <summary>
        /// Czyści i ponownie wypełnia danymi listę pacjentów.
        /// </summary>
        public void RefreshPatientsList()
        {
            registerVisitDetailsPresenter.ChoosePatientRefresh();
        }

        #endregion // Public methods
    }
}
