using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje jedną pozycję z listy zarejestrowanych wizyt.
    /// </summary>
    class VisitsListItem
    {
        // Public getters, private setters
        #region Public properties

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Visits, reprezentującego wskazaną wizytę.
        /// </summary>
        public int VisitId { get; private set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Patients, powiązanego ze wskazywanym przez VisitId rekordem z tabeli M_Visits.
        /// </summary>
        public int PatientId { get; private set; }

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

        /// <summary>
        /// Przechowuje planową datę i godzinę odbycia się wskazanej wizyty.
        /// Jest to wartość z kolumny DateOfVisit z tabeli M_Visits.
        /// </summary>
        public DateTime DateOfVisit { get; private set; }

        /// <summary>
        /// Przechowuje nazwisko lekarza, do którego została zarejestrowana wskazana wizyta.
        /// Jest to wartość z kolumny LastName z tabeli A_Workers.
        /// </summary>
        public string DoctorLastName { get; private set; }

        /// <summary>
        /// Przechowuje imię lekarza, do którego została zarejestrowana wskazana wizyta.
        /// Jest to wartość z kolumny FirstName z tabeli A_Workers.
        /// </summary>
        public string DoctorFirstName { get; private set; }

        /// <summary>
        /// Przechowuje numer gabinetu, w którym przyjmuje wskazany lekarz w ramach danej poradni medycznej.
        /// Jest to wartość z kolumny Number z tabeli A_DictionaryRoom.
        /// </summary>
        public string RoomNumber { get; private set; }

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
        /// Konstruktor zapisujący w tworzonym obiekcie wartości podane w argumentach.
        /// </summary>
        /// <param name="VisitId">ID rekordu z tabeli M_Visits.</param>
        /// <param name="PatientId">ID rekordu z tabeli M_Patients.</param>
        /// <param name="PatientLastName">Wartość z kolumny LastName z tabeli M_Patients.</param>
        /// <param name="PatientFirstName">Wartość z kolumny FirstName z tabeli M_Patients.</param>
        /// <param name="DateOfVisit">Wartość z kolumny DateOfVisit z tabeli M_Visits.</param>
        /// <param name="DoctorLastName">Wartość z kolumny LastName z tabeli A_Workers.</param>
        /// <param name="DoctorFirstName">Wartość z kolumny FirstName z tabeli A_Workers.</param>
        /// <param name="RoomNumber">Wartość z kolumny Number z tabeli A_DictionaryRoom.</param>
        /// <param name="State">Wartość z kolumny State z tabeli M_Visits.</param>
        /// <param name="IsEmergency">Wartość z kolumny IsEmergency z tabeli M_Visits.</param>
        public VisitsListItem(int VisitId,
                              int PatientId,
                              string PatientLastName,
                              string PatientFirstName,
                              DateTime DateOfVisit,
                              string DoctorLastName,
                              string DoctorFirstName,
                              string RoomNumber,
                              byte State,
                              bool IsEmergency)
        {
            this.VisitId = VisitId;
            this.PatientId = PatientId;
            this.PatientLastName = PatientLastName;
            this.PatientFirstName = PatientFirstName;
            this.DateOfVisit = DateOfVisit;
            this.DoctorLastName = DoctorLastName;
            this.DoctorFirstName = DoctorFirstName;
            this.RoomNumber = RoomNumber;
            this.State = State;
            this.IsEmergency = IsEmergency;
        }

        #endregion // Ctors
    }
}
