using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Doctor
{
    /// <summary>
    /// Reprezentuje jedną pozycję z listy wykonanych/zleconych procedur medycznych.
    /// </summary>
    public class MedicalTreatment
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
        /// Przechowuje ID rekordu z tabeli A_Workers, odpowiadającego wykonawcy tej procedury medycznej.
        /// </summary>
        public int DoerId { get; private set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli A_Workers, odpowiadającego kierownikowi laboratorium, który zweryfikował to badanie laboratoryjne.
        /// </summary>
        public int VerifierId { get; private set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli A_Workers, odpowiadającego kierownikowi laboratorium, który jako ostatni edytował ten rekord.
        /// </summary>
        public int EditorId { get; private set; }

        /// <summary>
        /// Przechowuje datę zlecenia/wykonania procedury medycznej.
        /// Jest to wartość z kolumny Ordered z tabeli M_MedicalTreatments.
        /// </summary>
        public System.DateTime Ordered { get; set; }

        /// <summary>
        /// Przechowuje planową datę wykonania procedury medycznej.
        /// Jest to wartość z kolumny DateOfExecution z tabeli M_MedicalTreatments.
        /// </summary>
        public Nullable<System.DateTime> DateOfExecution { get; set; }

        /// <summary>
        /// Przechowuje datę rozpoczęcia wykonywania procedury medycznej.
        /// Jest to wartość z kolumny StartOfExecution z tabeli M_MedicalTreatments.
        /// </summary>
        public Nullable<System.DateTime> StartOfExecution { get; private set; }

        /// <summary>
        /// Przechowuje datę zakończenia wykonywania (wykonania) procedury medycznej.
        /// Jest to wartość z kolumny EndOfExecution z tabeli M_MedicalTreatments.
        /// </summary>
        public Nullable<System.DateTime> EndOfExecution { get; private set; }

        /// <summary>
        /// Przechowuje datę weryfikacji tego badania laboratoryjnego przez kierownika laboratorium.
        /// Jest to wartość z kolumny Verified z tabeli M_MedicalTreatments.
        /// </summary>
        public Nullable<System.DateTime> Verified { get; private set; }

        /// <summary>
        /// Przechowuje datę ostatniej edycji tego rekordu.
        /// Jest to wartość z kolumny LastEdit z tabeli M_MedicalTreatments.
        /// </summary>
        public Nullable<System.DateTime> LastEdit { get; set; }

        /// <summary>
        /// Przechowuje stan procedury medycznej.
        /// Jest to wartość z kolumny State z tabeli M_MedicalTreatments.
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_DictionaryMedicalTreatments, odpowiadającego wybranej do wykonania procedurze medycznej.
        /// </summary>
        public int MedicalTreatmentId { get; set; }

        /// <summary>
        /// Określa czy ta procedura medyczna została zlecona prywatnie, na wniosek pacjenta.
        /// Jest to wartość z kolumny IsPrivate z tabeli M_MedicalTreatments.
        /// </summary>
        public bool IsPrivate { get; private set; }

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

        /// <summary>
        /// Przechowuje komentarze/uwagi kierownika laboratorium, jakie dodał podczas weryfikacji tego badania laboratoryjnego.
        /// Jest to wartość z kolumny Comments z tabeli M_MedicalTreatments.
        /// </summary>
        public string Comments { get; private set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący domyślne wartości i 3 wymagane argumenty.
        /// </summary>
        /// <param name="VisitId">ID rekordu z tabeli M_Visits, odpowiadającemu tej wizycie.</param>
        /// <param name="DoctorId">ID rekordu z tabeli A_Workers, odpowiadającego danemu lekarzowi.</param>
        /// <param name="PatientId">ID rekordu z tabeli M_Patients, reprezentującego danego pacjenta.</param>
        public MedicalTreatment(int VisitId, int DoctorId, int PatientId)
        {
            this.Id = 0;
            this.VisitId = VisitId;
            this.DoctorId = DoctorId;
            this.PatientId = PatientId;
            this.DoerId = 0;
            this.VerifierId = 0;
            this.EditorId = 0;
            this.MedicalTreatmentId = 0;
            IsPrivate = false;
        }

        /// <summary>
        /// Konstruktor zapisujący w tworzonym obiekcie wartości podane w argumentach.
        /// </summary>
        /// <param name="Id">ID rekordu z tabeli M_MedicalTreatmens, reprezentującego tę procedurę medyczną.</param>
        /// <param name="VisitId">ID rekordu z tabeli M_Visits, odpowiadającemu tej wizycie.</param>
        /// <param name="DoctorId">ID rekordu z tabeli A_Workers, odpowiadającego danemu lekarzowi.</param>
        /// <param name="PatientId">ID rekordu z tabeli M_Patients, reprezentującego danego pacjenta.</param>
        /// <param name="DoerId">ID rekordu z tabeli A_Workers, odpowiadającego wykonawcy tej procedury medycznej.</param>
        /// <param name="VerifierId">ID rekordu z tabeli A_Workers, odpowiadającego kierownikowi laboratorium, który zweryfikował to badanie laboratoryjne.</param>
        /// <param name="EditorId">ID rekordu z tabeli A_Workers, odpowiadającego kierownikowi laboratorium, który jako ostatni edytował ten rekord.</param>
        /// <param name="Ordered">Wartość z kolumny Ordered z tabeli M_MedicalTreatments.</param>
        /// <param name="DateOfExecution">Wartość z kolumny DateOfExecution z tabeli M_MedicalTreatments.</param>
        /// <param name="StartOfExecution">Wartość z kolumny StartOfExecution z tabeli M_MedicalTreatments.</param>
        /// <param name="EndOfExecution">Wartość z kolumny EndOfExecution z tabeli M_MedicalTreatments.</param>
        /// <param name="Verified">Wartość z kolumny Verified z tabeli M_MedicalTreatments.</param>
        /// <param name="LastEdit">Wartość z kolumny LastEdit z tabeli M_MedicalTreatments.</param>
        /// <param name="State">Wartość z kolumny State z tabeli M_MedicalTreatments.</param>
        /// <param name="MedicalTreatment">ID rekordu z tabeli M_DictionaryMedicalTreatments, odpowiadającego wybranej do wykonania procedurze medycznej.</param>
        /// <param name="IsPrivate">Wartość z kolumny IsPrivate z tabeli M_MedicalTreatments.</param>
        /// <param name="Description">Wartość z kolumny Description z tabeli M_MedicalTreatments.</param>
        /// <param name="Result">Wartość z kolumny Result z tabeli M_MedicalTreatments.</param>
        /// <param name="Comments">Wartość z kolumny Comments z tabeli M_MedicalTreatments.</param>
        public MedicalTreatment(int Id,
                                int VisitId,
                                int DoctorId,
                                int PatientId,
                                int DoerId,
                                int VerifierId,
                                int EditorId,
                                System.DateTime Ordered,
                                Nullable<System.DateTime> DateOfExecution,
                                Nullable<System.DateTime> StartOfExecution,
                                Nullable<System.DateTime> EndOfExecution,
                                Nullable<System.DateTime> Verified,
                                Nullable<System.DateTime> LastEdit,
                                byte State,
                                int MedicalTreatment,
                                bool IsPrivate,
                                string Description,
                                string Result,
                                string Comments)
        {
            this.Id = Id;
            this.VisitId = VisitId;
            this.DoctorId = DoctorId;
            this.PatientId = PatientId;
            this.DoerId = DoerId;
            this.VerifierId = VerifierId;
            this.EditorId = EditorId;
            this.Ordered = Ordered;
            this.DateOfExecution = DateOfExecution;
            this.StartOfExecution = StartOfExecution;
            this.EndOfExecution = EndOfExecution;
            this.Verified = Verified;
            this.LastEdit = LastEdit;
            this.State = State;
            this.MedicalTreatmentId = MedicalTreatment;
            this.IsPrivate = IsPrivate;
            this.Description = Description;
            this.Result = Result;
            this.Comments = Comments;
        }

        #endregion // Ctors
    }
}
