using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Doctor
{
    /// <summary>
    /// Reprezentuje jedną pozycję z listy zarejestrowanych na dziś wizyt.
    /// </summary>
    class TodaysVisitsListItem
    {
        // Public getters, private setters
        #region Public properties

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Visits, reprezentującego wskazaną wizytę.
        /// </summary>
        public int VisitId { get; private set; }

        /// <summary>
        /// Przechowuje planową datę i godzinę odbycia się wskazanej wizyty.
        /// Jest to wartość z kolumny DateOfVisit z tabeli M_Visits.
        /// </summary>
        public DateTime DateOfVisit { get; private set; }

        /// <summary>
        /// Przechowuje nazwisko pacjenta, dla którego została zarejestrowana wskazana wizyta.
        /// Jest to wartość z kolumny LastName z tabeli M_Patients.
        /// </summary>
        public string PatientLastName { get; private set; }

        /// <summary>
        /// Przechowuje imię pacjenta, dla którego została zarejestrowana wskazana wizyta.
        /// Jest to wartość z kolumny FirstName z tabeli M_Patients.
        /// </summary>
        public string PatientFirstName { get; private set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Ukrycie domyślnego konstruktora.
        /// </summary>
        private TodaysVisitsListItem() { }

        /// <summary>
        /// Konstruktor zapisujący w tworzonym obiekcie wartości podane w argumentach.
        /// </summary>
        /// <param name="VisitId">ID rekordu z tabeli M_Visits.</param>
        /// <param name="DateOfVisit">Wartość z kolumny DateOfVisit z tabeli M_Visits.</param>
        /// <param name="PatientLastName">Wartość z kolumny LastName z tabeli M_Patients.</param>
        /// <param name="PatientFirstName">Wartość z kolumny FirstName z tabeli M_Patients.</param>
        public TodaysVisitsListItem(int VisitId, DateTime DateOfVisit, string PatientLastName, string PatientFirstName)
        {
            this.VisitId = VisitId;
            this.DateOfVisit = DateOfVisit;
            this.PatientLastName = PatientLastName;
            this.PatientFirstName = PatientFirstName;
        }

        #endregion // Ctors
    }
}
