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

        #endregion // Public methods
    }
}
