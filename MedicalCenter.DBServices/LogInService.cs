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
    /// Usługa realizująca scenariusz logowania.
    /// </summary>
    public class LogInService
    {
        #region Private fields

        /// <summary>
        /// Obiekt kontekstu bazodanowego, używany do wykonywania operacji na bazie danych.
        /// </summary>
        MedicalCenterDBContainer db;

        /// <summary>
        /// Przechowuje nazwisko aktualnie zalogowanej osoby.
        /// Jest to wartość z kolumny LastName z tabeli A_Workers.
        /// </summary>
        string lastName;

        /// <summary>
        /// Przechowuje imię aktualnie zalogowanej osoby.
        /// Jest to wartość z kolumny FirstName z tabeli A_Workers.
        /// </summary>
        string firstName;

        #endregion // Private fields

        #region Public properties

        /// <summary>
        /// Zwraca imię i nazwisko aktualnie zalogowanej osoby, oddzielone spacją.
        /// </summary>
        public string Name
        {
            get
            {
                return firstName + " " + lastName;
            }

            private set;
        }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Bezargumentowy konstruktor inicjalizujący obiekt kontekstu bazodanowego.
        /// </summary>
        public LogInService()
        {
            db = new MedicalCenterDBContainer();
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Szuka w bazie danych użytkownika o zawartym w argumencie loginie i hashu hasła.
        /// Po wyszukiwaniu czyści w obiekcie źródłowym pola przechowujące login i hash hasła.
        /// </summary>
        /// <param name="user">Obiekt klasy MedicalCenter.Models.LoggingIn.User, przechowujący wpisany przez użytkownika login i hash wpisanego hasła.</param>
        /// <returns>ID pracownika powiązanego z zalogowanym użytkownikiem lub 0, jeśli nie znaleziono użytkownika o podanym loginie i hashu hasła.</returns>
        public int FindUser(User user)
        {
            // znajdź w tabeli A_Users użytkownika o podanym loginie i hashu hasła
            A_User usr = db.A_Users.FirstOrDefault(x => (x.Login == user.Login) && (x.Password == user.Password));

            // wyczyść login i hasło ze źródłowego obiektu
            user.Login = string.Empty;
            user.Password = string.Empty;

            // jeśli znaleziono takiego użytkownika, zapisz imię i nazwisko powiązanego z nim pracownika
            if (usr.WorkerId > 0)
            {
                A_Worker wrk = db.A_Workers.FirstOrDefault(x => x.Id == usr.WorkerId);

                lastName = wrk.LastName;
                firstName = wrk.FirstName;
            }

            return usr.WorkerId;
        }

        #endregion // Public methods
    }
}
