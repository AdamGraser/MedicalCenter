using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Doctor
{
    /// <summary>
    /// Reprezentuje wykonywaną przez lekarza podczas wizyty procedurę medyczną.
    /// </summary>
    class DoneMedicalTreatment
    {
        #region Public properties

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_MedicalTreatmens, reprezentującego tę procedurę medyczną.
        /// Wartość 0 oznacza użycie domyślnego konstruktora, co jest rozumiane jako utworzenie nowego obiektu (rekordu).
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Visits, odpowiadającemu tej wizycie.
        /// </summary>
        public int VisitId { get; private set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli A_Workers, odpowiadającego danemu lekarzowi.
        /// </summary>
        public int DoctorId { get; private set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Patients, reprezentującego danego pacjenta.
        /// </summary>
        public int PatientId { get; private set; }

        /// <summary>
        /// Przechowuje datę zakończenia wykonywania (wykonania) procedury medycznej.
        /// Jest to wartość z kolumny EndOfExecution z tabeli M_MedicalTreatments.
        /// </summary>
        public Nullable<System.DateTime> EndOfExecution { get; set; }

        /// <summary>
        /// Przechowuje stan procedury medycznej.
        /// Jest to wartość z kolumny State z tabeli M_MedicalTreatments.
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_DictionaryMedicalTreatments, odpowiadającego wybranej do wykonania procedurze medycznej.
        /// </summary>
        public int MedicalTreatment { get; set; }

        /// <summary>
        /// Przechowuje opis wykonywanej procedury medycznej.
        /// Jest to wartość z kolumny Description z tabeli M_MedicalTreatments.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Przechowuje wynik wykonywanej procedury medycznej.
        /// Jest to wartość z kolumny Result z tabeli M_MedicalTreatments.
        /// </summary>
        public string Result { get; set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący domyślne wartości i 3 wymagane argumenty.
        /// </summary>
        /// <param name="VisitId">ID rekordu z tabeli M_Visits, odpowiadającemu tej wizycie.</param>
        /// <param name="DoctorId">ID rekordu z tabeli A_Workers, odpowiadającego danemu lekarzowi.</param>
        /// <param name="PatientId">ID rekordu z tabeli M_Patients, reprezentującego danego pacjenta.</param>
        public DoneMedicalTreatment(int VisitId, int DoctorId, int PatientId)
        {
            this.Id = 0;
            this.VisitId = VisitId;
            this.DoctorId = DoctorId;
            this.PatientId = PatientId;
            this.MedicalTreatment = 0;
        }

        /// <summary>
        /// Konstruktor zapisujący w tworzonym obiekcie wartości podane w argumentach.
        /// </summary>
        /// <param name="Id">ID rekordu z tabeli M_MedicalTreatmens, reprezentującego tę procedurę medyczną.</param>
        /// <param name="VisitId">ID rekordu z tabeli M_Visits, odpowiadającemu tej wizycie.</param>
        /// <param name="DoctorId">ID rekordu z tabeli A_Workers, odpowiadającego danemu lekarzowi.</param>
        /// <param name="PatientId">ID rekordu z tabeli M_Patients, reprezentującego danego pacjenta.</param>
        /// <param name="EndOfExecution">Wartość z kolumny EndOfExecution z tabeli M_MedicalTreatments.</param>
        /// <param name="State">Wartość z kolumny State z tabeli M_MedicalTreatments.</param>
        /// <param name="MedicalTreatment">ID rekordu z tabeli M_DictionaryMedicalTreatments, odpowiadającego wybranej do wykonania procedurze medycznej.</param>
        /// <param name="Description">Wartość z kolumny Description z tabeli M_MedicalTreatments.</param>
        /// <param name="Result">Wartość z kolumny Result z tabeli M_MedicalTreatments.</param>
        public DoneMedicalTreatment(int Id,
                                    int VisitId,
                                    int DoctorId,
                                    int PatientId,
                                    Nullable<System.DateTime> EndOfExecution,
                                    byte State,
                                    int MedicalTreatment,
                                    string Description,
                                    string Result)
        {
            this.Id = Id;
            this.VisitId = VisitId;
            this.DoctorId = DoctorId;
            this.PatientId = PatientId;
            this.EndOfExecution = EndOfExecution;
            this.State = State;
            this.MedicalTreatment = MedicalTreatment;
            this.Description = Description;
            this.Result = Result;
        }

        #endregion // Ctors
    }
}
