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
        /// <param name="patient">Informacje do zapisania. Wartość null powoduje, że ta metoda zwraca wartość false.</param>
        /// <returns>
        /// true jeśli zapisano pomyślnie,
        /// null jeśli podane informacje nie przeszły walidacji po stronie bazy,
        /// false jeśli wystąpił inny błąd lub podany argument to null.
        /// </returns>
        public bool? SavePatient(Patient patient)
        {
            bool? retval = false;

            if (patient != null)
            {
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
                if (patient.Id > 0)
                    retval = patientService.UpdatePatient(entity);
                else
                    retval = patientService.InsertPatient(entity);
            }

            return retval;
        }

        /// <summary>
        /// Pobiera z bazy dane o pacjencie i zwraca jego nazwisko i imię.
        /// </summary>
        /// <param name="patientId">ID pacjenta.</param>
        /// <returns>
        /// Nazwisko i imię wskazanego pacjenta, oddzielone spacją,
        /// null jeśli nie znaleziono pacjenta o podanym ID.
        /// </returns>
        public string GetPatientName(int patientId)
        {
            if (patientId > 0)
            {
                M_Patient patient = patientService.SelectPatient(x => x.Id == patientId);

                if (patient != null)
                    return patient.LastName + " " + patient.FirstName;
                else
                    return null;
            }
            else
                return null;
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
                if (e.Id == 0)
                    continue;

                patients.Add(new Patient(e.Id, e.LastName, e.FirstName, e.SecondName, e.BirthDate, e.Gender, e.Pesel,
                                         e.Street, e.BuildingNumber, e.Apartment, e.PostalCode, e.City, e.Post, e.IsInsured));
            }

            return patients;
        }

        /// <summary>
        /// Pobiera z bazy danych informacje o pacjencie posiadającym wskazany nr PESEL.
        /// </summary>
        /// <param name="pesel">Nr PESEL szukanego pacjenta.</param>
        /// <returns>Obiekt z danymi pacjenta lub null jeśli nie znaleziono w tabeli podanego numeru PESEL.</returns>
        public Patient GetPatient(long pesel)
        {
            Patient patient = null;
            
            // pobranie z bazy rekordu dot. pacjenta o podanym nr PESEL
            M_Patient e = patientService.SelectPatient(x => x.Pesel == pesel);

            // jeśli znaleziono w tabeli podany PESEL, zwracany jest obiekt z danymi pacjenta
            if (e != null)
                patient = new Patient(e.Id, e.LastName, e.FirstName, e.SecondName, e.BirthDate, e.Gender, e.Pesel,
                                      e.Street, e.BuildingNumber, e.Apartment, e.PostalCode, e.City, e.Post, e.IsInsured);

            return patient;
        }

        #endregion // Public methods
    }
}
