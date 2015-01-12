using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje rejestrowaną wizytę.
    /// </summary>
    class Visit
    {
        #region Public properties

        /// <summary>
        /// Przechowuje ID rekordu z tabeli A_Workers, odpowiadającemu lekarzowi, do którego ta wizyta ma zostać zarejestrowana.
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Patients, odpowiadającemu pacjentowi, dla którego ta wizyta ma zostać zarejestrowana.
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Określa czy wizyta ta ma zostać zarejestrowana jako nagły przypadek.
        /// </summary>
        public bool IsEmergency { get; set; }

        /// <summary>
        /// Przechowuje planową datę i godzinę odbycia się rejestrowanej wizyty.
        /// </summary>
        public DateTime DateOfVisit { get; set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Tworzy obiekt reprezentujący rejestrowaną wizytę z wartościami domyślnymi.
        /// </summary>
        public Visit()
        {
            DoctorId = 0;
            PatientId = 0;
            IsEmergency = false;
            DateOfVisit = DateTime.Today;
        }

        #endregion // Ctors
    }
}
