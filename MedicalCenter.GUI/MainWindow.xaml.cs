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
        public int Id;

        /// <summary>
        /// Ekran logowania.
        /// </summary>
        public LogInView LoginView;

        /// <summary>
        /// Rejestratorka - menu główne.
        /// </summary>
        public Registrar.MainMenuView RegistrarMainMenuView;

        /// <summary>
        /// Rejestratorka - dodawanie nowego pacjenta do bazy.
        /// </summary>
        public Registrar.AddPatientView RegistrarAddPatientView;

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor okna głównego - inicjalizuje komponenty, ładuje treść okna, ustawia wartości domyślne.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ContentArea.Content = LoginView = new LogInView(this);

            Id = 0;
        }

        #endregion // Ctors
    }
}
