using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje jedną pozycję z listy lekarzy w widoku głównym rejestrowania wizyty przez rejestratorkę.
    /// </summary>
    class DoctorsListItem
    {
        // Public getters, private setters
        #region Public properties

        /// <summary>
        /// Przechowuje nazwę poradni medycznej, w której przyjmuje wskazany lekarz.
        /// Jest to wartość z kolumny Name z tabeli M_DictionaryClinics.
        /// </summary>
        public string ClinicName { get; private set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli A_Workers, odpowiadającemu danemu lekarzowi.
        /// </summary>
        public int DoctorId { get; private set; }
        
        /// <summary>
        /// Przechowuje nazwisko wskazanego lekarza.
        /// Jest to wartość z kolumny LastName z tabeli A_Workers.
        /// </summary>
        public string DoctorLastName { get; private set; }

        /// <summary>
        /// Przechowuje imię wskazanego lekarza.
        /// Jest to wartość z kolumny FirstName z tabeli A_Workers.
        /// </summary>
        public string DoctorFirstName { get; private set; }

        /// <summary>
        /// Przechowuje liczbę pacjentów, jaka jest zarejestrowana do wskazanego lekarza na podany dzień, w ramach danej poradni medycznej.
        /// </summary>
        public int PatientsNumber { get; private set; }

        /// <summary>
        /// Przechowuje numer gabinetu, w którym przyjmuje wskazany lekarz w ramach danej poradni medycznej.
        /// </summary>
        public string RoomNumber { get; private set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Ukrycie domyślnego konstruktora.
        /// </summary>
        private DoctorsListItem() { }

        /// <summary>
        /// Konstruktor zapisujący w tworzonym obiekcie wartości podane w argumentach.
        /// </summary>
        /// <param name="ClinicName">Wartość z kolumny Name z tabeli M_DictionaryClinics.</param>
        /// <param name="DoctorLastName">Wartość z kolumny LastName z tabeli A_Workers.</param>
        /// <param name="DoctorFirstName">Wartość z kolumny FirstName z tabeli A_Workers.</param>
        /// <param name="PatientsNumber">Liczba pacjentów zarejestrowana do wskazanego lekarza na podany dzień, w ramach danej poradni medycznej.</param>
        /// <param name="RoomNumber">Numer gabinetu, w którym przyjmuje wskazany lekarz w ramach danej poradni medycznej.</param>
        public DoctorsListItem(string ClinicName, int DoctorId, string DoctorLastName, string DoctorFirstName, int PatientsNumber, string RoomNumber)
        {
            this.ClinicName = ClinicName;
            this.DoctorId = DoctorId;
            this.DoctorLastName = DoctorLastName;
            this.DoctorFirstName = DoctorFirstName;
            this.PatientsNumber = PatientsNumber;
            this.RoomNumber = RoomNumber;
        }

        #endregion // Ctors
    }
}
