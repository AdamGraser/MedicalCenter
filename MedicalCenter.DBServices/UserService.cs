using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Data;
using MedicalCenter.Models.LoggingIn;

namespace MedicalCenter.DBServices
{
    /// <summary>
    /// Zestaw podstawowych operacji na tabelach związanych z użytkownikami systemu (pracownikami).
    /// </summary>
    public class UserService
    {
        #region Private fields

        /// <summary>
        /// Obiekt kontekstu bazodanowego, używany do wykonywania operacji na bazie danych.
        /// </summary>
        MedicalCenterDBContainer db;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Bezargumentowy konstruktor inicjalizujący obiekt kontekstu bazodanowego.
        /// </summary>
        public UserService()
        {
            db = new MedicalCenterDBContainer();
        }

        #endregion // Ctors

        #region Public methods

        #region Select

        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanym użytkowniku.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Users, odpowiadający szukanemu użytkownikowi,
        /// lub null, jeżeli nie znaleziono użytkownika odpowiadającego podanym warunkom.
        /// </returns>
        public A_User SelectUser(System.Linq.Expressions.Expression<Func<A_User, bool>> predicate)
        {
            return db.A_Users.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanym pracowniku.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Workers,
        /// lub null, jeżeli nie znaleziono pracownika odpowiadającego podanym warunkom.
        /// </returns>
        public A_Worker SelectWorker(System.Linq.Expressions.Expression<Func<A_Worker, bool>> predicate)
        {
            return db.A_Workers.FirstOrDefault(predicate);
        }


        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanych pracownikach.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Lista obiektów reprezentujących rekordy z tabeli A_Workers,
        /// lub null, jeżeli nie znaleziono pracowników odpowiadających podanym warunkom.
        /// </returns>
        public IEnumerable<A_Worker> SelectWorkers(Func<A_Worker, bool> predicate)
        {
            return db.A_Workers.Where(predicate).OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.SecondName);
        }

        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanym stanowisku służbowym.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_DictionaryJobTitles,
        /// lub null, jeżeli nie znaleziono stanowiska odpowiadającego podanym warunkom.
        /// </returns>
        public A_DictionaryJobTitle SelectJobTitle(System.Linq.Expressions.Expression<Func<A_DictionaryJobTitle, bool>> predicate)
        {
            return db.A_DictionaryJobTitles.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Pobiera z tabeli A_DictionaryRooms rekord, którego id zawiera spełniający podane kryteria rekord z tabeli A_WorkersRooms.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_DictionaryRooms,
        /// lub null, jeżeli nie znaleziono gabinetu odpowiadającego podanym warunkom.
        /// </returns>
        public A_DictionaryRoom SelectRoom(System.Linq.Expressions.Expression<Func<A_WorkersRoom, bool>> predicate)
        {
            // A_DictionaryRoom <---> A_WorkersRoom <---> A_Workers

            // znalezienie rekordu wiążącego A_Workers.Id z A_DictionaryRoom.Id
            A_WorkersRoom temp = db.A_WorkersRooms.FirstOrDefault(predicate);

            // zwrócenie numeru gabinetu lub null, jeśli nie znaleziono (lub nie znaleziono powiązania między gabinetem a pracownikiem)
            if (temp != null)
            {
                // pobranie obiektu z numerem telefonu
                A_DictionaryRoom room = db.A_DictionaryRooms.FirstOrDefault(x => x.Id == temp.RoomId);

                if (room != null)
                {
                    // jeśli pobrany wpis był aktualizowany, szukana jest jego najnowsza wersja
                    while (room.New > 0)
                        room = db.A_DictionaryRooms.FirstOrDefault(x => x.Id == room.New);

                    // jeśli wpis został oznaczony jako usunięty, zwracany jest null
                    if (room.IsDeleted)
                        room = null;
                }

                return room;
            }
            else
                return null;
        }

        /// <summary>
        /// Pobiera z bazy danych grafik spełniający podane warunki.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Schedules,
        /// lub null, jeżeli nie znaleziono grafika odpowiadającego podanym warunkom.
        /// </returns>
        public A_Schedule SelectSchedule(System.Linq.Expressions.Expression<Func<A_Schedule, bool>> predicate)
        {
            return db.A_Schedules.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Pobiera z bazy danych informację o nieobecności, spełniającą podane kryteria.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Absences,
        /// lub null, jeżeli nie znaleziono nieobecności odpowiadającej podanym warunkom.
        /// </returns>
        public A_Absence SelectAbsence(System.Linq.Expressions.Expression<Func<A_Absence, bool>> predicate)
        {
            return db.A_Absences.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Pobiera z bazy danych spełniający podane kryteria obiekt, zawierający datę, która wskazuje dzień wolny od pracy.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Holidays
        /// lub null, jeżeli nie znaleziono obiektu spełniającego podane kryteria (najczęściej: wskazany dzień nie jest dniem ustawowo wolnym od pracy).
        /// </returns>
        public A_Holiday SelectHoliday(System.Linq.Expressions.Expression<Func<A_Holiday, bool>> predicate)
        {
            return db.A_Holidays.FirstOrDefault(predicate);
        }

        #endregion // Select

        #endregion // Public methods
    }
}
