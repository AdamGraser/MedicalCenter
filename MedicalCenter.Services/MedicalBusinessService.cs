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
    /// Obsługa zaawansowanych scenariuszy logiki biznesowej aplikacji w zakresie medycznej działalności placówki.
    /// </summary>
    public class MedicalBusinessService
    {
        #region Private fields

        /// <summary>
        /// Usługa bazodanowa dla funkcjonalności obejmującej medyczne funkcje placówki.
        /// </summary>
        MedicalService medicalService;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący usługę bazodanową na potrzeby usług biznesowych obejmujących medyczne funkcje placówki.
        /// </summary>
        public MedicalBusinessService()
        {
            medicalService = new MedicalService();
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Pobiera z bazy danych informacje o wszystkich istniejących obecnie w tej placówce poradniach medycznych.
        /// Pobrane informacje dodaje do kolekcji clinicsList.
        /// </summary>
        /// <param name="clinicsList">Kolekcja, do której dodane mają zostać informacje pobrane z bazy danych.</param>
        public void GetClinics(Dictionary<int, string> clinicsList)
        {
            // pobranie z bazy danych informacji o poradniach
            List<M_DictionaryClinic> list = medicalService.SelectClinics();

            // przepisanie ID i nazw poradni do wskazanej kolekcji Dictionary
            foreach (M_DictionaryClinic entity in list)
                clinicsList.Add(entity.Id, entity.Name);
        }

        /// <summary>
        /// Pobiera nazwę wskazanej poradni medycznej.
        /// </summary>
        /// <param name="clinicId">ID poradni, której nazwa ma zostać pobrana.</param>
        /// <returns>Nazwa poradni medycznej lub null, jeśli nie znaleziono poradni o podanym ID.</returns>
        public string GetClinicName(int clinicId)
        {
            M_DictionaryClinic clinic = medicalService.SelectClinic(x => x.Id == clinicId);

            if (clinic != null)
                return clinic.Name;
            else
                return null;
        }

        /// <summary>
        /// Pobiera liczbę wizyt zarejestrowanych do wskazanego lekarza na dany dzień.
        /// </summary>
        /// <param name="doctorId">ID lekarza, dla którego wizyty mają zostać zliczone.</param>
        /// <param name="date">Dzień, z którego wizyty mają zostać zliczone</param>
        /// <returns>Liczba wizyt zarejestrowanych do wskazanego lekarza na dany dzień.</returns>
        public int TodaysVisitsCount(int doctorId, DateTime date)
        {
            return medicalService.SelectVisits(x => x.DoctorId == doctorId && x.DateOfVisit.Date == date.Date).Count();
        }

        #endregion // Public methods
    }
}
