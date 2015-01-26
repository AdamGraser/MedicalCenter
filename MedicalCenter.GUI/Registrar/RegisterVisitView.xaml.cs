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
        public int PatientId;

        /// <summary>
        /// Lista obiektów zawierających nazwę poradni, nazwisko i imię lekarza, nr gabinetu oraz liczbę pacjentów, których lekarz przyjąć ma w danym dniu.
        /// </summary>
        public List<DoctorsListItem> DoctorsList;

        /// <summary>
        /// Lista nazw poradni i ID odpowiadających im rekordów z tabeli M_DictionaryClinics.
        /// </summary>
        public Dictionary<int, string> ClinicsList;

        /// <summary>
        /// Filtr poradni.
        /// </summary>
        public ComboBox FilterClinicName;

        /// <summary>
        /// Filtr lekarzy.
        /// </summary>
        public TextBox FilterDoctorName;

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
        public RegisterVisitView(MainWindow parentWindow)
        {
            InitializeComponent();

            // zapisanie referencji do nadrzędnego okna
            this.ParentWindow = parentWindow;

            // utworzenie prezentera
            registerVisitPresenter = new RegisterVisitPresenter(this);

            // zapisanie domyślnego ID pacjenta
            PatientId = 0;

            // utworzenie listy lekarzy (poradnia, lekarz, liczba pacjentów w wybranym dniu, nr gabinetu)
            DoctorsList = new List<DoctorsListItem>();

            // utworzenie listy poradni medycznych
            ClinicsList = new Dictionary<int, string>();

            // ustawienie kontekstu danych tabeli, aby móc z nią powiązać listy lekarzy i poradni
            DoctorsListTable.DataContext = this;

            // zapisanie referencji do filtru poradni
            StackPanel header = DoctorsListTable.Columns[0].Header as StackPanel;
            FilterClinicName = header.Children.OfType<ComboBox>().First(x => x.Name == "FilterClinicName");

            // zapisanie referencji do filtru lekarzy
            header = DoctorsListTable.Columns[1].Header as StackPanel;
            FilterDoctorName = header.Children.OfType<TextBox>().First(x => x.Name == "FilterDoctorName");
        }

        #endregion // Ctors

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

        #endregion // Events handlers
    }
}
