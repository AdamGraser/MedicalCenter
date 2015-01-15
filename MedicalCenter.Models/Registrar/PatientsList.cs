using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje listę pacjentów widoczną podczas rejestrowania wizyty przez rejestratorkę.
    /// </summary>
    class PatientsList : IList
    {
        #region Private fields

        /// <summary>
        /// Lista pacjentów.
        /// </summary>
        private List<Patient> patientsList;

        #endregion // Private fields

        #region Private properties

        /// <summary>
        /// Zwraca wartość określającą czy dostęp do kolekcji ICollection jest synchronizowany pomiędzy wątkami. (Odziedziczone z ICollection.)
        /// Zwraca wartość false.
        /// </summary>
        private bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Zwraca obiekt, który może zostać użyty do synchronizacji dostępu do kolekcji ICollection. (Odziedziczone z ICollection.)
        /// Zwraca null.
        /// </summary>
        private object SyncRoot
        {
            get { return null; }
        }

        #endregion // Private properties

        #region Public properties

        /// <summary>
        /// Umożliwia pobranie nazwiska i imienia pacjenta o wskazanym indeksie w liście.
        /// Zmiana elementu o wskazanym indeksie jest dostępna tylko wewnątrz klasy.
        /// </summary>
        /// <param name="index">Indeks pacjenta, którego nazwisko i imię mają zostać zwrócone.</param>
        /// <returns>Nazwisko i imię pacjenta o wskazanym indeksie lub null, jeśli podany indeks jest spoza zakresu.</returns>
        public string this[int index]
        {
            get
            {
                try
                {
                    return patientsList[index].LastName + " " + patientsList[index].FirstName;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

            private set;
        }

        /// <summary>
        /// Określa czy rozmiar listy jest stały.
        /// </summary>
        public bool IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Określa czy lista jest tylko do odczytu.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Zwraca liczbę elementów w liście.
        /// </summary>
        public int Count
        {
            get { return patientsList.Count; }
        }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący pustą listę pacjentów.
        /// </summary>
        public PatientsList()
        {
            patientsList = new List<Patient>();
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Dodaje nowy element na koniec listy.
        /// </summary>
        /// <param name="newItem">Element, który ma zostać dodany do listy.</param>
        public void Add(Patient newItem)
        {
            patientsList.Add(newItem);
        }

        /// <summary>
        /// Usuwa wszystkie elementy z listy.
        /// </summary>
        public void Clear()
        {
            patientsList.Clear();
        }

        /// <summary>
        /// Determinuje czy lista zawiera podany element.
        /// </summary>
        /// <param name="item">Element, którego obecność w liście ma zostać sprawdzona.</param>
        /// <returns>Zwraca true jeśli lista zawiera podany element, false w przeciwnym razie.</returns>
        public bool Contains(Patient item)
        {
            return patientsList.Contains(item);
        }

        /// <summary>
        /// Podaje indeks w liście wskazanego elementu.
        /// </summary>
        /// <param name="item">Element, którego indeks ma zostać podany.</param>
        /// <returns>Zwraca indeks podanego elementu lub -1, jeśli nie znaleziono.</returns>
        public int IndexOf(Patient item)
        {
            return patientsList.IndexOf(item);
        }

        /// <summary>
        /// Wstawia element do listy na pozycję wskazywaną przez podany indeks.
        /// </summary>
        /// <param name="index">Indeks, jaki ma mieć element po wstawieniu do listy.</param>
        /// <param name="newItem">Element, który ma zostać wstawiony do listy.</param>
        public void Insert(int index, Patient newItem)
        {
            try
            {
                patientsList.Insert(index, newItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Usuwa z listy pierwsze znalezione wystąpienie podanego elementu.
        /// </summary>
        /// <param name="item">Element, którego pierwsze wystąpienie ma zostać usunięte z listy.</param>
        public void Remove(Patient item)
        {
            patientsList.Remove(item);
        }

        /// <summary>
        /// Usuwa z listy element o podanym indeksie.
        /// </summary>
        /// <param name="index">Indeks elementu, który ma zostać usunięty.</param>
        public void RemoveAt(int index)
        {
            try
            {
                patientsList.RemoveAt(index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Kopiuje wszystkie elementy listy do podanej tablicy, wstawiając pierwszy element na pozycję o wskazanym indeksie.
        /// </summary>
        /// <param name="array">Tablica, do której mają zostać skopiowane elementy listy.</param>
        /// <param name="index">Indeks, jaki ma mieć pierwszy element listy po skopiowaniu do tablicy.</param>
        public void CopyTo(Patient[] array, int index)
        {
            try
            {
                patientsList.CopyTo(array, index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Zwraca enumerator tej listy.
        /// </summary>
        /// <returns>Zwraca enumerator tej listy.</returns>
        public IEnumerator GetEnumerator()
        {
            return patientsList.GetEnumerator();
        }

        /// <summary>
        /// Determinuje czy lista zawiera pacjenta o podanym numerze PESEL.
        /// </summary>
        /// <param name="pesel">Numer PESEL do znalezienia na liście.</param>
        /// <returns>Zwraca true jeśli lista zawiera pacjenta o podanym numerze PESEL, false w przeciwnym razie.</returns>
        public bool Contains(Int64 pesel)
        {
            bool found = false;
            
            foreach (Patient p in patientsList)
            {
                if (p.Pesel == pesel)
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Podaje indeks w liście pacjenta o podanym numerze PESEL.
        /// </summary>
        /// <param name="pesel">Numer PESEL do znalezienia na liście.</param>
        /// <returns>Zwraca indeks podanego elementu lub -1, jeśli nie znaleziono.</returns>
        public int IndexOf(Int64 pesel)
        {
            int index = 0;

            for (; index < patientsList.Count; ++index)
            {
                if (patientsList[index].Pesel == pesel)
                    break;
            }

            if (index >= patientsList.Count)
                index = -1;

            return index;
        }

        #endregion // Public methods
    }
}
