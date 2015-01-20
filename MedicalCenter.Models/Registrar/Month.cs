using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje zbiór list nieobecnych lekarzy ze wszystkich dni wskazanego miesiąca w danym roku.
    /// </summary>
    public class Month : IList
    {
        #region Private fields

        /// <summary>
        /// Lista lekarzy nieobecnych w danym dniu.
        /// </summary>
        private List<AbsencesList> absencesList;

        #endregion // Private fields

        #region Public properties

        /// <summary>
        /// Miesiąc, z którego nieobecności przechowywane są w tej liście.
        /// </summary>
        public int MonthNumber { get; set; }

        /// <summary>
        /// Rok, z którego miesiąc jest brany pod uwagę.
        /// </summary>
        public int YearNumber { get; set; }

        /// <summary>
        /// Umożliwia pobranie referencji do elementu listy o wskazanym indeksie.
        /// Zmiana elementu o wskazanym indeksie jest dostępna tylko wewnątrz klasy.
        /// </summary>
        /// <param name="index">Indeks elementu, do którego referencja ma zostać zwrócona.</param>
        /// <returns>Element o wskazanym indeksie lub null, jeśli podany indeks jest spoza zakresu.</returns>
        public object this[int index]
        {
            get
            {
                try
                {
                    return absencesList[index];
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
            set
            {
                for(int i = 0; i < index; ++i);
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
            get { return absencesList.Count; }
        }

        /// <summary>
        /// Zwraca wartość określającą czy dostęp do kolekcji ICollection jest synchronizowany pomiędzy wątkami. (Odziedziczone z ICollection.)
        /// Zwraca wartość false.
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Zwraca obiekt, który może zostać użyty do synchronizacji dostępu do kolekcji ICollection. (Odziedziczone z ICollection.)
        /// Zwraca null.
        /// </summary>
        public object SyncRoot
        {
            get { return null; }
        }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący pustą listę lekarzy.
        /// </summary>
        public Month()
        {
            absencesList = new List<AbsencesList>();
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Dodaje nowy element na koniec listy.
        /// </summary>
        /// <param name="newItem">Element, który ma zostać dodany do listy.</param>
        /// <returns>Pozycja, na którą element został wstawiony, lub -1 by wskazać, że obiekt nie został dodany do kolekcji.</returns>
        public int Add(object newItem)
        {
            absencesList.Add(newItem as AbsencesList);

            return absencesList.Count - 1;
        }

        /// <summary>
        /// Usuwa wszystkie elementy z listy.
        /// </summary>
        public void Clear()
        {
            absencesList.Clear();
        }

        /// <summary>
        /// Determinuje czy lista zawiera podany element.
        /// </summary>
        /// <param name="item">Element, którego obecność w liście ma zostać sprawdzona.</param>
        /// <returns>Zwraca true jeśli lista zawiera podany element, false w przeciwnym razie.</returns>
        public bool Contains(object item)
        {
            return absencesList.Contains(item as AbsencesList);
        }

        /// <summary>
        /// Podaje indeks w liście wskazanego elementu.
        /// </summary>
        /// <param name="item">Element, którego indeks ma zostać podany.</param>
        /// <returns>Zwraca indeks podanego elementu lub -1, jeśli nie znaleziono.</returns>
        public int IndexOf(object item)
        {
            return absencesList.IndexOf(item as AbsencesList);
        }

        /// <summary>
        /// Wstawia element do listy na pozycję wskazywaną przez podany indeks.
        /// </summary>
        /// <param name="index">Indeks, jaki ma mieć element po wstawieniu do listy.</param>
        /// <param name="newItem">Element, który ma zostać wstawiony do listy.</param>
        public void Insert(int index, object newItem)
        {
            try
            {
                absencesList.Insert(index, newItem as AbsencesList);
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
        public void Remove(object item)
        {
            absencesList.Remove(item as AbsencesList);
        }

        /// <summary>
        /// Usuwa z listy element o podanym indeksie.
        /// </summary>
        /// <param name="index">Indeks elementu, który ma zostać usunięty.</param>
        public void RemoveAt(int index)
        {
            try
            {
                absencesList.RemoveAt(index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Pusta metoda (implementacja z konieczności).
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index) { }

        /// <summary>
        /// Zwraca enumerator tej listy.
        /// </summary>
        /// <returns>Zwraca enumerator tej listy.</returns>
        public IEnumerator GetEnumerator()
        {
            return absencesList.GetEnumerator();
        }

        #endregion // Public methods
    }
}
