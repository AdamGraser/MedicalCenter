using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.DBServices;
using MedicalCenter.Models.LoggingIn;
using MedicalCenter.Data;

namespace MedicalCenter.Services
{
    /// <summary>
    /// Obsługa zaawansowanych scenariuszy logiki biznesowej aplikacji w zakresie użytkowników systemu (pracowników).
    /// </summary>
    public class UserBusinessService
    {
        #region Private fields

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

        /// <summary>
        /// Przechowuje nazwę stanowiska aktualnie zalogowanej osoby.
        /// Jest to wartość z kolumny JobTitle z tabeli A_DictionaryJobTitle.
        /// </summary>
        string jobTitle;

        /// <summary>
        /// Usługa bazodanowa dla funkcjonalności obejmującej użytkowników systemu.
        /// </summary>
        UserService userService;

        #endregion // Private fields

        #region Public properties

        /// <summary>
        /// Zwraca imię i nazwisko aktualnie zalogowanej osoby, oddzielone spacją.
        /// </summary>
        public string Title
        {
            get
            {
                return jobTitle + " - " + firstName + " " + lastName;
            }

            private set
            {
                jobTitle = firstName = lastName = "";
            }
        }

        #endregion // Public properties

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
        /// Jeśli znaleziono w bazie odpowiadającego użytkownika, pobierane są również jego imię, nazwisko i nazwa stanowiska.
        /// </summary>
        /// <param name="user">Obiekt zawierający podane login i hasło.</param>
        /// <returns>ID osoby, do której przypisany jest wskazany użytkownik systemu lub 0, jeśli podane poświadczenia nie figurują w bazie danych.</returns>
        public int LogIn(User user)
        {
            System.Security.Cryptography.HashAlgorithm sha = System.Security.Cryptography.HashAlgorithm.Create("SHA512");

            // sprawdzenie czy w systemie istnieje użytkownik o podanym loginie i hashu hasła
            A_User usr = userService.SelectUser(x => x.Login == user.Login &&
                                              x.Password == System.Text.Encoding.ASCII.GetString(sha.ComputeHash(System.Text.Encoding.ASCII.GetBytes(user.Password))));
            
            // jeśli podane poświadczenia są prawidłowe
            if (usr.WorkerId > 0)
            {
                // pobranie informacji o pracowniku, do którego przypisany jest sprawdzony użytkownik systemu
                A_Worker wrk = userService.SelectWorker(x => x.Id == usr.WorkerId);

                // jeśli rekord użytkownika w bazie zawiera prawidłowe ID pracownika
                if (wrk.Id > 0)
                {
                    // zapisanie imienia i nazwiska pracownika
                    lastName = wrk.LastName;
                    firstName = wrk.FirstName;

                    // pobranie informacji o stanowisku służbowym pracownika
                    A_DictionaryJobTitle djt = userService.SelectJobTitle(x => x.Id == wrk.JobTitle);

                    // jeśli rekord pracownika w bazie zawiera prawidłowe ID stanowiska
                    if (djt.Id > 0)
                    {
                        // zapisanie nazwy stanowiska
                        jobTitle = djt.JobTitle;
                    }
                }
            }

            return usr.WorkerId;
        }

        #endregion // Public methods
    }
}
