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
    /// Reprezentuje widok dodawania nowego pacjenta do bazy danych.
    /// </summary>
    public partial class PatientDetailsView : UserControl
    {
        #region Private fields

        /// <summary>
        /// Prezenter obsługujący zdarzenia użytkownika.
        /// </summary>
        PatientDetailsPresenter patientDetailsPresenter;

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
        public PatientDetailsView(MainWindow parentWindow)
        {
            InitializeComponent();

            this.ParentWindow = parentWindow;

            patientDetailsPresenter = new PatientDetailsPresenter(this);
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
            // wyczyszczenie wszystkich pól i powrót do menu głównego
            patientDetailsPresenter.Back();
        }

        #endregion // Events handlers
    }
}
