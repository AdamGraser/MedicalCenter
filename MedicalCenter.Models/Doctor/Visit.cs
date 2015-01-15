﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Doctor
{
    /// <summary>
    /// Reprezentuje zbiór informacji o pojedynczej wizycie.
    /// </summary>
    class Visit
    {
        #region Public properties

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Visits, odpowiadającemu tej wizycie.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Przechowuje ID rekordu z tabeli M_Patients, reprezentującego danego pacjenta.
        /// </summary>
        public int PatientId { get; private set; }

        /// <summary>
        /// Przechowuje nazwisko pacjenta.
        /// Jest to wartość z kolumny LastName z tabeli z tabeli M_Patients.
        /// </summary>
        public string PatientLastName { get; private set; }

        /// <summary>
        /// Przechowuje pierwsze imię pacjenta.
        /// Jest to wartość z kolumny FirstName z tabeli M_Patients.
        /// </summary>
        public string PatientFirstName { get; private set; }

        /// <summary>
        /// Przechowuje datę zakończenia tej wizyty.
        /// Jest to wartość z kolumny Ended z tabeli M_Visits.
        /// </summary>
        public Nullable<System.DateTime> Ended { get; set; }

        /// <summary>
        /// Przechowuje stan wizyty.
        /// Jest to wartość z kolumny State z tabeli M_Visits.
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// Przechowuje opis wizyty.
        /// Jest to wartość z kolumny Description z tabeli M_Visits.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Przechowuje diagnozę wystawioną w trakcie trwania tej wizyty.
        /// Jest to wartość z kolumny Diagnosis z tabeli M_Visits.
        /// </summary>
        public string Diagnosis { get; set; }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Ukrycie domyślnego konstruktora.
        /// </summary>
        private Visit() { }

        /// <summary>
        /// Konstruktor zapisujący w tworzonym obiekcie wartości podane w argumentach.
        /// </summary>
        /// <param name="Id">ID rekordu z tabeli M_Visits, odpowiadającemu tej wizycie.</param>
        /// <param name="PatientId">Wartość z kolumny LastName z tabeli z tabeli M_Patients.</param>
        /// <param name="PatientLastName">Wartość z kolumny LastName z tabeli z tabeli M_Patients.</param>
        /// <param name="PatientFirstName">Wartość z kolumny FirstName z tabeli M_Patients.</param>
        /// <param name="Ended">Wartość z kolumny Ended z tabeli M_Visits.</param>
        /// <param name="State">Wartość z kolumny State z tabeli M_Visits.</param>
        /// <param name="Description">Wartość z kolumny Description z tabeli M_Visits.</param>
        /// <param name="Diagnosis">Wartość z kolumny Diagnosis z tabeli M_Visits.</param>
        public Visit(int Id, int PatientId, string PatientLastName, string PatientFirstName, Nullable<System.DateTime> Ended, byte State, string Description, string Diagnosis)
        {
            this.Id = Id;
            this.PatientId = PatientId;
            this.PatientLastName = PatientLastName;
            this.PatientFirstName = PatientFirstName;
            this.Ended = Ended;
            this.State = State;
            this.Description = Description;
            this.Diagnosis = Diagnosis;
        }

        #endregion // Ctors
    }
}
