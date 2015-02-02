using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Data;
using MedicalCenter.Models.Registrar;
using MedicalCenter.Services;

namespace MedicalCenter.GUI.Registrar
{
    /// <summary>
    /// Obsługa zdarzeń użytkownika dla dodatkowego widoku listy pacjentów przy rejestracji wizyty.
    /// </summary>
    public class PatientsListPresenter
    {
        #region Private fields

        /// <summary>
        /// Logika biznesowa w zakresie pacjentów.
        /// </summary>
        PatientBusinessService patientBusinessService;

        /// <summary>
        /// Widok listy pacjentów.
        /// </summary>
        PatientsList view;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący referencję do zarządzanego widoku i tworzący obiekty usług logiki biznesowej.
        /// </summary>
        /// <param name="view">Widok listy pacjentów, zarządzany przez tego prezentera.</param>
        public PatientsListPresenter(PatientsList view)
        {
            patientBusinessService = new PatientBusinessService();
            this.view = view;
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Wypełnia listę pacjentów.
        /// </summary>
        public void GetPatientsList()
        {
            view.SourcePatients = patientBusinessService.GetPatients();
            view.Patients = new List<Patient>(view.SourcePatients);
        }

        /// <summary>
        /// FIltruje listę pacjentów.
        /// </summary>
        public void FilterPatientName()
        {
            // jeśli wpisano co najmniej 3 znaki, następuje filtrowanie pacjentów po nazwiskach
            if (view.FilterPatientName.Text.Length > 2)
                // pacjenci sortowani są najpierw wg. nazwisk, a następnie wg. imion (pierwszych)
                view.Patients = new List<Patient>(view.SourcePatients.Where(x => x.LastName.StartsWith(view.FilterPatientName.Text) ||
                                                                     view.FilterPatientName.Text.StartsWith(x.LastName)).OrderBy(x => x.LastName).ThenBy(x => x.FirstName));
            // jeśli wpisano mniej niż 3 znaki, wyświetlana jest pełna lista pacjentów
            else
                view.Patients = view.SourcePatients;
        }

        /// <summary>
        /// Aktywuje/dezaktywuje przyciski "Szczegóły" i "Wybierz" w zależności od zaznaczenia pacjenta bądź jego braku.
        /// </summary>
        public void PatientSelected()
        {
            // wybranie pacjenta -> aktywacja przycisków
            if (view.PatientsList.SelectedIndex > -1)
            {
                view.Details.IsEnabled = true;
                view.Choose.IsEnabled = true;
            }
            else
            {
                view.Details.IsEnabled = false;
                view.Choose.IsEnabled = false;
            }
        }

        #endregion // Public methods
    }
}
