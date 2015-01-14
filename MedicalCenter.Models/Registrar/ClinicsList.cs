using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Models.Registrar
{
    /// <summary>
    /// Reprezentuje listę poradni medycznych, używaną w filtrze listy lekarzy widoczną w widoku głównym rejestrowania wizyty przez rejestratorkę.
    /// </summary>
    class ClinicsList : IList
    {
        #region Private fields

        /// <summary>
        /// Lista nazw poradni medycznych.
        /// </summary>
        private List<string> clinicNameList;

        /// <summary>
        /// Lista ID poradni medycznych w bazie danych.
        /// </summary>
        private List<int> clinicIdList;

        #endregion // Private fields

        #region Private properties

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
        private object SyncRoot
        {
            get { return null; }
        }

        #endregion // Private properties

        #region Public properties

        /// <summary>
        /// Umożliwia pobranie referencji do nazwy poradni o wskazanym indeksie.
        /// Zmiana elementu o wskazanym indeksie jest dostępna tylko wewnątrz klasy.
        /// </summary>
        /// <param name="index">Indeks nazwy, do której referencja ma zostać zwrócona.</param>
        /// <returns>Nazwa poradni o wskazanym indeksie lub null, jeśli podany indeks jest spoza zakresu.</returns>
        public string this[int index]
        {
            get
            {
                try
                {
                    return clinicNameList[index];
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
                    clinicNameList[index] = value;
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
            get { return clinicNameList.Count; }
        }

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący pustą listę poradni medycznych.
        /// </summary>
        public ClinicsList()
        {
            clinicNameList = new List<string>();
            clinicIdList = new List<int>();
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Dodaje nową nazwę poradni wraz z ID na koniec listy poradni.
        /// </summary>
        /// <param name="newName">Nazwa, która ma zostać dodana do listy nazw.</param>
        /// <param name="newId">ID odpowiadające nowe liście.</param>
        public void Add(string newName, int newId)
        {
            clinicNameList.Add(newName);
            clinicIdList.Add(newId);
        }

        /// <summary>
        /// Usuwa wszystkie elementy z listy.
        /// </summary>
        public void Clear()
        {
            clinicNameList.Clear();
            clinicIdList.Clear();
        }

        /// <summary>
        /// Determinuje czy lista zawiera podaną nazwę poradni.
        /// </summary>
        /// <param name="item">Nazwa, której obecność w liście ma zostać sprawdzona.</param>
        /// <returns>Zwraca true jeśli lista zawiera podaną nazwę poradni, false w przeciwnym razie.</returns>
        public bool Contains(string item)
        {
            return clinicNameList.Contains(item);
        }

        /// <summary>
        /// Podaje indeks w liście wskazanej nazwy poradni.
        /// </summary>
        /// <param name="item">Nazwa, której indeks ma zostać podany.</param>
        /// <returns>Zwraca indeks podanej nazwy poradni lub -1, jeśli nie znaleziono.</returns>
        public int IndexOf(string item)
        {
            return clinicNameList.IndexOf(item);
        }

        /// <summary>
        /// Wstawia nazwę wraz z ID do listy na pozycję wskazywaną przez podany indeks.
        /// </summary>
        /// <param name="index">Indeks, jaki ma mieć element po wstawieniu do listy.</param>
        /// <param name="newName">Nazwa, która ma zostać wstawiona do listy.</param>
        /// <param name="newId">ID odpowiadające podanej nazwie.</param>
        public void Insert(int index, string newName, int newId)
        {
            try
            {
                clinicNameList.Insert(index, newName);
                clinicIdList.Insert(index, newId);
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
            clinicIdList.RemoveAt(clinicNameList.IndexOf(item));
            clinicNameList.Remove(item);
        }

        /// <summary>
        /// Usuwa z listy element o podanym indeksie.
        /// </summary>
        /// <param name="index">Indeks elementu, który ma zostać usunięty.</param>
        public void RemoveAt(int index)
        {
            try
            {
                clinicNameList.RemoveAt(index);
                clinicIdList.RemoveAt(index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Kopiuje wszystkie elementy listy do podanych tablic, wstawiając pierwszy element na pozycję o wskazanym indeksie.
        /// </summary>
        /// <param name="namesArray">Tablica, do której mają zostać skopiowane nazwy poradni medycznych.</param>
        /// <param name="idsArray">Tablica, do której mają zostać skopiowane ID odpowiadające nazwom poradni.</param>
        /// <param name="index">Indeks, jaki ma mieć pierwszy element listy po skopiowaniu do tablicy.</param>
        public void CopyTo(string[] namesArray, int[] idsArray, int index)
        {
            try
            {
                clinicNameList.CopyTo(namesArray, index);
                clinicIdList.CopyTo(idsArray, index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Zwraca enumerator listy nazw poradni.
        /// </summary>
        /// <returns>Zwraca enumerator listy nazw poradni.</returns>
        public IEnumerator GetEnumerator()
        {
            return clinicNameList.GetEnumerator();
        }

        /// <summary>
        /// Zwraca nazwę poradni medycznej o podanym ID z bazy danych.
        /// </summary>
        /// <param name="id">ID z bazy danych odpowiadające szukanej poradni.</param>
        /// <returns>Nazwę poradni medycznej o wskazanym ID lub null, jeśli nie znaleziono poradni o podanym ID.</returns>
        public string GetClinicName(int id)
        {
            try
            {
                return clinicNameList[clinicIdList.IndexOf(id)];
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Zwraca ID poradni medycznej o wskazanej nazwie.
        /// </summary>
        /// <param name="name">Nazwa poradni.</param>
        /// <returns>ID poradni medycznej o podanej nazwie lub -1, jeśli nie znaleziono poradni o podanej nazwie.</returns>
        public int GetClinicId(string name)
        {
            try
            {
                return clinicIdList[clinicNameList.IndexOf(name)];
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Zwraca ID poradni medycznej o podanym indeksie w liście poradni.
        /// </summary>
        /// <param name="name">Indeks poradni w liście poradni.</param>
        /// <returns>ID poradni medycznej o podanym indeksie lub -1, jeśli podany indeks jest spoza zakresu.</returns>
        public int GetClinicId(int index)
        {
            try
            {
                return clinicIdList[index];
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        #endregion // Public methods
    }
}
