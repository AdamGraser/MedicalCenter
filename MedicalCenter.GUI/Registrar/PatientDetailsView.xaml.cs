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
            // czyszczenie pola przy dodawaniu nowego pacjenta
            // /dopełnianie zerami z lewej strony przy edycji danych istniejącego pacjenta
            // /usuwanie z pola niedozwolonych znaków
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
            e.Handled = !patientDetailsPresenter.KindOfKey(e.Key, "NUM");
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pól tekstowych, zezwalających na wpisywanie do nich cyfr, liter i spacji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // usuwanie z pola niedozwolonych znaków
            patientDetailsPresenter.TextBoxChanged(e.Source as TextBox, "LAN");
        }

        /// <summary>
        /// Obsługa zdarzenia wciśnięcia klawisza podczas edycji pól tekstowych, zezwalających na wpisywanie do nich cyfr, liter i spacji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // sprawdzenie czy wciśniety został dozwolony klawisz
            e.Handled = !patientDetailsPresenter.KindOfKey(e.Key, "LAN");
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pól tekstowych, zezwalających na wpisywanie do nich tylko liter i spacji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LetterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // usuwanie z pola niedozwolonych znaków
            patientDetailsPresenter.TextBoxChanged(e.Source as TextBox, "LET");
        }

        /// <summary>
        /// Obsługa zdarzenia wciśnięcia klawisza podczas edycji pól tekstowych, zezwalających na wpisywanie do nich tylko liter i spacji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LetterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // sprawdzenie czy wciśniety został dozwolony klawisz
            e.Handled = !patientDetailsPresenter.KindOfKey(e.Key, "LET");
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany daty w polu na datę urodzenia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BirthDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // sprawdzenie czy wybrana data mieści się w możliwym do sprawdzenia z pesel'em przedziale
            patientDetailsPresenter.BirthDateChanged();

            // walidacja pesel'u
            patientDetailsPresenter.ValidatePesel();
        }

        /// <summary>
        /// Obsuga zdarzenia zmiany wyboru elementu z listy płci.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // warunek ze względów bezpieczeństwa kodu
            if(Pesel != null)
                // walidacja pesel'u
                patientDetailsPresenter.ValidatePesel();
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola tekstowego na miejscowość.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void City_TextChanged(object sender, TextChangedEventArgs e)
        {
            // usuwanie z pola niedozwolonych znaków
            patientDetailsPresenter.CityChanged();
        }

        /// <summary>
        /// Obsługa zdarzenia wciśnięcia klawisza podczas edycji pola tekstowego na miejscowość.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void City_KeyDown(object sender, KeyEventArgs e)
        {
            // sprawdzenie czy wciśniety został dozwolony klawisz
            e.Handled = !patientDetailsPresenter.CityKeyDown(e.Key);
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola tekstowego na kod pocztowy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostalCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            // usuwanie z pola niedozwolonych znaków
            patientDetailsPresenter.PostalCodeChanged();
        }

        /// <summary>
        /// Obsługa zdarzenia wciśnięcia klawisza podczas edycji pola tekstowego na kod pocztowy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostalCode_KeyDown(object sender, KeyEventArgs e)
        {
            // sprawdzenie czy wciśniety został dozwolony klawisz
            e.Handled = !patientDetailsPresenter.KindOfKey(e.Key, "NUM");
        }

        // TODO: w obsłudze kliknięcia przycisku "Zapisz" należy odbierać focus elementowi, który go aktualnie posiada, chyba, że jest to sam przycisk

        #endregion // Events handlers
    }
}
