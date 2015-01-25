using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje obiekt klasy M_Patient w widoku dla rejestratorki.
    /// </summary>
    public class Patient
    {
        #region Public properties

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Patients, reprezentującego danego pacjenta.
        /// Wartość 0 oznacza użycie domyślnego konstruktora, co jest rozumiane jako utworzenie nowego obiektu (rekordu).
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Przechowuje nazwisko pacjenta.
        /// Jest to wartość z kolumny LastName z tabeli z tabeli M_Patients.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Przechowuje pierwsze imię pacjenta.
        /// Jest to wartość z kolumny FirstName z tabeli M_Patients.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Przechowuje drugie imię pacjenta.
        /// Jest to wartość z kolumny SecondName z tabeli M_Patients.
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Przechowuje datę urodzenia pacjenta.
        /// Jest to wartość z kolumny BirthDate z tabeli M_Patients.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Przechowuje informację o płci pacjenta.
        /// Jest to wartość z kolumny Gender z tabeli M_Patients.
        /// </summary>
        public bool Gender { get; set; }

        /// <summary>
        /// Przechowuje numer PESEL pacjenta.
        /// Jest to wartość z kolumny Pesel z tabeli M_Patients.
        /// Ta wartość musi być unikatowa w tabeli.
        /// </summary>
        public long Pesel { get; set; }
        
        /// <summary>
        /// Przechowuje nazwę ulicy z adresu podanego przez pacjenta.
        /// Jest to wartość z kolumny Street z tabeli M_Patients.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Przechowuje numer budynku z adresu podanego przez pacjenta.
        /// Jest to wartość z kolumny BuildingNumber z tabeli M_Patients.
        /// </summary>
        public string BuildingNumber { get; set; }

        /// <summary>
        /// Przechowuje numer lokalu z adresu podanego przez pacjenta.
        /// Jest to wartość z kolumny Apartment z tabeli M_Patients.
        /// </summary>
        public string Apartment { get; set; }

        /// <summary>
        /// Przechowuje kod pocztowy z adresu podanego przez pacjenta.
        /// Jest to wartość z kolumny PostalCode z tabeli M_Patients.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Przechowuje nazwę miejscowości z adresu podanego przez pacjenta.
        /// Jest to wartość z kolumny City z tabeli M_Patients.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Przechowuje nazwę urzędu pocztowego z adresu podanego przez pacjenta.
        /// Jest to wartość z kolumny Post z tabeli M_Patients.
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// Przechowuje informację o ubezpieczeniu pacjenta.
        /// Jest to wartość z kolumny IsInsured z tabeli M_Patients.
        /// </summary>
        public bool IsInsured { get; set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Domyślny konstruktor. Tworzy obiekt reprezentujący nowy rekord wstawiany do tabeli M_Patients.
        /// </summary>
        public Patient()
        {
            this.Id = 0;
            this.LastName = string.Empty;
            this.FirstName = string.Empty;
            this.SecondName = string.Empty;
            this.BirthDate = DateTime.Today;
            this.Gender = false;
            this.Pesel = 0;
            this.Street = string.Empty;
            this.BuildingNumber = string.Empty;
            this.Apartment = string.Empty;
            this.PostalCode = string.Empty;
            this.City = string.Empty;
            this.Post = string.Empty;
            this.IsInsured = true;
        }

        /// <summary>
        /// Konstruktor ustawiający podane w argumentach wartości polom obiektu. Używany do przechowywania danych z istniejących w tabeli M_Patients rekordów.
        /// </summary>
        /// <param name="Id">Wartość z kolumny Id pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="LastName">Wartość z kolumny LastName pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="FirstName">Wartość z kolumny FirstName pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="SecondName">Wartość z kolumny SecondName pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="BirthDate">Wartość z kolumny BirthDate pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="Gender">Wartość z kolumny Gender pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="Pesel">Wartość z kolumny Pesel pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="Street">Wartość z kolumny Street pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="BuildingNumber">Wartość z kolumny BuildingNumber pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="Apartment">Wartość z kolumny Apartment pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="PostalCode">Wartość z kolumny PostalCode pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="City">Wartość z kolumny City pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="Post">Wartość z kolumny Post pobranego z tabeli M_Patients rekordu.</param>
        /// <param name="IsInsured">Wartość z kolumny IsInsured pobranego z tabeli M_Patients rekordu.</param>
        public Patient(int Id,
                       string LastName,
                       string FirstName,
                       string SecondName,
                       DateTime BirthDate,
                       bool Gender,
                       long Pesel,
                       string Street,
                       string BuildingNumber,
                       string Apartment,
                       string PostalCode,
                       string City,
                       string Post,
                       bool IsInsured)
        {
            this.Id = Id;
            this.LastName = LastName;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.BirthDate = BirthDate;
            this.Gender = Gender;
            this.Pesel = Pesel;
            this.Street = Street;
            this.BuildingNumber = BuildingNumber;
            this.Apartment = Apartment;
            this.PostalCode = PostalCode;
            this.City = City;
            this.Post = Post;
            this.IsInsured = IsInsured;
        }

        #endregion // Ctors
    }
}
