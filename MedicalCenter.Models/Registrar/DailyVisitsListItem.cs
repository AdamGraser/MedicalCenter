using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje jedną pozycję z listy wizyt z danego dnia u wskazanego lekarza (podczas rejestrowania wizyty).
    /// </summary>
    public class DailyVisitsListItem
    {
        #region Private fields

        /// <summary>
        /// Przechowuje nazwisko pacjenta, dla którego ta wizyta została zarejestrowana.
        /// Jest to wartość z kolumny LastName z tabeli M_Patients.
        /// </summary>
        string patientLastName;

        /// <summary>
        /// Przechowuje imię pacjenta, dla którego ta wizyta została zarejestrowana.
        /// Jest to wartość z kolumny FirstName z tabeli M_Patients.
        /// </summary>
        string patientFirstName;

        #endregion // Private fields

        // Public getters, private setters
        #region Public properties

        /// <summary>
        /// Przechowuje datę i czas rozpoczęcia wizyty.
        /// Jest to wartość z kolumny DateOfVisit z tabeli M_Visits.
        /// </summary>
        public DateTime DateOfVisit { get; private set; }

        /// <summary>
        /// Zwraca nazwisko i imię pacjenta, oddzielone spacją lub pusty string, jeśli brak nazwiska.
        /// </summary>
        public string PatientName
        {
            get
            {
                if (patientLastName != string.Empty)
                    return patientLastName + " " + patientFirstName;
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Przechowuje stan wizyty.
        /// Jest to wartość z kolumny State z tabeli M_Visits.
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// Określa czy wizyta została zarejestrowana jako nagły przypadek.
        /// Jest to wartość z kolumny IsEmergency z tabeli M_Visits.
        /// </summary>
        public bool IsEmergency { get; private set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Tworzy obiekt listy wizyt, reprezentujący "wolną godzinę" (na którą nikt dotychczas nie zarejestrował wizyty do wskazanego lekarza).
        /// </summary>
        /// <param name="DateOfVisit">Data i godzina, na którą wciąż można zarejestrować wizytę.</param>
        public DailyVisitsListItem(DateTime DateOfVisit)
        {
            this.DateOfVisit = DateOfVisit;
            this.patientFirstName = string.Empty;
            this.patientLastName = string.Empty;
            this.State = 0;
            this.IsEmergency = false;
        }

        /// <summary>
        /// Tworzy obiekt listy wizyt, reprezentujący zarejestrowaną wizytę.
        /// </summary>
        /// <param name="DateOfVisit">Data i godzina, na którą dana wizyta została zarejestrowana.</param>
        /// <param name="PatientLastName">Nazwisko pacjenta, dla którego dana wizyta została zarejestrowana.</param>
        /// <param name="PatientFirstName">Imię pacjenta, dla którego dana wizyta została zarejestrowana.</param>
        /// <param name="State">Wartość z kolumny State z tabeli M_Visits.</param>
        /// <param name="IsEmergency">Określa czy wskazana wizyta została zarejestrowana jako nagły przypadek.</param>
        public DailyVisitsListItem(DateTime DateOfVisit, string PatientLastName, string PatientFirstName, byte State, bool IsEmergency)
        {
            this.DateOfVisit = DateOfVisit;
            this.patientLastName = PatientLastName;
            this.patientFirstName = PatientFirstName;
            this.State = State;
            this.IsEmergency = IsEmergency;
        }

        #endregion // Ctors
    }
}
