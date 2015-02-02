using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Data;
using MedicalCenter.DBServices;
using MedicalCenter.Models.Registrar;

namespace MedicalCenter.Services
{
    /// <summary>
    /// Obsługa zaawansowanych scenariuszy logiki biznesowej aplikacji w zakresie pacjentów.
    /// </summary>
    public class PatientBusinessService
    {
        #region Private fields

        /// <summary>
        /// Usługa bazodanowa dla funkcjonalności obejmującej pacjentów.
        /// </summary>
        PatientService patientService;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący usługę bazodanową na potrzeby usług biznesowych obejmujących pacjentów.
        /// </summary>
        public PatientBusinessService()
        {
            patientService = new PatientService();
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Zapisuje w bazie danych podane informacje o pacjencie.
        /// </summary>
        /// <param name="patient">Informacje do zapisania.</param>
        /// <returns>true jeśli zapisano pomyślnie, null jeśli podane informacje nie przeszły walidacji po stronie bazy, false jeśli wystąpił inny błąd</returns>
        public bool? SavePatient(Patient patient)
        {
            bool? retval = true;
            
            // stworzenie nowej encji, przepisanie wartości
            M_Patient entity = new M_Patient();
            entity.Apartment = patient.Apartment;
            entity.BirthDate = patient.BirthDate;
            entity.BuildingNumber = patient.BuildingNumber;
            entity.City = patient.City;
            entity.FirstName = patient.FirstName;
            entity.Gender = patient.Gender;
            entity.IsInsured = patient.IsInsured;
            entity.LastName = patient.LastName;
            entity.Pesel = patient.Pesel;
            entity.Post = patient.Post;
            entity.PostalCode = patient.PostalCode;
            entity.SecondName = patient.SecondName;
            entity.Street = patient.Street;

            // ID > 0 -> zmiana danych istniejącego w bazie pacjenta
            if (entity.Id > 0)
                retval = patientService.UpdatePatient(entity);
            else
                retval = patientService.InsertPatient(entity);

            return retval;
        }

        /// <summary>
        /// Pobiera z bazy dane o pacjencie i zwraca jego nazwisko i imię.
        /// </summary>
        /// <param name="patientId">ID pacjenta.</param>
        /// <returns>Nazwisko i imię wskazanego pacjenta, oddzielone spacją.</returns>
        public string GetPatientName(int patientId)
        {
            M_Patient patient = patientService.SelectPatient(x => x.Id == patientId);

            return patient.LastName + " " + patient.FirstName;
        }

        /// <summary>
        /// Pobiera z bazy listę wszystkich pacjentów.
        /// </summary>
        /// <returns>Lista wszystkich pacjentów.</returns>
        public List<Patient> GetPatients()
        {
            // pobranie kolekcji wszystkich rekordów z tabeli M_Patients
            IEnumerable<M_Patient> entities = patientService.SelectPatients();

            List<Patient> patients = new List<Patient>();

            // konwersja do listy obiektów Patient
            foreach (M_Patient e in entities)
            {
                patients.Add(new Patient(e.Id, e.LastName, e.FirstName, e.SecondName, e.BirthDate, e.Gender, e.Pesel,
                                         e.Street, e.BuildingNumber, e.Apartment, e.PostalCode, e.City, e.Post, e.IsInsured));
            }

            return patients;
        }

        #endregion // Public methods
    }
}
