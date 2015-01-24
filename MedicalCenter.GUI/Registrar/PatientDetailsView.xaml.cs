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
        /// Dane pacjenta, którego dane są przeglądane lub edytowane/zbiór danych nowego pacjenta.
        /// </summary>
        public Patient PatientData;

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

            PatientData = new Patient();

            // ustawienie kontekstu danych, aby móc powiązać obiekt PatientData z polami formularza
            DataContext = PatientData;
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

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola tekstowego "Pesel".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pesel_TextChanged(object sender, TextChangedEventArgs e)
        {
            // czyszczenie pola przy dodawaniu nowego pacjenta/dopełnianie zerami z lewej strony przy edycji danych istniejącego pacjenta
            patientDetailsPresenter.PeselChanged();
        }

        /// <summary>
        /// Obsługa zdarzenia utraty focusa klawiatury przez pole tekstowe "Pesel".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pesel_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // walidacja pola z peselem
            patientDetailsPresenter.ValidatePesel();
        }

        /// <summary>
        /// Obsługa zdarzenia wciśnięcia klawisza podczas edycji pola tekstowego "Pesel".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pesel_KeyDown(object sender, KeyEventArgs e)
        {
            // sprawdzenie czy wciśniety został dozwolony klawisz
            e.Handled = patientDetailsPresenter.PeselKeyDown(e.Key);
        }

        // TODO: w obsłudze kliknięcia przycisku "Zapisz" należy odbierać focus elementowi, który go aktualnie posiada, chyba, że jest to sam przycisk

        #endregion // Events handlers
    }
}
