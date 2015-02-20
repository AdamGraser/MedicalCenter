using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MedicalCenter.Data;
using MedicalCenter.Models.Registrar;
using MedicalCenter.Services;

namespace MedicalCenter.GUI.Registrar
{
    /// <summary>
    /// Obsługa zdarzeń użytkownika dla widoku listy lekarzy przy rejestracji wizyty.
    /// </summary>
    public class RegisterVisitPresenter
    {
        #region Private fields

        /// <summary>
        /// Logika biznesowa w zakresie medycznej działalności placówki.
        /// </summary>
        MedicalBusinessService medicalBusinessService;

        /// <summary>
        /// Logika biznesowa w zakresie użytkowników systemu/pracowników.
        /// </summary>
        UserBusinessService userBusinessService;

        /// <summary>
        /// Logika biznesowa w zakresie pacjentów.
        /// </summary>
        PatientBusinessService patientBusinessService;

        /// <summary>
        /// Widok listy lekarzy (przy rejestracji wizyty) zarządzany przez tego prezentera.
        /// </summary>
        RegisterVisitView view;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący referencję do zarządzanego widoku i tworzący obiekty usług logiki biznesowej.
        /// </summary>
        /// <param name="view">Widok listy lekarzy (przy rejestracji wizyty) zarządzany przez tego prezentera.</param>
        public RegisterVisitPresenter(RegisterVisitView view)
        {
            medicalBusinessService = new MedicalBusinessService();
            userBusinessService = new UserBusinessService();
            patientBusinessService = new PatientBusinessService();
            this.view = view;
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Pobiera z bazy danych listę wszystkich lekarzy wraz z innymi potrzebnymi informacjami, a następnie zapisuje w odpowiedniej liście.
        /// </summary>
        public void GetDoctorsList()
        {
            // pobranie z bazy danych listy wszystkich lekarzy
            List<A_Worker> rawWorkersList = userBusinessService.GetSameWorkers(userBusinessService.GetJobTitleId("LEK"));

            int clinicId = 0;
            int visitsCount = 0;
            int maxVisitsCount = 0;
            bool? state = null;

            // wartość określająca czy rozpatrywany dzień jest wolny od pracy
            bool holiday = userBusinessService.IsHoliday(view.TheDate.SelectedDate.Value);

            // wyczyszczenie lub utworzenie nowej podstawowej listy lekarzy
            if (view.SourceDoctorsList == null)
                view.SourceDoctorsList = new List<DoctorsListItem>();
            else
                view.SourceDoctorsList.Clear();

            foreach (A_Worker w in rawWorkersList)
            {
                // pobranie ID poradni, do której przynależy lekarz w wybranym dniu
                // jeśli pobrane ID ma wartość 0, to lekarz jeszcze nie pracował/już nie pracuje w tej przychodni w wybranym dniu
                if ((clinicId = userBusinessService.GetClinicId(w.Id, view.TheDate.SelectedDate.Value)) == 0)
                    continue;

                // pobranie maksymalnej planowej liczby wizyt dla danego lekarza
                // liczba ujemna oznacza brak grafika danego lekarza, obejmującego wskazany dzień - lekarz ten więc nie może pojawić się na liście
                if ((maxVisitsCount = userBusinessService.GetVisitsPerDay(w.Id, view.TheDate.SelectedDate.Value)) < 0)
                    continue;
                
                // pobranie liczby wizyt zarejestrowanych na wskazany dzień do danego lekarza
                visitsCount = medicalBusinessService.TodaysVisitsCount(w.Id, view.TheDate.SelectedDate.Value);

                // jeśli wybrany dzień jest wolny od pracy, żaden lekarz nie przyjmuje
                // w przeciwnym razie trzeba sprawdzać grafik i nieobecności indywidualnie
                if (!holiday)
                {
                    // sprawdzenie, czy dany lekarz przyjmuje w danym dniu i czy ma jeszcze wolne godziny do przyjęcia pacjentów
                    if (maxVisitsCount == 0 || userBusinessService.IsWorkerAbsent(w.Id, view.TheDate.SelectedDate.Value))
                        state = null;
                    else if (visitsCount == maxVisitsCount)
                        state = false;
                    else
                        state = true;
                }

                // dodanie rekordu o lekarzu do listy
                view.SourceDoctorsList.Add(new DoctorsListItem(clinicId, medicalBusinessService.GetClinicName(clinicId), w.Id, w.LastName, w.FirstName,
                                                               visitsCount, userBusinessService.GetRoomNumber(w.Id, view.TheDate.SelectedDate.Value), state));
                                                                                
            }

            // wyczyszczenie lub utworzenie nowej listy źródłowej lekarzy
            if (view.DoctorsList == null)
                view.DoctorsList = new List<DoctorsListItem>();
            else
                view.DoctorsList.Clear();

            // wypełnienie listy lekarzy
            view.DoctorsList.AddRange(view.SourceDoctorsList);

            // odświeżenie tabeli
            view.DoctorsListTable.Items.Refresh();
        }

        /// <summary>
        /// Pobiera z bazy listę wszystkich poradni.
        /// </summary>
        public void GetClinicsList()
        {
            view.ClinicsList = medicalBusinessService.GetClinics();

            if(!view.ClinicsList.ContainsKey(0))
                view.ClinicsList.Add(0, "");
        }

        /// <summary>
        /// Filtruje zawartość tabeli z listą lekarzy wg. kryteriów podanych w filtrach.
        /// </summary>
        public void FilterDoctorsList()
        {
            // jeśli kontrolki filtrów już istnieją
            if (view.FilterClinicName != null && view.FilterDoctorName != null)
            {
                // tworzenie kolekcji uwzględniającej podane kryteria
                IEnumerable<DoctorsListItem> filteredList;

                // jeśli wybrano poradnię w filtrze
                if (view.FilterClinicName.SelectedIndex > 0)
                {
                    // aktywacja przycisku "Najbl. termin"
                    view.ClosestFreeDate.IsEnabled = true;

                    // jeśli wpisano również co najmniej 3 znaki w filtrze nazwiska
                    if (view.FilterDoctorName.Text.Length > 2)
                    {
                        filteredList = view.SourceDoctorsList.Where(x => x.ClinicId == view.ClinicsList.Keys.ElementAt(view.FilterClinicName.SelectedIndex)
                                                                      && (x.DoctorName.ToLower().StartsWith(view.FilterDoctorName.Text.ToLower())
                                                                      || view.FilterDoctorName.Text.ToLower().StartsWith(x.DoctorName.ToLower())));
                    }
                    // filtrowanie tylko po poradni
                    else
                    {
                        filteredList = view.SourceDoctorsList.Where(x => x.ClinicId == view.ClinicsList.Keys.ElementAt(view.FilterClinicName.SelectedIndex));
                    }
                }
                else
                {
                    // dezaktywacja przycisku "Najbl. termin"
                    view.ClosestFreeDate.IsEnabled = false;

                    // filtrowanie tylko po nazwiskach
                    if (view.FilterDoctorName.Text.Length > 2)
                    {
                        filteredList = view.SourceDoctorsList.Where(x => x.DoctorName.ToLower().StartsWith(view.FilterDoctorName.Text.ToLower())
                                                                      || view.FilterDoctorName.Text.ToLower().StartsWith(x.DoctorLastName.ToLower()));
                    }
                    // brak filtrowania
                    else
                    {
                        filteredList = view.SourceDoctorsList;
                    }
                }

                // wyczyszczenie źródłowej kolekcji
                view.DoctorsList.Clear();

                // zapełnienie źródłowej kolekcji elementami z tymczasowej kolekcji uwzględniającej kryteria,
                // posortowanymi wg. aktualnie używanego kryterium sortowania
                switch (view.Criteria)
                {
                    case SortingCriteria.SortByDoctorNameAscending:
                        view.DoctorsList.AddRange(filteredList.OrderBy(x => x.DoctorName));
                        break;

                    case SortingCriteria.SortByDoctorNameDescending:
                        view.DoctorsList.AddRange(filteredList.OrderByDescending(x => x.DoctorName));
                        break;

                    case SortingCriteria.SortByClinicNameAscending:
                        view.DoctorsList.AddRange(filteredList.OrderBy(x => x.ClinicName));
                        break;

                    case SortingCriteria.SortByClinicNameDescending:
                        view.DoctorsList.AddRange(filteredList.OrderByDescending(x => x.ClinicName));
                        break;

                    case SortingCriteria.SortByPatientsNumberAscending:
                        view.DoctorsList.AddRange(filteredList.OrderBy(x => x.PatientsNumber));
                        break;

                    case SortingCriteria.SortByPatientsNumberDescending:
                        view.DoctorsList.AddRange(filteredList.OrderByDescending(x => x.PatientsNumber));
                        break;

                    case SortingCriteria.SortByRoomNumberAscending:
                        view.DoctorsList.AddRange(filteredList.OrderBy(x => x.RoomNumber.Length).ThenBy(x => x.RoomNumber));
                        break;

                    case SortingCriteria.SortByRoomNumberDescending:
                        view.DoctorsList.AddRange(filteredList.OrderByDescending(x => x.RoomNumber.Length).ThenByDescending(x => x.RoomNumber));
                        break;
                }

                // odświeżenie tabeli
                view.DoctorsListTable.Items.Refresh();
            }
        }

        /// <summary>
        /// Zapisuje kryterium sortowania listy lekarzy.
        /// </summary>
        /// <param name="column">Kolumna, wg. której lista lekarzy została posortowana.</param>
        public void DoctorsListSorting(DataGridColumn column)
        {
            IEnumerable<DoctorsListItem> temp = new List<DoctorsListItem>();

            // stworzenie nowej kolekcji źródłowej, będącej posortowaną wg. nowego kryterium wersją bieżącej kolekcji źródłowej
            switch (view.DoctorsListTable.Columns.IndexOf(column))
            {
                case 0:
                    if (view.Criteria == SortingCriteria.SortByClinicNameAscending)
                    {
                        view.Criteria = SortingCriteria.SortByClinicNameDescending;
                        temp = new List<DoctorsListItem>(view.DoctorsList.OrderByDescending(x => x.ClinicName));
                    }
                    else
                    {
                        view.Criteria = SortingCriteria.SortByClinicNameAscending;
                        temp = new List<DoctorsListItem>(view.DoctorsList.OrderBy(x => x.ClinicName));
                    }
                    break;

                case 1:
                    if (view.Criteria == SortingCriteria.SortByDoctorNameAscending)
                    {
                        view.Criteria = SortingCriteria.SortByDoctorNameDescending;
                        temp = new List<DoctorsListItem>(view.DoctorsList.OrderByDescending(x => x.DoctorName));
                    }
                    else
                    {
                        view.Criteria = SortingCriteria.SortByDoctorNameAscending;
                        temp = new List<DoctorsListItem>(view.DoctorsList.OrderBy(x => x.DoctorName));
                    }
                    break;

                case 2:
                    if (view.Criteria == SortingCriteria.SortByPatientsNumberAscending)
                    {
                        view.Criteria = SortingCriteria.SortByPatientsNumberDescending;
                        temp = new List<DoctorsListItem>(view.DoctorsList.OrderByDescending(x => x.PatientsNumber));
                    }
                    else
                    {
                        view.Criteria = SortingCriteria.SortByPatientsNumberAscending;
                        temp = new List<DoctorsListItem>(view.DoctorsList.OrderBy(x => x.PatientsNumber));
                    }
                    break;

                case 3:
                    if (view.Criteria == SortingCriteria.SortByRoomNumberAscending)
                    {
                        view.Criteria = SortingCriteria.SortByRoomNumberDescending;
                        temp = new List<DoctorsListItem>(view.DoctorsList.OrderByDescending(x => x.RoomNumber.Length).ThenByDescending(x => x.RoomNumber));
                    }
                    else
                    {
                        view.Criteria = SortingCriteria.SortByRoomNumberAscending;
                        temp = new List<DoctorsListItem>(view.DoctorsList.OrderBy(x => x.RoomNumber.Length).ThenBy(x => x.RoomNumber));
                    }
                    break;
            }

            // ustawienie nowej kolekcji źródłowej jako źródła danych tabeli
            view.DoctorsList.Clear();
            view.DoctorsList.AddRange(temp);

            // odświeżenie tabeli
            view.DoctorsListTable.Items.Refresh();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót" w widoku listy lekarzy przy rejestracji wizyty.
        /// Czyści filtry, zaznaczenie i zmienia zawartość okna głównego aplikacji na menu główne.
        /// </summary>
        public void Back()
        {
            // przywrócenie domyślnego sortowania
            view.Criteria = SortingCriteria.SortByDoctorNameAscending;
            
            // wyczyszczenie filtrów
            view.FilterClinicName.SelectedIndex = -1;
            view.FilterDoctorName.Clear();

            // ustawienie dzisiejszej daty
            view.TheDate.SelectedDate = DateTime.Today;

            // wyczyszczenie zaznaczenia w tabeli
            view.DoctorsListTable.SelectedIndex = -1;

            // wyczyszczenie ID pacjenta
            view.PatientId = 0;

            // przywrócenie widoku menu głównego
            view.ParentWindow.ContentArea.Content = view.ParentWindow.History.Pop();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Dalej" w widoku listy lekarzy przy rejestracji wizyty.
        /// Przekazuje ID lekarza, pacjenta i wybraną datę do kolejnego ekranu rejestracji wizyty.
        /// </summary>
        public void Next()
        {
            // jeśli widok listy wizyt na dany dzień nie był dotychczas wyświetlany, musi zostać najpierw utworzony
            if (view.ParentWindow.RegistrarRegisterVisitDetailsView == null)
                view.ParentWindow.RegistrarRegisterVisitDetailsView = new RegisterVisitDetailsView(view.ParentWindow);

            // przekazanie do następnego widoku daty wizyty, ID pacjenta, ID oraz imię i nazwisko lekarza
            view.ParentWindow.RegistrarRegisterVisitDetailsView.VisitData.PatientId = view.PatientId;
            view.ParentWindow.RegistrarRegisterVisitDetailsView.VisitData.DoctorId = view.DoctorsList[view.DoctorsListTable.SelectedIndex].DoctorId;
            view.ParentWindow.RegistrarRegisterVisitDetailsView.DoctorName = userBusinessService.GetWorkerName(view.ParentWindow.RegistrarRegisterVisitDetailsView.VisitData.DoctorId);
            view.ParentWindow.RegistrarRegisterVisitDetailsView.VisitData.DateOfVisit = view.TheDate.SelectedDate.Value;
            view.ParentWindow.RegistrarRegisterVisitDetailsView.TheDate.SelectedDate = view.TheDate.SelectedDate.Value;

            // jeśli wybrano pacjenta, do następnego widoku przekazywane jest także jego imię i nazwisko
            if (view.PatientId > 0)
                view.ParentWindow.RegistrarRegisterVisitDetailsView.PatientName.Content = patientBusinessService.GetPatientName(view.PatientId);

            // jeśli data wizyty jest dzisiejsza, lista wizyt nie została automatycznie wypełniona,
            // więc musi to zostać wykonane ręcznie
            if (view.TheDate.SelectedDate.Value == DateTime.Today)
                view.ParentWindow.RegistrarRegisterVisitDetailsView.RefreshVisitsList();

            // zapisanie bieżącego widoku w historii widoków
            view.ParentWindow.History.Push(view);

            // zmiana treści okna głównego na widok listy wizyt na dany dzień u wybranego lekarza
            view.ParentWindow.ContentArea.Content = view.ParentWindow.RegistrarRegisterVisitDetailsView;
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Dodaj pacjenta" przy liście lekarzy (rejestracja wizyty).
        /// </summary>
        public void AddPatient()
        {
            // jeśli widok szczegółów pacjenta nie był dotychczas używany, należy go utworzyć
            if (view.ParentWindow.RegistrarPatientDetailsView == null)
                view.ParentWindow.RegistrarPatientDetailsView = new PatientDetailsView(view.ParentWindow);

            // zmiana zawartości okna głównego z menu na szczegóły pacjenta
            view.ParentWindow.ContentArea.Content = view.ParentWindow.RegistrarPatientDetailsView;

            // włączenie trybu edycji formularza na ekranie ze szczegółami pacjenta
            view.ParentWindow.RegistrarPatientDetailsView.EnableEditing();

            // zapisanie w historii referencji do tego widoku
            view.ParentWindow.History.Push(view);
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zaznaczenia w tabeli z listą lekarzy (aktywuje/dezaktywuje przyciski pod tabelą).
        /// </summary>
        public void DoctorSelected()
        {
            if (view.DoctorsListTable.SelectedIndex > -1)
            {
                view.Next.IsEnabled = true;

                if (view.DoctorsList[view.DoctorsListTable.SelectedIndex].State != true)
                    view.ClosestFreeDate.IsEnabled = true;
                else
                    view.ClosestFreeDate.IsEnabled = false;
            }
            else
            {
                view.ClosestFreeDate.IsEnabled = false;
                view.Next.IsEnabled = false;
            }
        }

        /// <summary>
        /// Kontroluje zakres wybranej daty, wczytuje listę lekarzy i filtruje ją.
        /// </summary>
        public void TheDateChanged()
        {
            // kontrola zakresu - przy rejestrowaniu wizyty rejestratorka ma widzieć dni od dziś w przód
            if (view.TheDate.SelectedDate == null)
                view.TheDate.SelectedDate = DateTime.Today;
            else if (view.TheDate.SelectedDate < DateTime.Today)
                view.TheDate.SelectedDate = DateTime.Today;

            // wczytanie na nowo listy lekarzy dla nowo wybranego dnia
            GetDoctorsList();

            // uwzględnienie filtracji
            FilterDoctorsList();
        }

        /// <summary>
        /// Szuka najbliższego dnia, w którym zaznaczony na liście lekarz ma wolną godzinę (istnieje możliwość zarejestrowania do niego wizyty).
        /// Gdy znajdzie taki dzień, ustawia go jako wybraną datę w polu pod listą lekarzy, co powoduje odświeżenie listy.
        /// </summary>
        public void ClosestFreeDate()
        {
            // lista lekarzy
            List<DoctorsListItem> doctors = new List<DoctorsListItem>();

            // jeśli wybrano lekarza z listy, to szukany jest najbliższy wolny termin tylko dla niego
            if (view.DoctorsListTable.SelectedIndex > -1)
            {
                doctors.Add(view.DoctorsList[view.DoctorsListTable.SelectedIndex]);
            }
            // w przeciwnym razie szukany jest najbliższy wolny termin dla któregokolwiek lekarza z listy
            // (z założenia w ramach jednej poradni)
            else if (view.FilterClinicName.SelectedIndex > 0)
            {
                doctors.AddRange(view.DoctorsList);
            }
            // (dlatego jeśli nie wybrano również żadnej poradni w filtrze, metoda nie podejmuje żadnych działań)
            else
            {
                return;
            }

            // wybrana data
            DateTime date = view.TheDate.SelectedDate.Value;

            // planowa maksymalna liczba wizyt
            int maxVisitsCount = 0;

            // true = znaleziono, false = nie znaleziono, null = w tej iteracji nie znaleziono jeszcze lekarza z grafikiem obejmującym wskazany dzień
            bool? found = null;

            // szukanie dnia niebędącego świętem, w którym wybrany(i) lekarz(e) przyjmuje(ą) pacjentów
            // wyszukiwanie trwa dopóki nie zostanie znaleziony dzień, w którym nie osiągnięto maksymalnej liczby zarejestrowanych do wybranego(ych) lekarza(y) wizyt
            do
            {
                // "niebędącego świętem"
                do { date = date.AddDays(1.0); } while (userBusinessService.IsHoliday(date));

                // wartość początkowa dla każdej iteracji
                found = null;

                foreach (DoctorsListItem selectedDoctor in doctors)
                {
                    // pobranie planowej maksymalnej liczby wizyt, możliwej do zarejestrowania na dany dzień u danego lekarza
                    maxVisitsCount = userBusinessService.GetVisitsPerDay(selectedDoctor.DoctorId, date);

                    if (maxVisitsCount < 0)
                        continue;
                    else
                    {
                        // sprawdzenie liczby wizyt zarejestrowanych na ten dzień i innych warunków stopu
                        // jeśli wszystkie spełnione -> koniec poszukiwań
                        if (maxVisitsCount > 0
                        && medicalBusinessService.TodaysVisitsCount(selectedDoctor.DoctorId, date) < maxVisitsCount
                        && !userBusinessService.IsWorkerAbsent(selectedDoctor.DoctorId, date))
                        {
                            found = true;
                            break;
                        }
                        else
                            found = false;
                    }
                }
            }
            while (found == false);

            // jeśli znaleziono wolny termin
            if (found == true)
            {
                // jeśli operacja dotyczyła jednego wybranego lekarza, następuje zapisanie jego ID
                int doctorId = (view.DoctorsListTable.SelectedIndex > -1) ? doctors[0].DoctorId : 0;

                // zmiana daty na "najbliższy wolny termin"
                view.TheDate.SelectedDate = date;

                // zaznaczenie na odświeżonej liście lekarza, którego dotyczyła ta operacja (jeśli dotyczyła jednego lekarza)
                DoctorsListItem selectedDoctor = view.DoctorsList.FirstOrDefault(x => x.DoctorId == doctorId);
                view.DoctorsListTable.SelectedIndex = (selectedDoctor != null) ? view.DoctorsList.IndexOf(selectedDoctor) : -1;
            }
            else
                System.Windows.Forms.MessageBox.Show("Brak wolnych terminów u "
                                                   + ((view.DoctorsListTable.SelectedIndex > -1)
                                                       ? "wybranego lekarza w okresie obowiązywania jego grafika."
                                                       : "wybranych lekarzy w okresach obowiązywania ich grafików.")
                                                   , "Brak wolnych terminów"
                                                   , System.Windows.Forms.MessageBoxButtons.OK
                                                   , System.Windows.Forms.MessageBoxIcon.Exclamation);
        }

        #endregion // Public methods
    }

    #region Enums

    /// <summary>
    /// Pole wyliczeniowe przedstawiające zestaw opcji sortowania dostępnych dla listy lekarzy.
    /// </summary>
    public enum SortingCriteria : byte
    {
        /// <summary>
        /// Sortowanie rosnąco według nazw poradni.
        /// </summary>
        SortByClinicNameAscending,

        /// <summary>
        /// Sortowanie malejąco według nazw poradni.
        /// </summary>
        SortByClinicNameDescending,

        /// <summary>
        /// Sortowanie rosnąco według nazwisk lekarzy.
        /// </summary>
        SortByDoctorNameAscending,

        /// <summary>
        /// Sortowanie malejąco według nazwisk lekarzy.
        /// </summary>
        SortByDoctorNameDescending,

        /// <summary>
        /// Sortowanie rosnąco według liczby pacjentów.
        /// </summary>
        SortByPatientsNumberAscending,

        /// <summary>
        /// Sortowanie malejąco według liczby pacjentów.
        /// </summary>
        SortByPatientsNumberDescending,

        /// <summary>
        /// Sortowanie rosnąco według numerów gabinetów.
        /// </summary>
        SortByRoomNumberAscending,

        /// <summary>
        /// Sortowanie malejąco według numerów gabinetów.
        /// </summary>
        SortByRoomNumberDescending
    };

    #endregion // Enums
}
