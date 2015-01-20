using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.LoggingIn
{
    /// <summary>
    /// Reprezentuje użytkownika systemu.
    /// </summary>
    class User
    {
        #region Public properties

        /// <summary>
        /// Przechowuje wprowadzoną w formularzu logowania nazwę użytkownika systemu.
        /// Jest to wartość z kolumny Login z tabeli A_Users.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Przechowuje hash wprowadzonego w formularzu logowania hasła dostępu do konta użytkownika.
        /// Jest to wartość z kolumny Password z tabeli A_Users.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli A_Workers, reprezentującego osobę aktualnie zalogowaną.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Przechowuje imię i nazwisko aktualnie zalogowanej osoby.
        /// </summary>
        public string Name { get; set; }
        
        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący obiekt z domyślną wartością Id.
        /// </summary>
        public User()
        {
            Id = 0;
        }

        #endregion // Ctors
    }
}
