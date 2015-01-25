using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Data;
using MedicalCenter.Models.Registrar;

namespace MedicalCenter.DBServices
{
    /// <summary>
    /// Zestaw podstawowych operacji na tabelach związanych z pacjentami.
    /// </summary>
    public class PatientService
    {
        #region Private fields

        /// <summary>
        /// Obiekt kontekstu bazodanowego, używany do wykonywania operacji na bazie danych.
        /// </summary>
        MedicalCenterDBContainer db;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Bezargumentowy konstruktor inicjalizujący obiekt kontekstu bazodanowego.
        /// </summary>
        public PatientService()
        {
            db = new MedicalCenterDBContainer();
        }

        #endregion // Ctors

        #region Public methods

        #region Select

        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanym pacjencie.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli M_Patients,
        /// lub obiekt z wartościami domyślnymi, jeżeli nie znaleziono pacjenta odpowiadającego podanym warunkom.
        /// </returns>
        public M_Patient SelectPatient(System.Linq.Expressions.Expression<Func<M_Patient, bool>> predicate)
        {
            return db.M_Patients.FirstOrDefault(predicate);
        }

        #endregion // Select

        #region Insert

        /// <summary>
        /// Zapisuje w bazie danych nowy rekord dla pacjenta, wpisując do niego podane informacje.
        /// </summary>
        /// <param name="newPatient">Informacje o nowym pacjencie.</param>
        /// <returns>true jeśli wstawiono pomyślnie, null jeśli podana encja nie przeszła walidacji po stronie bazy, false jeśli wystąpił inny błąd</returns>
        public bool? InsertPatient(M_Patient newPatient)
        {
            bool? retval = true;

            // dodanie nowej encji do lokalnego zbioru
            db.M_Patients.Add(newPatient);

            // przechowuje liczbę wierszy, jaka została dodana/zmieiona podczas wskazanej operacji
            int rowsAffected = 0;

            try
            {
                // synchronizacja zbioru danych z bazą danych
                rowsAffected = db.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException concurrencyException)
            {
                System.Console.WriteLine(concurrencyException.Message);

                // jeśli wystąpił ten błąd i nowy rekord nie został zapisany w tabeli, podejmowana jest jeszcze jedna próba zapisu
                if (rowsAffected == 0)
                {
                    try
                    {
                        rowsAffected = db.SaveChanges();
                    }
                    catch (Exception exception)
                    {
                        System.Console.WriteLine(exception.Message);

                        // jeśli druga próba również zakończyła się niepowodzeniem, zwracana jest informacja o błędzie
                        retval = false;
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException validationException)
            {
                System.Console.WriteLine(validationException.Message);

                // podana encja nie przeszła walidacji
                retval = null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);

                // inny błąd
                retval = false;
            }

            return retval;
        }

        #endregion // Insert

        #region Update

        /// <summary>
        /// Aktualizuje w bazie danych rekord o wskazanym pacjencie.
        /// </summary>
        /// <param name="patient">Zaktualizowane informacje o pacjencie.</param>
        /// <returns>true jeśli zaktualizowano pomyślnie, null jeśli podana encja nie przeszła walidacji po stronie bazy, false jeśli wystąpił inny błąd</returns>
        public bool? UpdatePatient(M_Patient patient)
        {
            bool? retval = true;

            // przechowuje liczbę wierszy, jaka została dodana/zmieiona podczas wskazanej operacji
            int rowsAffected = 0;

            // referencja do encji, która ma zostać zmieniona
            M_Patient record = null;

            try
            {
                // szukanie encji o podanym ID
                record = db.M_Patients.Find(new int[] { patient.Id });
            }
            catch (InvalidOperationException ioe)
            {
                System.Console.WriteLine(ioe.Message);

                retval = false;
            }

            // jeśli znaleziono encję o podanym ID, następuje aktualizacja jej właściwości
            if (record != null)
            {
                if (retval == true)
                {
                    record.Apartment      = patient.Apartment;
                    record.BirthDate      = patient.BirthDate;
                    record.BuildingNumber = patient.BuildingNumber;
                    record.City           = patient.City;
                    record.FirstName      = patient.FirstName;
                    record.Gender         = patient.Gender;
                    record.IsInsured      = patient.IsInsured;
                    record.LastName       = patient.LastName;
                    record.Pesel          = patient.Pesel;
                    record.Post           = patient.Post;
                    record.PostalCode     = patient.PostalCode;
                    record.SecondName     = patient.SecondName;
                    record.Street         = patient.Street;

                    try
                    {
                        // synchronizacja zbioru danych z bazą danych
                        rowsAffected = db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException concurrencyException)
                    {
                        System.Console.WriteLine(concurrencyException.Message);

                        // jeśli wystąpił ten błąd i nowy rekord nie został zapisany w tabeli, podejmowana jest jeszcze jedna próba zapisu
                        if (rowsAffected == 0)
                        {
                            try
                            {
                                rowsAffected = db.SaveChanges();
                            }
                            catch (Exception exception)
                            {
                                System.Console.WriteLine(exception.Message);

                                // jeśli druga próba również zakończyła się niepowodzeniem, zwracana jest informacja o błędzie
                                retval = false;
                            }
                        }
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException validationException)
                    {
                        System.Console.WriteLine(validationException.Message);

                        // podana encja nie przeszła walidacji
                        retval = null;
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);

                        // inny błąd
                        retval = false;
                    }
                }
            }
            // jeśli nie znaleziono encji o podanym ID, zgłaszane jest to jako błąd
            else
                retval = false;

            return retval;
        }

        #endregion // Update

        #endregion // Public methods
    }
}
