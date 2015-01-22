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

namespace MedicalCenter.GUI.Registrar
{
    /// <summary>
    /// Reprezentuje widok menu głównego rejestratorki.
    /// </summary>
    public partial class MainMenuView : UserControl
    {
        #region Private fields

        /// <summary>
        /// Prezenter obsługujący zdarzenia użytkownika.
        /// </summary>
        MainMenuPresenter mainMenuPresenter;

        #endregion // Private fields

        #region Public properties

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
        public MainMenuView(MainWindow parentWindow)
        {
            InitializeComponent();

            this.ParentWindow = parentWindow;

            mainMenuPresenter = new MainMenuPresenter(this);
        }

        #endregion // Ctors

        #region Events handlers

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Wyloguj".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            // rozpoczęcie procedury wylogowania
            mainMenuPresenter.LogOut();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Dodaj pacjenta".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            // zmiana zawartości okna głównego z menu na widok szczegółów pacjenta
            mainMenuPresenter.AddPatient();
        }

        #endregion // Events handlers
    }
}
