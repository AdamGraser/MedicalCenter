using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Doctor
{
    /// <summary>
    /// Reprezentuje zbiór informacji o pojedynczej wizycie.
    /// </summary>
    class Visit
    {
        #region Public properties

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Visits, odpowiadającemu tej wizycie.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Patients, reprezentującego danego pacjenta.
        /// </summary>
        public int PatientId { get; private set; }

        /// <summary>
        /// Przechowuje nazwisko pacjenta.
        /// Jest to wartość z kolumny LastName z tabeli z tabeli M_Patients.
        /// </summary>
        public string PatientLastName { get; private set; }

        /// <summary>
        /// Przechowuje pierwsze imię pacjenta.
        /// Jest to wartość z kolumny FirstName z tabeli M_Patients.
        /// </summary>
        public string PatientFirstName { get; private set; }

        /// <summary>
        /// Przechowuje planową datę i godzinę odbycia się wskazanej wizyty.
        /// Jest to wartość z kolumny DateOfVisit z tabeli M_Visits.
        /// </summary>
        public DateTime DateOfVisit { get; private set; }

        /// <summary>
        /// Przechowuje datę zakończenia tej wizyty.
        /// Jest to wartość z kolumny Ended z tabeli M_Visits.
        /// </summary>
        public Nullable<System.DateTime> Ended { get; set; }

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

        /// <summary>
        /// Przechowuje opis wizyty.
        /// Jest to wartość z kolumny Description z tabeli M_Visits.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Przechowuje diagnozę wystawioną w trakcie trwania tej wizyty.
        /// Jest to wartość z kolumny Diagnosis z tabeli M_Visits.
        /// </summary>
        public string Diagnosis { get; set; }

        /// <summary>
        /// Przechowuje wszystkie zlecone podczas tej wizyty badania laboratoryjne.
        /// </summary>
        public List<MedicalTreatment> LaboratoryTests { get; private set; }

        /// <summary>
        /// Przechowuje wszystkie wykonane podczas tej wizyty badania fizykalne.
        /// </summary>
        public List<MedicalTreatment> PhysicalTests { get; private set; }

        /// <summary>
        /// Przechowuje wszystkie wykonane podczas tej wizyty zabiegi medyczne.
        /// </summary>
        public List<MedicalTreatment> MedicalOperations { get; private set; }

        /// <summary>
        /// Przechowuje wszystkie wykonane podczas tej wizyty inne procedury medyczne.
        /// </summary>
        public List<MedicalTreatment> OtherProcedures { get; private set; }

        /// <summary>
        /// Przechowuje wszystkie wypisane podczas tej wizyty recepty (lista numerów recept).
        /// </summary>
        public List<string> Prescriptions { get; private set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_DictionaryDisease, reprezentującego schorzenie zdiagnozowane podczas tej wizyty,
        /// z powodu którego wypisane zostało dla pacjenta zwolnienie lekarskie.
        /// </summary>
        public int Disease { get; set; }

        /// <summary>
        /// Przechowuje numer druku zwolnienia lekarskiego, wypisanego podczas tej wizyty.
        /// Jest to wartość z kolumny L4Number z tabeli M_L4Disease.
        /// </summary>
        public string L4number { get; set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący w tworzonym obiekcie wartości podane w argumentach.
        /// </summary>
        /// <param name="Id">ID rekordu z tabeli M_Visits, odpowiadającemu tej wizycie.</param>
        /// <param name="PatientId">Wartość z kolumny LastName z tabeli z tabeli M_Patients.</param>
        /// <param name="PatientLastName">Wartość z kolumny LastName z tabeli z tabeli M_Patients.</param>
        /// <param name="PatientFirstName">Wartość z kolumny FirstName z tabeli M_Patients.</param>
        /// <param name="DateOfVisit">Wartość z kolumny DateOfVisit z tabeli M_Visits.</param>
        /// <param name="Ended">Wartość z kolumny Ended z tabeli M_Visits.</param>
        /// <param name="State">Wartość z kolumny State z tabeli M_Visits.</param>
        /// <param name="IsEmergency">Wartość z kolumny IsEmergency z tabeli M_Visits.</param>
        /// <param name="Description">Wartość z kolumny Description z tabeli M_Visits.</param>
        /// <param name="Diagnosis">Wartość z kolumny Diagnosis z tabeli M_Visits.</param>
        /// <param name="LaboratoryTests">Lista zleconych podczas tej wizyty badań laboratoryjnych.</param>
        /// <param name="PhysicalTests">Lista wykonanych podczas tej wizyty badań fizykalnych.</param>
        /// <param name="MedicalOperations">Lista wykonanych podczas tej wizyty zabiegów medycznych.</param>
        /// <param name="OtherProcedures">Lista wykonanych podczas tej wizyty innych procedur medycznych.</param>
        /// <param name="Prescriptions">Lista numerów recept wypisanych podczas tej wizyty.</param>
        /// <param name="Disease">ID rekordu z tabeli M_DictionaryDisease, reprezentującego schorzenie zdiagnozowane podczas tej wizyty, 
        /// z powodu którego wypisane zostało dla pacjenta zwolnienie lekarskie.</param>
        /// <param name="L4number">Wartość z kolumny L4Number z tabeli M_L4Disease.</param>
        public Visit(int Id,
            int PatientId,
            string PatientLastName,
            string PatientFirstName,
            DateTime DateOfVisit,
            Nullable<System.DateTime> Ended,
            byte State,
            bool IsEmergency,
            string Description,
            string Diagnosis,
            List<MedicalTreatment> LaboratoryTests,
            List<MedicalTreatment> PhysicalTests,
            List<MedicalTreatment> MedicalOperations,
            List<MedicalTreatment> OtherProcedures,
            List<string> Prescriptions,
            int Disease,
            string L4number)
        {
            this.Id = Id;
            this.PatientId = PatientId;
            this.PatientLastName = PatientLastName;
            this.PatientFirstName = PatientFirstName;
            this.DateOfVisit = DateOfVisit;
            this.Ended = Ended;
            this.State = State;
            this.IsEmergency = IsEmergency;
            this.Description = Description;
            this.Diagnosis = Diagnosis;
            this.LaboratoryTests = (LaboratoryTests == null) ? new List<MedicalTreatment>() : LaboratoryTests;
            this.PhysicalTests = (PhysicalTests == null) ? new List<MedicalTreatment>() : PhysicalTests;
            this.MedicalOperations = (MedicalOperations == null) ? new List<MedicalTreatment>() : MedicalOperations;
            this.OtherProcedures = (OtherProcedures == null) ? new List<MedicalTreatment>() : OtherProcedures;
            this.Prescriptions = (Prescriptions == null) ? new List<string>() : Prescriptions;
            this.Disease = Disease;
            this.L4number = L4number;
        }

        #endregion // Ctors
    }
}
