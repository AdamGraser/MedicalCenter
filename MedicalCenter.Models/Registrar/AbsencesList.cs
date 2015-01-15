using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje listę lekarzy nieobecnych we wskazanym dniu.
    /// </summary>
    class AbsencesList
    {
        #region Public properties

        /// <summary>
        /// Data wskazująca dzień, w którym nieobecni są lekarze znajdujący się na tej liście.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Lista lekarzy nieobecnych w danym dniu.
        /// </summary>
        public List<string> DoctorsList { get; private set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący pustą listę lekarzy.
        /// </summary>
        public AbsencesList()
        {
            DoctorsList = new List<string>();
        }

        #endregion // Ctors
    }
}
