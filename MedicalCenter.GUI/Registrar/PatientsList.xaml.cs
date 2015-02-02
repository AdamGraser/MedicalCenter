﻿using System;
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
    /// Reprezentuje widok listy pacjentów do wyboru podczas rejestracji wizyty, nakładany na widok listy wizyt u danego lekarza w wybranym dniu.
    /// </summary>
    public partial class PatientsList : UserControl
    {
        #region Private fields

        /// <summary>
        /// Prezenter obsługujący m.in. zdarzenia użytkownika.
        /// </summary>
        PatientsListPresenter patientsListPresenter;

        #endregion // Private fields

        #region Public properties

        /// <summary>
        /// Źródło elementów wyświetlanych na liście pacjentów.
        /// </summary>
        public List<Patient> Patients;

        /// <summary>
        /// Lista pacjentów.
        /// </summary>
        public List<Patient> SourcePatients;

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor ustawiający kontekst danych dla listy pacjentów.
        /// </summary>
        public PatientsList()
        {
            InitializeComponent();

            // utworzenie prezentera dla tego widoku
            patientsListPresenter = new PatientsListPresenter(this);

            // ustawienie kontekstu danych dla listy pacjentów
            PatientsList.DataContext = this;
        }

        #endregion // Ctors

        #region Events handlers

        /// <summary>
        /// Obsługa zdarzenia załadowania listy pacjentów (element interfejsu).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientsList_Loaded(object sender, RoutedEventArgs e)
        {
            // wypełnienie listy pacjentów
            patientsListPresenter.GetPatientsList();
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola tekstowego filtru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // filtrowanie listy pacjentów
            patientsListPresenter.FilterPatientName();
        }
        
        /// <summary>
        /// Obsługa zdarzenia zmiany zaznaczenia na liście pacjentów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // aktywacja/dezaktywacja przycisków "Szczegóły" i "Wybierz"
            patientsListPresenter.PatientSelected();
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
        /// Obsługa zdarzenia kliknięcia przycisku "Szczegóły".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Details_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Wybierz".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Choose_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion // Events handlers
    }
}
