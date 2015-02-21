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
        /// Bezargumentowy, pusty konstruktor.
        /// </summary>
        public PatientService()
        {
            
        }

        #endregion // Ctors

        #region Public methods

        #region Select

        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanym pacjencie.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu. Wartość null powoduje, że ta metoda również zwraca null.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli M_Patients,
        /// null jeżeli nie znaleziono pacjenta odpowiadającego podanym warunkom lub podany argument to null.
        /// </returns>
        public M_Patient SelectPatient(System.Linq.Expressions.Expression<Func<M_Patient, bool>> predicate)
        {
            M_Patient entity = null;

            if (predicate != null)
            {
                // utworzenie obiektu kontekstu bazodanowego
                db = new MedicalCenterDBContainer();

                // pobranie encji
                entity =  db.M_Patients.FirstOrDefault(predicate);

                // usunięcie obiektu kontekstu bazodanowego
                db.Dispose();
            }

            return entity;
        }

        /// <summary>
        /// Pobiera z bazy zbiór wszystkich pacjentów.
        /// </summary>
        /// <returns>Wszystkie rekordy z tabeli M_Patients, przedstawione jako kolekcja obiektów M_Patient.</returns>
        public IEnumerable<M_Patient> SelectPatients()
        {
            db = new MedicalCenterDBContainer();

            IEnumerable<M_Patient> entities = db.M_Patients.AsEnumerable().OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.SecondName);

            return entities;
        }

        #endregion // Select

        #region Insert

        /// <summary>
        /// Zapisuje w bazie danych nowy rekord dla pacjenta, wpisując do niego podane informacje.
        /// </summary>
        /// <param name="newPatient">Informacje o nowym pacjencie. Wartość null powoduje, że ta metoda zwraca false.</param>
        /// <returns>
        /// true jeśli wstawiono pomyślnie,
        /// null jeśli wystąpił błąd aktualizacji,
        /// false jeśli wystąpił inny błąd lub podany argument to null.
        /// </returns>
        public bool? InsertPatient(M_Patient newPatient)
        {
            bool? retval = true;

            if (newPatient != null)
            {
                db = new MedicalCenterDBContainer();

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
                catch (System.Data.Entity.Infrastructure.DbUpdateException updateException)
                {
                    System.Console.WriteLine(updateException.Message);

                    Exception ex = updateException;

                    // szukanie właściwego wyjątku
                    if (ex.InnerException != null)
                    {
                        while (ex.InnerException != null)
                            ex = ex.InnerException;

                        // jeśli istniał jakiś wewnętrzy wyjątek, wyświetlona zostanie jego wiadomość
                        System.Console.WriteLine(ex.Message);
                    }

                    // podana encja nie może zostać dodana (np. z powodu złamania ograniczenia unikatowości)
                    retval = null;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);

                    // inny błąd
                    retval = false;
                }

                db.Dispose();
            }
            else
                retval = false;

            return retval;
        }

        #endregion // Insert

        #region Update

        /// <summary>
        /// Aktualizuje w bazie danych rekord o wskazanym pacjencie.
        /// </summary>
        /// <param name="patient">Zaktualizowane informacje o pacjencie. Wartość null powoduje, że ta metoda zwraca false.</param>
        /// <returns>
        /// true jeśli zaktualizowano pomyślnie,
        /// null jeśli wystąpił błąd aktualizacji,
        /// false jeśli wystąpił inny błąd lub podany argument to null.
        /// </returns>
        public bool? UpdatePatient(M_Patient patient)
        {
            bool? retval = true;

            if (patient != null)
            {
                db = new MedicalCenterDBContainer();

                // przechowuje liczbę wierszy, jaka została dodana/zmieiona podczas wskazanej operacji
                int rowsAffected = 0;

                // referencja do encji, która ma zostać zmieniona
                M_Patient record = null;

                try
                {
                    // szukanie encji o podanym ID
                    record = db.M_Patients.FirstOrDefault(x => x.Pesel == patient.Pesel);
                }
                catch (InvalidOperationException ioe)
                {
                    System.Console.WriteLine(ioe.Message);

                    retval = false;

                    // utworzenie nowego obiektu kontekstu bazodanowego
                    db.Dispose();
                    db = new MedicalCenterDBContainer();
                }

                // jeśli znaleziono encję o podanym ID, następuje aktualizacja jej właściwości
                if (record != null)
                {
                    if (retval == true)
                    {
                        record.Apartment = patient.Apartment;
                        record.BirthDate = patient.BirthDate;
                        record.BuildingNumber = patient.BuildingNumber;
                        record.City = patient.City;
                        record.FirstName = patient.FirstName;
                        record.Gender = patient.Gender;
                        record.IsInsured = patient.IsInsured;
                        record.LastName = patient.LastName;
                        record.Pesel = patient.Pesel;
                        record.Post = patient.Post;
                        record.PostalCode = patient.PostalCode;
                        record.SecondName = patient.SecondName;
                        record.Street = patient.Street;

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
                                db.Dispose();
                                db = new MedicalCenterDBContainer();

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
                        catch (System.Data.Entity.Infrastructure.DbUpdateException updateException)
                        {
                            System.Console.WriteLine(updateException.Message);

                            Exception ex = updateException;

                            // szukanie właściwego wyjątku
                            if (ex.InnerException != null)
                            {
                                while (ex.InnerException != null)
                                    ex = ex.InnerException;

                                // jeśli istniał jakiś wewnętrzy wyjątek, wyświetlona zostanie jego wiadomość
                                System.Console.WriteLine(ex.Message);
                            }

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

                db.Dispose();
            }
            else
                retval = false;

            return retval;
        }

        #endregion // Update

        /// <summary>
        /// Usuwa obiekt kontekstu bazodanowego.
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }

        #endregion // Public methods
    }
}
