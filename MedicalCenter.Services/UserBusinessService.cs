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
            A_User usr = userService.SelectUser(x => x.Login == user.Login &&
                                              x.Password == user.Password);
            
            // jeśli podane poświadczenia są prawidłowe
            if (usr.WorkerId > 0)
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
                        user.Title = job.JobTitle + " - " + wrk.LastName + "  " + wrk.FirstName;

                        // zapisanie kodu stanowiska
                        user.JobTitleCode = job.Code;
                    }
                }
            }
        }

        #endregion // Public methods
    }
}
