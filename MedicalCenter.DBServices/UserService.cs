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
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Users, odpowiadający szukanemu użytkownikowi,
        /// null jeżeli nie znaleziono użytkownika odpowiadającego podanym warunkom lub podany argument to null.
        /// </returns>
        public A_User SelectUser(System.Linq.Expressions.Expression<Func<A_User, bool>> predicate)
        {
            if (predicate != null)
                return db.A_Users.FirstOrDefault(predicate);
            else
                return null;
        }

        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanym pracowniku.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Workers,
        /// null jeżeli nie znaleziono pracownika odpowiadającego podanym warunkom lub podany argument to null.
        /// </returns>
        public A_Worker SelectWorker(System.Linq.Expressions.Expression<Func<A_Worker, bool>> predicate)
        {
            if (predicate != null)
                return db.A_Workers.FirstOrDefault(predicate);
            else
                return null;
        }


        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanych pracownikach.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Lista obiektów reprezentujących rekordy z tabeli A_Workers,
        /// null jeżeli nie znaleziono pracowników odpowiadających podanym warunkom lub podany argument to null.
        /// </returns>
        public IEnumerable<A_Worker> SelectWorkers(Func<A_Worker, bool> predicate)
        {
            if (predicate != null)
                return db.A_Workers.Where(predicate).OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.SecondName);
            else
                return null;
        }

        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanym stanowisku służbowym.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_DictionaryJobTitles,
        /// null jeżeli nie znaleziono stanowiska odpowiadającego podanym warunkom lub podany argument to null.
        /// </returns>
        public A_DictionaryJobTitle SelectJobTitle(System.Linq.Expressions.Expression<Func<A_DictionaryJobTitle, bool>> predicate)
        {
            if (predicate != null)
                return db.A_DictionaryJobTitles.FirstOrDefault(predicate);
            else
                return null;
        }

        /// <summary>
        /// Pobiera z tabeli A_DictionaryRooms rekord, którego id zawiera spełniający podane kryteria rekord z tabeli A_WorkersRooms.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_DictionaryRooms,
        /// null jeżeli nie znaleziono gabinetu odpowiadającego podanym warunkom lub podany argument to null.
        /// </returns>
        public A_DictionaryRoom SelectRoom(System.Linq.Expressions.Expression<Func<A_WorkersRoom, bool>> predicate)
        {
            // A_DictionaryRoom <---> A_WorkersRoom <---> A_Workers

            if (predicate != null)
            {
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
            else
                return null;
        }

        /// <summary>
        /// Pobiera z bazy danych grafik spełniający podane warunki.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Schedules,
        /// null jeżeli nie znaleziono grafika odpowiadającego podanym warunkom lub podany argument to null.
        /// </returns>
        public A_Schedule SelectSchedule(System.Linq.Expressions.Expression<Func<A_Schedule, bool>> predicate)
        {
            if (predicate != null)
                return db.A_Schedules.FirstOrDefault(predicate);
            else
                return null;
        }

        /// <summary>
        /// Pobiera z bazy danych informację o nieobecności, spełniającą podane kryteria.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Absences,
        /// null jeżeli nie znaleziono nieobecności odpowiadającej podanym warunkom lub podany argument to null.
        /// </returns>
        public A_Absence SelectAbsence(System.Linq.Expressions.Expression<Func<A_Absence, bool>> predicate)
        {
            if (predicate != null)
                return db.A_Absences.FirstOrDefault(predicate);
            else
                return null;
        }

        /// <summary>
        /// Pobiera z bazy danych spełniający podane kryteria obiekt, zawierający datę, która wskazuje dzień wolny od pracy.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Holidays
        /// null jeżeli nie znaleziono obiektu spełniającego podane kryteria (najczęściej: wskazany dzień nie jest dniem ustawowo wolnym od pracy) lub podany argument to null.
        /// </returns>
        public A_Holiday SelectHoliday(System.Linq.Expressions.Expression<Func<A_Holiday, bool>> predicate)
        {
            if (predicate != null)
                return db.A_Holidays.FirstOrDefault(predicate);
            else
                return null;
        }

        #endregion // Select

        #endregion // Public methods
    }
}
