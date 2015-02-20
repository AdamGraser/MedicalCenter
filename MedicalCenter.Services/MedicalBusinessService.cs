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

        /// <summary>
        /// Usługa bazodanowa dla funkcjonalności obejmującej pacjentów.
        /// </summary>
        PatientService patientService;

        /// <summary>
        /// Usługa bazodanowa dla funkcjonalności obejmującej użytkowników systemu i pracowników.
        /// </summary>
        UserService userService;

        /// <summary>
        /// Realizacja złożonych scenariuszy w zakresie użytkowników systemu i pracowników.
        /// </summary>
        UserBusinessService userBusinessService;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący usługi bazodanowe na potrzeby usług biznesowych obejmujących medyczne funkcje placówki.
        /// </summary>
        public MedicalBusinessService()
        {
            medicalService = new MedicalService();
            patientService = new PatientService();
            userService = new UserService();
            userBusinessService = new UserBusinessService();
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Pobiera z bazy danych informacje o wszystkich istniejących obecnie w tej placówce poradniach medycznych.
        /// Pobrane informacje dodaje do kolekcji clinicsList.
        /// </summary>
        /// <returns>Lista informacji o poradniach (pary &lt;id,nazwa&gt;).</returns>
        public SortedDictionary<int, string> GetClinics()
        {
            // pobranie z bazy danych informacji o poradniach
            List<M_DictionaryClinic> list = medicalService.SelectClinics();

            SortedDictionary<int, string> clinicsList = new SortedDictionary<int, string>();

            // przepisanie ID i nazw poradni do wskazanej kolekcji Dictionary
            foreach (M_DictionaryClinic entity in list)
                clinicsList.Add(entity.Id, entity.Name);

            return clinicsList;
        }

        /// <summary>
        /// Pobiera nazwę wskazanej poradni medycznej.
        /// </summary>
        /// <param name="clinicId">ID poradni, której nazwa ma zostać pobrana.</param>
        /// <returns>
        /// Nazwa poradni medycznej,
        /// null jeśli nie znaleziono poradni o podanym ID.
        /// </returns>
        public string GetClinicName(int clinicId)
        {
            if (clinicId > 0)
            {
                M_DictionaryClinic clinic = medicalService.SelectClinic(x => x.Id == clinicId);

                if (clinic != null)
                    return clinic.Name;
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Pobiera liczbę wizyt zarejestrowanych do wskazanego lekarza na dany dzień.
        /// </summary>
        /// <param name="doctorId">ID lekarza, dla którego wizyty mają zostać zliczone.</param>
        /// <param name="date">Dzień, z którego wizyty mają zostać zliczone. Wartość null powoduje, że ta metoda zwraca wartość -1.</param>
        /// <returns>
        /// Liczba wizyt zarejestrowanych do wskazanego lekarza na dany dzień,
        /// -1 jeśli nie znaleziono lekarza o podanym ID lub drugi argument to null.
        /// </returns>
        public int TodaysVisitsCount(int doctorId, DateTime date)
        {
            if (date != null && userBusinessService.GetWorkerName(doctorId) != null)
                return medicalService.SelectVisits(x => x.DoctorId == doctorId && x.DateOfVisit.Date == date.Date).Count();
            else
                return -1;
        }

        /// <summary>
        /// Tworzy listę planowych godzin rozpoczęcia wizyt, zawierającą także informacje o zarejestrowanych wizytach.
        /// </summary>
        /// <param name="doctorId">ID lekarza, którego tworzona lista dotyczy.</param>
        /// <param name="date">Dzień, z którego lista ma zostać utworzona. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Lista obiektów z informacjami o wizytach (lub "pustych"),
        /// null jeśli lekarz nie przyjmuje we wskazanym dniu, nie znaleziono lekarza o podanym ID lub drugi argument to null.
        /// </returns>
        public List<DailyVisitsListItem> GetTodaysVisits(int doctorId, DateTime date)
        {
            List<DailyVisitsListItem> todaysVisits = null;

            if (date != null && doctorId > 0)
            {
                // jeśli wybrany dzień jest świętem lub pracownik jest nieobecny w tym dniu, to lista wizyt jest pusta
                if (!userBusinessService.IsHoliday(date) && !userBusinessService.IsWorkerAbsent(doctorId, date))
                {
                    // pobranie grafika wskazanego lekarza
                    A_Schedule schedule = userService.SelectSchedule(x => x.WorkerId == doctorId && x.ValidFrom <= date && (x.ValidTo == null || x.ValidTo >= date));

                    if (schedule != null)
                    {
                        DateTime dateOfVisit = date;
                        bool hasSchedule = false;

                        // sprawdzenie czy w wybranym dniu tygodnia dany lekarz w ogóle pracuje
                        switch (date.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                if (schedule.D1From != null && schedule.D1To != null)
                                {
                                    hasSchedule = true;
                                    
                                    //godzina startowa DXFrom
                                    dateOfVisit = new DateTime(date.Year, date.Month, date.Day
                                                             , schedule.D1From.Value.Hour, schedule.D1From.Value.Minute, schedule.D1From.Value.Second);
                                }
                                break;

                            case DayOfWeek.Tuesday:
                                if (schedule.D2From != null && schedule.D2To != null)
                                {
                                    hasSchedule = true;
                                    dateOfVisit = new DateTime(date.Year, date.Month, date.Day
                                                             , schedule.D2From.Value.Hour, schedule.D2From.Value.Minute, schedule.D2From.Value.Second);
                                }
                                break;

                            case DayOfWeek.Wednesday:
                                if (schedule.D3From != null && schedule.D3To != null)
                                {
                                    hasSchedule = true;
                                    dateOfVisit = new DateTime(date.Year, date.Month, date.Day
                                                             , schedule.D3From.Value.Hour, schedule.D3From.Value.Minute, schedule.D3From.Value.Second);
                                }
                                break;

                            case DayOfWeek.Thursday:
                                if (schedule.D4From != null && schedule.D4To != null)
                                {
                                    hasSchedule = true;
                                    dateOfVisit = new DateTime(date.Year, date.Month, date.Day
                                                             , schedule.D4From.Value.Hour, schedule.D4From.Value.Minute, schedule.D4From.Value.Second);
                                }
                                break;

                            case DayOfWeek.Friday:
                                if (schedule.D5From != null && schedule.D5To != null)
                                {
                                    hasSchedule = true;
                                    dateOfVisit = new DateTime(date.Year, date.Month, date.Day
                                                             , schedule.D5From.Value.Hour, schedule.D5From.Value.Minute, schedule.D5From.Value.Second);
                                }
                                break;

                            case DayOfWeek.Saturday:
                                if (schedule.D6From != null && schedule.D6To != null)
                                {
                                    hasSchedule = true;
                                    dateOfVisit = new DateTime(date.Year, date.Month, date.Day
                                                             , schedule.D6From.Value.Hour, schedule.D6From.Value.Minute, schedule.D6From.Value.Second);
                                }
                                break;
                        }

                        // jeśli lekarz przyjmuje w danym dniu tygodnia
                        if (hasSchedule)
                        {
                            // pobranie listy wizyt zarejestrowanych do wybranego lekarza na wskazany dzień
                            List<M_Visit> temp = new List<M_Visit>(medicalService.SelectVisits(x => x.DoctorId == doctorId && x.DateOfVisit.Date == date.Date));

                            todaysVisits = new List<DailyVisitsListItem>();
                            M_Patient patient;

                            // stworzenie listy planowych godzin rozpoczęcia wizyt
                            for (int i = 0; i < userBusinessService.GetVisitsPerDay(doctorId, date); ++i)
                            {
                                // jeśli są jeszcze jakieś zarejestrowane wizyty
                                if (temp.Count > 0)
                                {
                                    // jeśli następna wizyta jest za więcej niż 20 minut lub została anulowana, wstawiamy do listy "puste miejsce"
                                    if (temp[0].DateOfVisit > dateOfVisit || temp[0].State > 2)
                                    {
                                        todaysVisits.Add(new DailyVisitsListItem(dateOfVisit));
                                    }
                                    else
                                    {
                                        // w przeciwnym razie wstawiamy do listy informacje o zarejestrowanej wizycie
                                        patient = patientService.SelectPatient(x => x.Id == temp[0].PatientId);
                                        todaysVisits.Add(new DailyVisitsListItem(temp[0].DateOfVisit, patient.LastName, patient.FirstName, temp[0].State, temp[0].IsEmergency));
                                        temp.RemoveAt(0);
                                    }
                                }
                                // jeśli brak już zarejestrowanych wizyt, wstawiamy do listy "puste miejsce"
                                else
                                    todaysVisits.Add(new DailyVisitsListItem(dateOfVisit));

                                dateOfVisit = dateOfVisit.AddMinutes(20.0);
                            }
                        }
                    }
                }
            }

            return todaysVisits;
        }

        /// <summary>
        /// Zapisuje w bazie danych nową wizytę
        /// </summary>
        /// <param name="visit">Informacje do zapisania.</param>
        /// <returns>
        /// true jeśli zapisano pomyślnie,
        /// null jeśli podane informacje nie przeszły walidacji po stronie bazy,
        /// false jeśli wystąpił inny błąd lub podany argument to null.
        /// </returns>
        public bool? RegisterVisit(Visit visit)
        {
            bool? retval = false;

            if (visit != null)
            {
                // stworzenie nowej encji, przepisanie wartości
                M_Visit entity = new M_Visit();
                entity.DateOfVisit = visit.DateOfVisit;
                entity.DoctorId = visit.DoctorId;
                entity.IsEmergency = visit.IsEmergency;
                entity.PatientId = visit.PatientId;

                // próba wstawienia nowego rekordu do tabeli
                retval = medicalService.InsertVisit(entity);
            }

            return retval;
        }

        #endregion // Public methods
    }
}
