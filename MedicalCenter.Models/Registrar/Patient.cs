using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models
{
    /// <summary>
    /// Reprezentuje obiekt klasy <see cref="M_Patient"/> w widoku dla rejestratorki.
    /// </summary>
    public class Patient
    {
        #region Public properties

        /// <summary>
        /// Przechowuje wartość z kolumny Id rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// Wartość 0 oznacza użycie domyślnego konstruktora, co jest rozumiane jako utworzenie nowego obiektu (rekordu).
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Przechowuje wartość z kolumny LastName rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny FirstName rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny SecondName rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny BirthDate rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny Gender rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public bool Gender { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny Pesel rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// Ta wartość musi być unikatowa w tabeli.
        /// </summary>
        public long Pesel { get; set; }
        
        /// <summary>
        /// Przechowuje wartość z kolumny Street rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny BuildingNumber rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public string BuildingNumber { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny Apartment rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public string Apartment { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny PostalCode rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny City rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny Post rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// Przechowuje wartość z kolumny IsInsured rekordu reprezentowanego przez obiekt klasy <see cref="M_Patient"/>.
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
            this.Gender = false;
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
