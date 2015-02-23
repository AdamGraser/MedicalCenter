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
using MedicalCenter.GUI.LoggingIn;

namespace MedicalCenter.GUI
{
    /// <summary>
    /// Reprezentuje okno główne aplikacji.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Public properties

        /// <summary>
        /// Przechowuje ID rekordu z tabeli A_Workers, reprezentującego osobę aktualnie zalogowaną.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Historia stron, przez które przeszedł użytkownik.
        /// </summary>
        public Stack<UserControl> History { get; private set; }

        /// <summary>
        /// Rejestratorka - menu główne.
        /// </summary>
        public Registrar.MainMenuView RegistrarMainMenuView { get; set; }

        /// <summary>
        /// Rejestratorka - dodawanie nowego pacjenta do bazy.
        /// </summary>
        public Registrar.PatientDetailsView RegistrarPatientDetailsView { get; set; }

        /// <summary>
        /// Rejestratorka - rejestrowanie wizyty - lista lekarzy.
        /// </summary>
        public Registrar.RegisterVisitView RegistrarRegisterVisitView { get; set; }

        /// <summary>
        /// Rejestratorka - rejestrowanie wizyty - lista wizyt w danym dniu dla wybranego lekarza.
        /// </summary>
        public Registrar.RegisterVisitDetailsView RegistrarRegisterVisitDetailsView { get; set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor okna głównego - inicjalizuje komponenty, ładuje treść okna, ustawia wartości domyślne.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // ustawienie początkowego widoku - formularza logowania
            ContentArea.Content = new LogInView(this);

            // domyślna wartość ID zalogowanego użytkownika
            Id = 0;

            // utworzenie stosu na ostatnio wyświetlane widoki
            History = new Stack<UserControl>();
        }

        #endregion // Ctors

        #region Events handlers
        
        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku zamykającego okno główne aplikacji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // jeśli użytkownik jest zalogowany, należy go zapytać, czy jest pewien chęci wylogowania się i zamknięcia aplikacji
            if (Id > 0)
            {
                // jeśli użytkownik kliknął przycisk "Nie", należy anulować procedurę wylogowania i zamknięcia aplikacji
                if (System.Windows.Forms.MessageBox.Show("Czy na pewno chcesz się wylogować i zamknąć aplikację?",
                                                         "Pytanie",
                                                         System.Windows.Forms.MessageBoxButtons.YesNo,
                                                         System.Windows.Forms.MessageBoxIcon.Question,
                                                         System.Windows.Forms.MessageBoxDefaultButton.Button2)
                    == System.Windows.Forms.DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion // Events handlers
    }
}
