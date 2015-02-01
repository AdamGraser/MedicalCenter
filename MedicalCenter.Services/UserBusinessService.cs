using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Data;
using MedicalCenter.DBServices;
using MedicalCenter.Models.LoggingIn;

namespace MedicalCenter.Services
{
    /// <summary>
    /// Obsługa zaawansowanych scenariuszy logiki biznesowej aplikacji w zakresie użytkowników systemu (pracowników).
    /// </summary>
    public class UserBusinessService
    {
        #region Private fields

        /// <summary>
        /// Usługa bazodanowa dla funkcjonalności obejmującej użytkowników systemu.
        /// </summary>
        UserService userService;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący usługę bazodanową na potrzeby usług biznesowych obejmujących użytkowników systemu.
        /// </summary>
        public UserBusinessService()
        {
            userService = new UserService();
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Sprawdza w bazie danych podane poświadczenia.
        /// Jeśli znaleziono w bazie odpowiadającego użytkownika, pobierane są również jego imię, nazwisko oraz nazwa i kod stanowiska.
        /// Informacje te zapisywane są w obiekcie wskazywanym przez argument.
        /// </summary>
        /// <param name="user">Obiekt zawierający podany login i hash hasła.</param>
        public void LogIn(User user)
        {
            // sprawdzenie czy w systemie istnieje użytkownik o podanym loginie i hashu hasła
            A_User usr = userService.SelectUser(x => x.Login == user.Login && x.Password == user.Password);
            
            // jeśli podane poświadczenia są prawidłowe
            if (usr != null && usr.WorkerId > 0)
            {
                // zapisanie ID pracownika
                user.Id = usr.WorkerId;

                // pobranie informacji o pracowniku, do którego przypisany jest sprawdzony użytkownik systemu
                A_Worker wrk = userService.SelectWorker(x => x.Id == usr.WorkerId);

                // jeśli rekord użytkownika w bazie zawiera prawidłowe ID pracownika
                if (wrk.Id > 0)
                {
                    // pobranie informacji o stanowisku służbowym pracownika
                    A_DictionaryJobTitle job = userService.SelectJobTitle(x => x.Id == wrk.JobTitle);

                    // jeśli rekord pracownika w bazie zawiera prawidłowe ID stanowiska
                    if (job.Id > 0)
                    {
                        // zapisanie imienia, nazwiska i nazwy stanowiska
                        user.Title = job.JobTitle + " - " + wrk.LastName + " " + wrk.FirstName;

                        // zapisanie kodu stanowiska
                        user.JobTitleCode = job.Code;
                    }
                }
            }
        }

        /// <summary>
        /// Zwraca wartość określającą czy wskazany pracownik jest nieobecny w danym dniu.
        /// </summary>
        /// <param name="workerId">ID pracownika, którego nieobecność ma zostać sprawdzona.</param>
        /// <param name="date">Dzień, na który ma przypadać rzekoma nieobecność pracownika.</param>
        /// <returns>true jeśli pracownik jest nieobecny w danym dniu, w przeciwnym razie false</returns>
        public bool IsWorkerAbsent(int workerId, DateTime date)
        {
            A_Absence absence = userService.SelectAbsence(x => x.WorkerId == workerId && x.DateFrom <= date && (x.DateTo == null || x.DateTo >= date));

            if (absence == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Zwraca wartość określającą czy wskazany pracownik pracuje w danym dniu.
        /// </summary>
        /// <param name="workerId">ID pracownika, którego grafik ma zostać sprawdzony.</param>
        /// <param name="date">Dzień, pod kątem którego grafik ma zostać sprawdzony.</param>
        /// <returns>true jeśli pracownik pracuje w danym dniu, w przeciwnym razie false</returns>
        public bool IsWorking(int workerId, DateTime date)
        {
            bool retval = false;

            A_Schedule schedule = userService.SelectSchedule(x => x.WorkerId == workerId && x.ValidFrom <= date && (x.ValidTo == null || x.ValidTo >= date));

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (schedule.D1From != null && schedule.D1To != null)
                        retval = true;
                    break;

                case DayOfWeek.Tuesday:
                    if (schedule.D2From != null && schedule.D2To != null)
                        retval = true;
                    break;

                case DayOfWeek.Wednesday:
                    if (schedule.D3From != null && schedule.D3To != null)
                        retval = true;
                    break;

                case DayOfWeek.Thursday:
                    if (schedule.D4From != null && schedule.D4To != null)
                        retval = true;
                    break;

                case DayOfWeek.Friday:
                    if (schedule.D5From != null && schedule.D5To != null)
                        retval = true;
                    break;

                case DayOfWeek.Saturday:
                    if (schedule.D6From != null && schedule.D6To != null)
                        retval = true;
                    break;
            }

            return retval;
        }

        /// <summary>
        /// Pobiera z bazy danych ID stanowiska określonego podanym kodem.
        /// </summary>
        /// <param name="code">Kod stanowiska, którego klucz ma zostać pobrany.</param>
        /// <returns>ID stanowiska lub 0, jeśli nie znaleziono stanowiska o podanym kodzie.</returns>
        public int GetJobTitleId(string code)
        {
            A_DictionaryJobTitle job = userService.SelectJobTitle(x => x.Code.StartsWith(code));

            if (job != null)
                return job.Id;
            else
                return 0;
        }

        /// <summary>
        /// Pobiera z bazy danych listę pracowników na tym samym stanowisku.
        /// </summary>
        /// <param name="jobTitleId">ID stanowiska</param>
        /// <returns>Lista pracowników o tym samym stanowisku.</returns>
        public List<A_Worker> GetSameWorkers(int jobTitleId)
        {
            return new List<A_Worker>(userService.SelectWorkers(x => x.JobTitle == jobTitleId));
        }

        /// <summary>
        /// Pobiera z bazy numer gabinetu, w którym przyjmuje/przyjmował dany lekarz we wskazanym dniu.
        /// </summary>
        /// <param name="workerId">ID lekarza, którego gabinet ma zostać znaleziony.</param>
        /// <param name="date">Data, dla której obowiązywać ma przypisanie wskazanego lekarza do poszukiwanego gabinetu.</param>
        /// <returns>Numer gabinetu lub null, jeśli nie znaleziono.</returns>
        public string GetRoomNumber(int workerId, DateTime date)
        {
            A_DictionaryRoom entity = userService.SelectRoom(x => x.WorkerId == workerId && x.DateFrom <= date && (x.DateTo == null || x.DateTo >= date));

            if (entity != null)
                return entity.Number;
            else
                return null;
        }

        /// <summary>
        /// Pobiera z bazy ID poradni, w ramach której przyjmuje/przyjmował dany lekarz we wskazanym dniu.
        /// </summary>
        /// <param name="workerId">ID lekarza, dla którego poradnia ma zostać znaleziona.</param>
        /// <param name="date">Data, dla której obowiązywać ma przynależność wskazanego lekarza do poszukiwanej poradni.</param>
        /// <returns>ID poradni lub 0, jeśli nie znaleziono.</returns>
        public int GetClinicId(int workerId, DateTime date)
        {
            A_DictionaryRoom entity = userService.SelectRoom(x => x.WorkerId == workerId && x.DateFrom <= date && (x.DateTo == null || x.DateTo >= date));

            if (entity != null)
                return entity.ClinicId;
            else
                return 0;
        }

        /// <summary>
        /// Pobiera z bazy wybrany grafik wskazanego lekarza, a następnie oblicza ilu maksymalnie pacjentów może przyjąć w podanym dniu tygodnia ten lekarz.
        /// </summary>
        /// <param name="doctorId">ID lekarza, którego grafik ma zostać rozpatrzony.</param>
        /// <param name="date">Data, którą objemować ma grafik.</param>
        /// <returns>
        /// Maksymalną liczbę pacjentów, jaką lekarz może przyjąć w danym dniu tygodnia, wg. wybranego grafika,
        /// lub 0 jeśli nie znaleziono grafika dla wskazanego lekarza i/lub objemującego podaną datę.
        /// </returns>
        public int GetVisitsPerDay(int doctorId, DateTime date)
        {
            A_Schedule schedule = userService.SelectSchedule(x => x.WorkerId == doctorId && x.ValidFrom <= date && (x.ValidTo == null || x.ValidTo >= date));

            int visitsPerDay = 0;

            if (schedule != null)
            {
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        if(schedule.D1From != null && schedule.D1To != null)
                            visitsPerDay = (int)(schedule.D1To.Value.Subtract(schedule.D1From.Value).TotalMinutes / 20.0);
                        break;

                    case DayOfWeek.Tuesday:
                        if (schedule.D2From != null && schedule.D2To != null)
                            visitsPerDay = (int)(schedule.D2To.Value.Subtract(schedule.D2From.Value).TotalMinutes / 20.0);
                        break;

                    case DayOfWeek.Wednesday:
                        if (schedule.D3From != null && schedule.D3To != null)
                            visitsPerDay = (int)(schedule.D3To.Value.Subtract(schedule.D3From.Value).TotalMinutes / 20.0);
                        break;

                    case DayOfWeek.Thursday:
                        if (schedule.D4From != null && schedule.D4To != null)
                            visitsPerDay = (int)(schedule.D4To.Value.Subtract(schedule.D4From.Value).TotalMinutes / 20.0);
                        break;

                    case DayOfWeek.Friday:
                        if (schedule.D5From != null && schedule.D5To != null)
                            visitsPerDay = (int)(schedule.D5To.Value.Subtract(schedule.D5From.Value).TotalMinutes / 20.0);
                        break;

                    case DayOfWeek.Saturday:
                        if (schedule.D6From != null && schedule.D6To != null)
                            visitsPerDay = (int)(schedule.D6To.Value.Subtract(schedule.D6From.Value).TotalMinutes / 20.0);
                        break;
                }
            }

            return visitsPerDay;
        }

        /// <summary>
        /// Pobiera z bazy danych informacje o wybranym pracowniku, zwraca jego nazwisko i imię.
        /// </summary>
        /// <param name="workerId">ID pracownika, którego dane mają zostać pobrane.</param>
        /// <returns>Oddzielone spacją nazwisko i imię wskazanego pracownika lub null, jeśli nie znaleziono pracownika o podanym ID.</returns>
        public string GetWorkerName(int workerId)
        {
            A_Worker worker = userService.SelectWorker(x => x.Id == workerId);

            if (worker == null)
                return null;
            else
                return worker.LastName + " " + worker.FirstName;
        }

        #endregion // Public methods
    }
}
