using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Models.LoggingIn;
using MedicalCenter.Data;

namespace MedicalCenter.DBServices
{
    /// <summary>
    /// Obsługa podstawowych operacji na tabelach związanych z użytkownikami systemu (pracownikami).
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

        /// <summary>
        /// Pobiera z bazy danych informacje o użytkowniku identyfikowanym przez podany login i hash hasła.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_Users, odpowiadający szukanemu użytkownikowi,
        /// lub obiekt z wartościami domyślnymi, jeżeli nie znaleziono użytkownika o podanym loginie i/lub hashu hasła.
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
        /// lub obiekt z wartościami domyślnymi, jeżeli podane ID jest spoza zakresu.
        /// </returns>
        public A_Worker SelectWorker(System.Linq.Expressions.Expression<Func<A_Worker, bool>> predicate)
        {
            return db.A_Workers.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanym stanowisku służbowym.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli A_DictionaryJobTitle,
        /// lub obiekt z wartościami domyślnymi, jeżeli podane ID jest spoza zakresu.
        /// </returns>
        public A_DictionaryJobTitle SelectJobTitle(System.Linq.Expressions.Expression<Func<A_DictionaryJobTitle, bool>> predicate)
        {
            return db.A_DictionaryJobTitles.FirstOrDefault(predicate);
        }

        #endregion // Public methods
    }
}
