using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje listę wizyt zarejestrowanych na wskazany dzień.
    /// </summary>
    class VisitsList : IList
    {
        #region Private fields

        /// <summary>
        /// Lista wizyt zarejestrowanych na wskazany dzień.
        /// </summary>
        private List<VisitsListItem> visitsList;

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
        /// Umożliwia pobranie referencji do elementu listy o wskazanym indeksie.
        /// Zmiana elementu o wskazanym indeksie jest dostępna tylko wewnątrz klasy.
        /// </summary>
        /// <param name="index">Indeks elementu, do którego referencja ma zostać zwrócona.</param>
        /// <returns>Element o wskazanym indeksie lub null, jeśli podany indeks jest spoza zakresu.</returns>
        public VisitsListItem this[int index]
        {
            get
            {
                try
                {
                    return visitsList[index];
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
                    visitsList[index] = value;
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
            get { return visitsList.Count; }
        }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący pustą listę wizyt.
        /// </summary>
        public VisitsList()
        {
            visitsList = new List<VisitsListItem>();
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Dodaje nowy element na koniec listy.
        /// </summary>
        /// <param name="newItem">Element, który ma zostać dodany do listy.</param>
        public void Add(VisitsListItem newItem)
        {
            visitsList.Add(newItem);
        }

        /// <summary>
        /// Usuwa wszystkie elementy z listy.
        /// </summary>
        public void Clear()
        {
            visitsList.Clear();
        }

        /// <summary>
        /// Determinuje czy lista zawiera podany element.
        /// </summary>
        /// <param name="item">Element, którego obecność w liście ma zostać sprawdzona.</param>
        /// <returns>Zwraca true jeśli lista zawiera podany element, false w przeciwnym razie.</returns>
        public bool Contains(VisitsListItem item)
        {
            return visitsList.Contains(item);
        }

        /// <summary>
        /// Podaje indeks w liście wskazanego elementu.
        /// </summary>
        /// <param name="item">Element, którego indeks ma zostać podany.</param>
        /// <returns>Zwraca indeks podanego elementu lub -1, jeśli nie znaleziono.</returns>
        public int IndexOf(VisitsListItem item)
        {
            return visitsList.IndexOf(item);
        }

        /// <summary>
        /// Wstawia element do listy na pozycję wskazywaną przez podany indeks.
        /// </summary>
        /// <param name="index">Indeks, jaki ma mieć element po wstawieniu do listy.</param>
        /// <param name="newItem">Element, który ma zostać wstawiony do listy.</param>
        public void Insert(int index, VisitsListItem newItem)
        {
            try
            {
                visitsList.Insert(index, newItem);
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
        public void Remove(VisitsListItem item)
        {
            visitsList.Remove(item);
        }

        /// <summary>
        /// Usuwa z listy element o podanym indeksie.
        /// </summary>
        /// <param name="index">Indeks elementu, który ma zostać usunięty.</param>
        public void RemoveAt(int index)
        {
            try
            {
                visitsList.RemoveAt(index);
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
        public void CopyTo(VisitsListItem[] array, int index)
        {
            try
            {
                visitsList.CopyTo(array, index);
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
            return visitsList.GetEnumerator();
        }

        #endregion // Public methods
    }
}
