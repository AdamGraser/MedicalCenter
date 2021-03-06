﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.LoggingIn
{
    /// <summary>
    /// Reprezentuje użytkownika systemu.
    /// </summary>
    public class User
    {
        #region Private fields

        /// <summary>
        /// Przechowuje hash wprowadzonego w formularzu logowania hasła dostępu do konta użytkownika.
        /// Jest to wartość z kolumny Password z tabeli A_Users.
        /// </summary>
        string password;

        #endregion // Private fields

        #region Public properties

        /// <summary>
        /// Przechowuje wprowadzoną w formularzu logowania nazwę użytkownika systemu.
        /// Jest to wartość z kolumny Login z tabeli A_Users.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Akcesor dla prywatnego pola "password", przechowującego hash hasła dostępu do konta użytkownika.
        /// Setter zapisuje w owym polu odpowiednio obliczony hash hasła.
        /// </summary>
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                System.Security.Cryptography.HashAlgorithm sha = System.Security.Cryptography.HashAlgorithm.Create("SHA512");

                password = System.Text.Encoding.ASCII.GetString(sha.ComputeHash(System.Text.Encoding.ASCII.GetBytes(value)));
            }
        }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli A_Workers, reprezentującego osobę aktualnie zalogowaną.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Przechowuje nazwę stanowiska, imię i nazwisko aktualnie zalogowanej osoby.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Przechowuje kod stanowiska aktualnie zalogowanej osoby.
        /// Jest to wartość z kolumny Code z tabeli A_DictionaryJobTitles.
        /// </summary>
        public string JobTitleCode { get; set; }
        
        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący obiekt z wartościami domyślnymi.
        /// </summary>
        public User()
        {
            Id = 0;
            Login = string.Empty;
            Password = string.Empty;
            Title = string.Empty;
            JobTitleCode = string.Empty;
        }

        #endregion // Ctors
    }
}
