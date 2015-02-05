﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalCenter.Data;

namespace MedicalCenter.DBServices
{
    /// <summary>
    /// Zestaw podstawowych operacji na tabelach związanych z medyczną działalnością placówki.
    /// </summary>
    public class MedicalService
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
        public MedicalService()
        {
            db = new MedicalCenterDBContainer();
        }

        #endregion // Ctors

        #region Public methods

        #region Select

        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanej poradni medycznej. Funkcja ta uwzględnia modyfikacje słownika.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Obiekt reprezentujący rekord z tabeli M_DictionaryClinic, odpowiadający szukanej poradni medycznej,
        /// lub obiekt z wartościami domyślnymi, jeżeli nie znaleziono poradni medycznej odpowiadającej podanym warunkom.
        /// </returns>
        public M_DictionaryClinic SelectClinic(System.Linq.Expressions.Expression<Func<M_DictionaryClinic, bool>> predicate)
        {
            // pobranie encji spełniającej podane kryteria
            M_DictionaryClinic entity = db.M_DictionaryClinics.FirstOrDefault(predicate);

            // jeśli znaleziono encję pasującą do opisu
            if (entity != null)
            {
                // jeśli wybrana pozycja była aktualizowana, należy znaleźć najnowszą wersję
                while (entity.New > 0)
                    entity = db.M_DictionaryClinics.FirstOrDefault(x => x.Id == entity.New);

                // jeśli wybrana pozycja została oznaczona jako usunięta, to nie można jej zwrócić
                if (entity.IsDeleted)
                    entity = null;
            }

            return entity;
        }

        /// <summary>
        /// Pobiera z bazy danych informacje o wszystkich poradniach medycznych. Funkcja ta uwzględnia modyfikacje słownika.
        /// </summary>
        /// <returns>Zbiór rekordów z tabeli M_DictionaryClinics.</returns>
        public List<M_DictionaryClinic> SelectClinics()
        {
            // wrzucenie całej zawartości tabeli do listy
            List<M_DictionaryClinic> list = new List<M_DictionaryClinic>(db.M_DictionaryClinics.AsEnumerable().OrderBy(x => x.Name));

            int i = 0;
            int temp = 0;
            M_DictionaryClinic entity = null;

            // usuwanie z listy referencji do starych i oznaczonych jako usunięte wpisów
            while (i < list.Count)
            {
                entity = list[i];

                // jeśli rekord był aktualizowany
                if (entity.New > 0)
                {
                    temp = i;
                    
                    // szukanie najnowszej wersji
                    while (entity.New > 0)
                    {
                        // przejście do zaktualizowanej wersji rekordu
                        entity = list.FirstOrDefault(x => x.Id == entity.New);
                        // usunięcie starej wersji z listy
                        list.RemoveAt(temp);
                        // zapisanie pozycji nowej wersji na potrzeby następnej iteracji
                        temp = list.IndexOf(entity);
                    }
                }
                // jeśli rekord nie był aktualizowany, następuje przejście do następnego
                else
                    ++i;
            }

            return list;
        }

        /// <summary>
        /// Pobiera z bazy danych informacje o wskazanych wizytach.
        /// </summary>
        /// <param name="predicate">Funkcja (predykat) sprawdzająca warunek dla każdego elementu.</param>
        /// <returns>
        /// Lista obiektów reprezentujących rekordy z tabeli M_Visits,
        /// lub null, jeżeli nie znaleziono wizyt odpowiadających podanym warunkom.
        /// </returns>
        public IEnumerable<M_Visit> SelectVisits(Func<M_Visit, bool> predicate)
        {
            return db.M_Visits.Where(predicate).OrderBy(x => x.DateOfVisit);
        }

        #endregion // Select

        #region Insert

        /// <summary>
        /// Zapisuje w bazie danych nowy rekord dla wizyty, wpisując do niego podane informacje.
        /// </summary>
        /// <param name="newVisit">Informacje o nowej wizycie.</param>
        /// <returns>true jeśli wstawiono pomyślnie, null jeśli podana encja nie przeszła walidacji po stronie bazy, false jeśli wystąpił inny błąd</returns>
        public bool? InsertVisit(M_Visit newVisit)
        {
            bool? retval = true;

            // dodanie nowej encji do lokalnego zbioru
            db.M_Visits.Add(newVisit);

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
        /// Aktualizuje w bazie danych rekord o wskazanej wizycie.
        /// </summary>
        /// <param name="visit">Zaktualizowane informacje o wizycie.</param>
        /// <returns>true jeśli zaktualizowano pomyślnie, null jeśli podana encja nie przeszła walidacji po stronie bazy, false jeśli wystąpił inny błąd</returns>
        public bool? UpdateVisit(M_Visit visit)
        {
            bool? retval = true;

            // przechowuje liczbę wierszy, jaka została dodana/zmieiona podczas wskazanej operacji
            int rowsAffected = 0;

            // referencja do encji, która ma zostać zmieniona
            M_Visit record = null;

            try
            {
                // szukanie encji o podanym ID
                record = db.M_Visits.Find(new int[] { visit.Id });
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
                    record.DateOfVisit = visit.DateOfVisit;
                    record.Description = visit.Description;
                    record.Diagnosis   = visit.Diagnosis;
                    record.DoctorId    = visit.DoctorId;
                    record.Ended       = visit.Ended;
                    record.IsEmergency = visit.IsEmergency;
                    record.LastEdit    = visit.LastEdit;
                    record.Started     = visit.Started;
                    record.State       = visit.State;

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
