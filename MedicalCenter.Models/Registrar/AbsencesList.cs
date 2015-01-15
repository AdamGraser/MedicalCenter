using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje listę lekarzy nieobecnych we wskazanym dniu.
    /// </summary>
    class AbsencesList : IList
    {
        #region Private fields

        /// <summary>
        /// Lista lekarzy nieobecnych w danym dniu.
        /// </summary>
        private List<string> doctorsList;

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
        /// Data wskazująca dzień, w którym nieobecni są lekarze znajdujący się na tej liście.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Umożliwia pobranie referencji do elementu listy o wskazanym indeksie.
        /// Zmiana elementu o wskazanym indeksie jest dostępna tylko wewnątrz klasy.
        /// </summary>
        /// <param name="index">Indeks elementu, do którego referencja ma zostać zwrócona.</param>
        /// <returns>Element o wskazanym indeksie lub null, jeśli podany indeks jest spoza zakresu.</returns>
        public string this[int index]
        {
            get
            {
                try
                {
                    return doctorsList[index];
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
            private set
            {
                try
                {
                    doctorsList[index] = value;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
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
            get { return doctorsList.Count; }
        }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący pustą listę lekarzy.
        /// </summary>
        public AbsencesList()
        {
            doctorsList = new List<string>();
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Dodaje nowy element na koniec listy.
        /// </summary>
        /// <param name="newItem">Element, który ma zostać dodany do listy.</param>
        public void Add(string newItem)
        {
            doctorsList.Add(newItem);
        }

        /// <summary>
        /// Usuwa wszystkie elementy z listy.
        /// </summary>
        public void Clear()
        {
            doctorsList.Clear();
        }

        /// <summary>
        /// Determinuje czy lista zawiera podany element.
        /// </summary>
        /// <param name="item">Element, którego obecność w liście ma zostać sprawdzona.</param>
        /// <returns>Zwraca true jeśli lista zawiera podany element, false w przeciwnym razie.</returns>
        public bool Contains(string item)
        {
            return doctorsList.Contains(item);
        }

        /// <summary>
        /// Podaje indeks w liście wskazanego elementu.
        /// </summary>
        /// <param name="item">Element, którego indeks ma zostać podany.</param>
        /// <returns>Zwraca indeks podanego elementu lub -1, jeśli nie znaleziono.</returns>
        public int IndexOf(string item)
        {
            return doctorsList.IndexOf(item);
        }

        /// <summary>
        /// Wstawia element do listy na pozycję wskazywaną przez podany indeks.
        /// </summary>
        /// <param name="index">Indeks, jaki ma mieć element po wstawieniu do listy.</param>
        /// <param name="newItem">Element, który ma zostać wstawiony do listy.</param>
        public void Insert(int index, string newItem)
        {
            try
            {
                doctorsList.Insert(index, newItem);
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
        public void Remove(string item)
        {
            doctorsList.Remove(item);
        }

        /// <summary>
        /// Usuwa z listy element o podanym indeksie.
        /// </summary>
        /// <param name="index">Indeks elementu, który ma zostać usunięty.</param>
        public void RemoveAt(int index)
        {
            try
            {
                doctorsList.RemoveAt(index);
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
        public void CopyTo(string[] array, int index)
        {
            try
            {
                doctorsList.CopyTo(array, index);
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
            return doctorsList.GetEnumerator();
        }

        #endregion // Public methods
    }
}
