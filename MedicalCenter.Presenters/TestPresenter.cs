using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Presenters
{
    public class TestPresenter
    {
        #region Public properties

        /// <summary>
        /// Przechowuje treść komunikatu o nieprawidłowych danych logowania.
        /// </summary>
        public string InvalidLogon { get; private set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Domyślny konstruktor, zapisujący domyślną treść komunikatu o nieprawidłowych danych logowania.
        /// </summary>
        public TestPresenter()
        {
            InvalidLogon = "Nieprawidłowa nazwa użytkownika i/lub hasło";
        }

        #endregion // Ctors
    }
}
