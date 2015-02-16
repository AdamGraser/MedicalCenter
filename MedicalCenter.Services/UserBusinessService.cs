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
        /// <param name="user">Obiekt zawierający podany login i hash hasła. Wartość null powoduje, że ta metoda nie wykonuje żadnej akcji.</param>
        public void LogIn(User user)
        {
            if (user != null)
            {
                // sprawdzenie czy w systemie istnieje użytkownik o podanym loginie i hashu hasła
                A_User usr = userService.SelectUser(x => x.Login == user.Login && x.Password == user.Password);

                // jeśli podane poświadczenia są prawidłowe
                if (usr != null && usr.WorkerId > 0)
                {
                    // jeśli konto jest aktywne
                    if (usr.Expires == null || usr.Expires.Value > DateTime.Today)
                    {
                        // pobranie informacji o pracowniku, do którego przypisany jest sprawdzony użytkownik systemu
                        A_Worker wrk = userService.SelectWorker(x => x.Id == usr.WorkerId);

                        // jeśli rekord użytkownika w bazie zawiera prawidłowe ID pracownika
                        if (wrk.Id > 0)
                        {
                            // zapisanie ID pracownika
                            user.Id = usr.WorkerId;

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
                    // jeśli konto zostało dezaktywowane, ID pracownika zostaje przypisana wartość -1, co jest rozpoznawane w prezenterze formularza logowania
                    else
                        user.Id = -1;
                }
            }
        }

        /// <summary>
        /// Pobiera z bazy danych informacje o wybranym pracowniku, zwraca jego nazwisko i imię.
        /// </summary>
        /// <param name="workerId">ID pracownika, którego dane mają zostać pobrane.</param>
        /// <returns>Oddzielone spacją nazwisko i imię wskazanego pracownika lub null, jeśli nie znaleziono pracownika o podanym ID.</returns>
        public string GetWorkerName(int workerId)
        {
            if (workerId > 0)
            {
                A_Worker worker = userService.SelectWorker(x => x.Id == workerId);

                if (worker == null)
                    return null;
                else
                    return worker.LastName + " " + worker.FirstName;
            }
            else
                return null;
        }

        /// <summary>
        /// Zwraca wartość określającą czy wskazany pracownik jest nieobecny w danym dniu.
        /// </summary>
        /// <param name="workerId">ID pracownika, którego nieobecność ma zostać sprawdzona.</param>
        /// <param name="date">Dzień, na który ma przypadać rzekoma nieobecność pracownika. Wartość null powoduje, że ta metoda zwraca false.</param>
        /// <returns>
        /// true jeśli pracownik jest nieobecny w danym dniu,
        /// false jeśli jest obecny, nie znaleziono pracownika o podanym ID lub jeśli drugi argument to null.
        /// </returns>
        public bool IsWorkerAbsent(int workerId, DateTime date)
        {
            bool retval = false;

            if (date != null && workerId > 0)
            {
                // szukanie nieobecności wskazanego pracownika, która zaczęła się najpóźniej we wskazanym dniu
                // i nie ma określonej daty zakończenia lub kończy się nie wcześniej niż we wskazanym dniu
                A_Absence absence = userService.SelectAbsence(x => x.WorkerId == workerId && x.DateFrom <= date && (x.DateTo == null || x.DateTo >= date));

                // jeśli nie znaleziono takiej nieobecności wskazanego pracownika, to jest on obecny
                if (absence != null)
                    retval = true;
            }

            return retval;
        }

        /// <summary>
        /// Zwraca wartość określającą czy wskazany pracownik pracuje w danym dniu.
        /// </summary>
        /// <param name="workerId">ID pracownika, którego grafik ma zostać sprawdzony.</param>
        /// <param name="date">Dzień, pod kątem którego grafik ma zostać sprawdzony. Wartość null powoduje, że ta metoda zwraca false.</param>
        /// <returns>
        /// true jeśli pracownik pracuje w danym dniu,
        /// false jeśli nie pracuje, nie znaleziono pracownika o podanym ID lub jeśli drugi argument to null.
        /// </returns>
        public bool IsWorking(int workerId, DateTime date)
        {
            bool retval = false;

            if (date != null && workerId > 0)
            {
                // szukanie grafika danego pracownika, który obowiązuje w podanym dniu
                // (obowiązuje co najmniej od wskazanego dnia i nie ma daty utraty ważności lub data ta jest nie wcześniejsza niż data podana w argumencie)
                A_Schedule schedule = userService.SelectSchedule(x => x.WorkerId == workerId && x.ValidFrom <= date && (x.ValidTo == null || x.ValidTo >= date));

                // sprawdzanie, czy pracownik ma określone godziny pracy na wskazany dzień tygodnia
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
            }

            return retval;
        }

        /// <summary>
        /// Zwraca wartość określającą czy wskazany dzień jest dniem ustawowo wolnym od pracy.
        /// Z podanej struktury DateTime brana jest do testu wartość zwracana przez właściwość Date (data z wyzerowaną częścią czasu).
        /// </summary>
        /// <param name="date">Dzień do sprawdzenia. Wartość null powoduje, że ta metoda zwraca false.</param>
        /// <returns>true jeśli wskazany dzień jest ustawowo wolny od pracy, w przeciwnym razie false (lub jeśli podano null jako argument).</returns>
        public bool IsHoliday(DateTime date)
        {
            bool holiday = false;

            if (date != null)
            {
                // jeśli wskazany dzień jest świętem wolnym od pracy lub jest to niedziela, zwracana jest wartość "true"
                if (userService.SelectHoliday(x => x.Date == date) != null || date.DayOfWeek == DayOfWeek.Sunday)
                    holiday = true;
            }

            return holiday;
        }

        /// <summary>
        /// Pobiera z bazy wybrany grafik wskazanego lekarza, a następnie oblicza ilu maksymalnie pacjentów może przyjąć w podanym dniu tygodnia ten lekarz.
        /// </summary>
        /// <param name="doctorId">ID lekarza, którego grafik ma zostać rozpatrzony.</param>
        /// <param name="date">Data, którą objemować ma grafik. Wartość null powoduje, że ta metoda zwraca wartość -1.</param>
        /// <returns>
        /// Maksymalną liczbę pacjentów, jaką lekarz może przyjąć w danym dniu tygodnia, wg. wybranego grafika,
        /// 0 jeśli nie znaleziono grafika dla wskazanego lekarza i/lub objemującego podaną datę lub nie znaleziono lekarza o podanym ID,
        /// -1 jeśli drugi argument to null.
        /// </returns>
        public int GetVisitsPerDay(int doctorId, DateTime date)
        {
            int visitsPerDay = 0;

            if (date != null)
            {
                if (doctorId > 0)
                {
                    // szukanie grafika danego pracownika, który obowiązuje w podanym dniu
                    // (obowiązuje co najmniej od wskazanego dnia i nie ma daty utraty ważności lub data ta jest nie wcześniejsza niż data podana w argumencie)
                    A_Schedule schedule = userService.SelectSchedule(x => x.WorkerId == doctorId && x.ValidFrom <= date && (x.ValidTo == null || x.ValidTo >= date));

                    if (schedule != null)
                    {
                        switch (date.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                if (schedule.D1From != null && schedule.D1To != null)
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
                }
            }
            else
                visitsPerDay = -1;

            return visitsPerDay;
        }

        /// <summary>
        /// Pobiera z bazy danych godziny pracy wskazanego pracownika w danym dniu.
        /// </summary>
        /// <param name="workerId">ID pracownika, którego godziny pracy mają zostać pobrane.</param>
        /// <param name="date">Dzień, z którego godziny pracy pracownika mają zostać pobrane. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Napis zawierający godziny pracy wybranego pracownika w danym dniu, w formacie {godzina od} - {godzina do},
        /// null jeśli nie znaleziono grafika dla wskazanego pracownika i/lub objemującego podaną datę, nie znaleziono pracownika o podanym ID, albo drugi argument to null.
        /// </returns>
        public string GetWorkingHours(int workerId, DateTime date)
        {
            string workingHours = null;

            if (date != null && workerId > 0)
            {
                // szukanie grafika danego pracownika, który obowiązuje w podanym dniu
                // (obowiązuje co najmniej od wskazanego dnia i nie ma daty utraty ważności lub data ta jest nie wcześniejsza niż data podana w argumencie)
                A_Schedule schedule = userService.SelectSchedule(x => x.WorkerId == workerId && x.ValidFrom <= date && (x.ValidTo == null || x.ValidTo >= date));

                if (schedule != null)
                {
                    switch (date.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            if (schedule.D1From != null && schedule.D1To != null)
                                workingHours = schedule.D1From.Value.Hour + ":" + schedule.D1From.Value.Minute + " - " + schedule.D1To.Value.Hour + ":" + schedule.D1To.Value.Minute;
                            break;

                        case DayOfWeek.Tuesday:
                            if (schedule.D2From != null && schedule.D2To != null)
                                workingHours = schedule.D1From.Value.Hour + ":" + schedule.D1From.Value.Minute + " - " + schedule.D1To.Value.Hour + ":" + schedule.D1To.Value.Minute;
                            break;

                        case DayOfWeek.Wednesday:
                            if (schedule.D3From != null && schedule.D3To != null)
                                workingHours = schedule.D1From.Value.Hour + ":" + schedule.D1From.Value.Minute + " - " + schedule.D1To.Value.Hour + ":" + schedule.D1To.Value.Minute;
                            break;

                        case DayOfWeek.Thursday:
                            if (schedule.D4From != null && schedule.D4To != null)
                                workingHours = schedule.D1From.Value.Hour + ":" + schedule.D1From.Value.Minute + " - " + schedule.D1To.Value.Hour + ":" + schedule.D1To.Value.Minute;
                            break;

                        case DayOfWeek.Friday:
                            if (schedule.D5From != null && schedule.D5To != null)
                                workingHours = schedule.D1From.Value.Hour + ":" + schedule.D1From.Value.Minute + " - " + schedule.D1To.Value.Hour + ":" + schedule.D1To.Value.Minute;
                            break;

                        case DayOfWeek.Saturday:
                            if (schedule.D6From != null && schedule.D6To != null)
                                workingHours = schedule.D1From.Value.Hour + ":" + schedule.D1From.Value.Minute + " - " + schedule.D1To.Value.Hour + ":" + schedule.D1To.Value.Minute;
                            break;
                    }
                }
            }

            return workingHours;
        }

        /// <summary>
        /// Pobiera z bazy numer pomieszczenia, do którego przypisany jest/był dany pracownik we wskazanym dniu.
        /// </summary>
        /// <param name="workerId">ID pracownika, którego gabinet ma zostać znaleziony.</param>
        /// <param name="date">
        /// Data, dla której obowiązywać ma przypisanie wskazanego pracownika do poszukiwanego pomieszczenia.
        /// Wartość null powoduje, że ta metoda również zwraca null.
        /// </param>
        /// <returns>
        /// Numer gabinetu,
        /// null jeśli nie gabinetu, nie znaleziono pracownika o podanym ID lub drugi argument to null.
        /// </returns>
        public string GetRoomNumber(int workerId, DateTime date)
        {
            string roomNumber = null;

            if (date != null && workerId > 0)
            {
                // szukanie gabinetu, do którego wskazany pracownik jest/był przypisany w wybranym dniu
                A_DictionaryRoom entity = userService.SelectRoom(x => x.WorkerId == workerId && x.DateFrom <= date && (x.DateTo == null || x.DateTo >= date));

                if (entity != null)
                    roomNumber = entity.Number;
            }

            return roomNumber;
        }

        /// <summary>
        /// Pobiera z bazy ID poradni, w ramach której przyjmuje/przyjmował dany lekarz we wskazanym dniu.
        /// </summary>
        /// <param name="doctorId">ID lekarza, dla którego poradnia ma zostać znaleziona.</param>
        /// <param name="date">
        /// Data, dla której obowiązywać ma przynależność wskazanego lekarza do poszukiwanej poradni.
        /// Wartość null powoduje, że ta metoda zwraca wartość -1.
        /// </param>
        /// <returns>
        /// ID poradni większe od 0,
        /// 0 jeśli nie znaleziono takiej poradni lub nie znaleziono lekarza o podanym ID,
        /// -1 jeśli drugi argument to null.
        /// </returns>
        public int GetClinicId(int doctorId, DateTime date)
        {
            int clinicId = 0;

            if (date != null && doctorId > 0)
            {
                A_DictionaryRoom entity = userService.SelectRoom(x => x.WorkerId == doctorId && x.DateFrom <= date && (x.DateTo == null || x.DateTo >= date));

                if (entity != null)
                    clinicId = entity.ClinicId;
            }
            else
                clinicId = -1;

            return clinicId;
        }

        /// <summary>
        /// Pobiera z bazy danych ID stanowiska określonego podanym kodem.
        /// </summary>
        /// <param name="code">Kod stanowiska, którego klucz ma zostać pobrany. Wartość null powoduje, że ta metoda zwraca wartość -1.</param>
        /// <returns>
        /// ID stanowiska większe od 0,
        /// 0 jeśli nie znaleziono stanowiska o podanym kodzie,
        /// -1 jeśli drugi argument to null.
        /// </returns>
        public int GetJobTitleId(string code)
        {
            int jobTitleId = 0;

            if (code != null)
            {
                A_DictionaryJobTitle job = userService.SelectJobTitle(x => x.Code.StartsWith(code));

                if (job != null)
                    jobTitleId = job.Id;
            }
            else
                jobTitleId = -1;

            return jobTitleId;
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

        #endregion // Public methods
    }
}
