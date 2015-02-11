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
    /// Reprezentuje widok listy lekarzy przy rejestrowaniu nowej wizyty.
    /// </summary>
    public partial class RegisterVisitView : UserControl
    {
        #region Private fields

        /// <summary>
        /// Prezenter obsługujący m.in. zdarzenia użytkownika.
        /// </summary>
        RegisterVisitPresenter registerVisitPresenter;

        #endregion // Private fields

        #region Public properties

        /// <summary>
        /// Przechowuje ID pacjenta, dla którego ma zostać zarejestrowana wizyta (0 jeśli jeszcze nie wybrano).
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Lista obiektów zawierających nazwę poradni, nazwisko i imię lekarza, nr gabinetu oraz liczbę pacjentów, których lekarz przyjąć ma w danym dniu.
        /// </summary>
        public List<DoctorsListItem> SourceDoctorsList { get; set; }

        /// <summary>
        /// Źródło danych dla tabeli w widoku - zmodyfikowana przez filtry (najczęściej okrojona) wersja listy SourceDoctorsList.
        /// </summary>
        public List<DoctorsListItem> DoctorsList { get; set; }

        /// <summary>
        /// Lista nazw poradni i ID odpowiadających im rekordów z tabeli M_DictionaryClinics.
        /// </summary>
        public Dictionary<int, string> ClinicsList { get; private set; }

        /// <summary>
        /// Filtr poradni.
        /// </summary>
        public ComboBox FilterClinicName { get; private set; }

        /// <summary>
        /// Filtr lekarzy.
        /// </summary>
        public TextBox FilterDoctorName { get; private set; }

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
        public RegisterVisitView(MainWindow parentWindow)
        {
            InitializeComponent();

            // zapisanie referencji do nadrzędnego okna
            this.ParentWindow = parentWindow;

            // utworzenie prezentera
            registerVisitPresenter = new RegisterVisitPresenter(this);

            // zapisanie domyślnego ID pacjenta
            PatientId = 0;

            // ustawienie startowej i minimalnej daty na dziś
            TheDate.SelectedDate = DateTime.Today;
            TheDate.DisplayDateStart = DateTime.Today;

            // utworzenie listy lekarzy (poradnia, lekarz, liczba pacjentów w wybranym dniu, nr gabinetu)
            SourceDoctorsList = new List<DoctorsListItem>();
            registerVisitPresenter.GetDoctorsList();

            // utworzenie listy poradni medycznych
            ClinicsList = new Dictionary<int, string>();
            ClinicsList.Add(0, "");
            registerVisitPresenter.GetClinicsList();

            // ustawienie kontekstu danych tabeli, aby móc z nią powiązać listy lekarzy i poradni
            DoctorsListTable.DataContext = this;
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Przywraca widok do stanu domyślnego, zmienia treść okna głównego na menu główne.
        /// </summary>
        public void ViewBack()
        {
            // powrót do menu głównego
            registerVisitPresenter.Back();
        }

        #endregion // Public methods

        #region Events handlers

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // wyczyszczenie filtrów, zaznaczenia, przywrócenie domyślnej daty i powrót do menu głównego
            registerVisitPresenter.Back();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Dodaj pacjenta".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            // zmiana zawartości okna głównego z menu na widok szczegółów pacjenta
            registerVisitPresenter.AddPatient();
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zaznaczenia w tabeli z listą lekarzy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoctorsListTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // aktywacja/dezaktywacja przycisków pod tabelą
            registerVisitPresenter.DoctorSelected();
        }

        /// <summary>
        /// Obsługa zdarzenia załadowania pola tekstowego filtru lekarzy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterDoctorName_Loaded(object sender, RoutedEventArgs e)
        {
            // zapisanie referencji do tego pola tekstowego
            FilterDoctorName = sender as TextBox;
        }
        
        /// <summary>
        /// Obsługa zdarzenia zmiany wyboru w filtrze poradni.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterClinicName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // filtrowanie listy lekarzy
            registerVisitPresenter.FilterDoctorsList();
        }

        /// <summary>
        /// Obsługa zdarzenia załadowania listy rozwijanej filtru poradni medycznych.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterClinicName_Loaded(object sender, RoutedEventArgs e)
        {
            // zapisanie referencji do tej listy rozwijanej
            FilterClinicName = sender as ComboBox;
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany tekstu w filtrze nazwisk lekarzy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterDoctorName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // filtrowanie listy lekarzy
            registerVisitPresenter.FilterDoctorsList();
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany wybranej daty w polu pod tabelą.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TheDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // kontrola zakresu daty, wczytanie nowej listy lekarzy z uwzględnieniem filtrów
            registerVisitPresenter.TheDateChanged();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Najbl. termin"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosestFreeDate_Click(object sender, RoutedEventArgs e)
        {
            // znalezienie najbliższego wolnego terminu u wybranego lekarza
            registerVisitPresenter.ClosestFreeDate();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Dalej"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            // przejście do następnego ekranu rejestracji wizyty
            registerVisitPresenter.Next();
        }

        #endregion // Events handlers
    }
}
