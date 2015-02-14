using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            foreach (A_Worker w in rawWorkersList)
            {
                // pobranie ID poradni, do której przynależy lekarz w wybranym dniu
                // jeśli pobrane ID ma wartość 0, to lekarz jeszcze nie pracował/już nie pracuje w tej przychodni w wybranym dniu
                if ((clinicId = userBusinessService.GetClinicId(w.Id, view.TheDate.SelectedDate.Value)) == 0)
                    continue;
                
                // pobranie liczby wizyt zarejestrowanych na wskazany dzień do danego lekarza
                visitsCount = medicalBusinessService.TodaysVisitsCount(w.Id, view.TheDate.SelectedDate.Value);
                // pobranie maksymalnej planowej liczby wizyt dla danego lekarza
                maxVisitsCount = userBusinessService.GetVisitsPerDay(w.Id, view.TheDate.SelectedDate.Value);

                // jeśli wybrany dzień jest wolny od pracy, żaden lekarz nie przyjmuje
                // w przeciwnym razie trzeba sprawdzać grafik i nieobecności indywidualnie
                if (!holiday)
                {
                    // sprawdzenie, czy dany lekarz przyjmuje w danym dniu i czy ma jeszcze wolne godziny do przyjęcia pacjentów
                    if (maxVisitsCount < 1 || userBusinessService.IsWorkerAbsent(w.Id, view.TheDate.SelectedDate.Value))
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

            view.DoctorsList = view.SourceDoctorsList;
        }

        /// <summary>
        /// Pobiera z bazy listę wszystkich poradni.
        /// </summary>
        public void GetClinicsList()
        {
            medicalBusinessService.GetClinics(view.ClinicsList);
        }

        /// <summary>
        /// Filtruje zawartość tabeli z listą lekarzy wg. kryteriów podanych w filtrach.
        /// </summary>
        public void FilterDoctorsList()
        {
            // jeśli kontrolki filtrów już istnieją
            if (view.FilterClinicName != null && view.FilterDoctorName != null)
            {
                // jeśli wybrano poradnię w filtrze
                if (view.FilterClinicName.SelectedIndex > 0)
                {
                    // jeśli wpisano również co najmniej 3 znaki w filtrze nazwiska
                    if (view.FilterDoctorName.Text.Length > 2)
                    {
                        view.DoctorsList = new List<DoctorsListItem>(view.SourceDoctorsList.Where(x => x.ClinicName == (view.FilterClinicName.SelectedItem as string)
                                                                                                    && (x.DoctorName.StartsWith(view.FilterDoctorName.Text)
                                                                                                     || view.FilterDoctorName.Text.StartsWith(x.DoctorName))).OrderBy(x => x.DoctorName));
                    }
                    // filtrowanie tylko po poradni
                    else
                    {
                        view.DoctorsList = new List<DoctorsListItem>(view.SourceDoctorsList.Where(x => x.ClinicName == (view.FilterClinicName.SelectedItem as string)).OrderBy(x => x.DoctorName));
                    }
                }
                // filtrowanie tylko po nazwiskach
                else if (view.FilterDoctorName.Text.Length > 2)
                {
                    view.DoctorsList = new List<DoctorsListItem>(view.SourceDoctorsList.Where(x => x.DoctorName.StartsWith(view.FilterDoctorName.Text)
                                                                                                || view.FilterDoctorName.Text.StartsWith(x.DoctorName)).OrderBy(x => x.DoctorName));
                }
                // brak filtrowania
                else
                {
                    view.DoctorsList = view.SourceDoctorsList;
                }
            }
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót" w widoku listy lekarzy przy rejestracji wizyty.
        /// Czyści filtry, zaznaczenie i zmienia zawartość okna głównego aplikacji na menu główne.
        /// </summary>
        public void Back()
        {
            // wyczyszczenie filtrów
            view.FilterClinicName.SelectedIndex = -1;
            view.FilterDoctorName.Clear();

            // wyczyszczenie zaznaczenia w tabeli
            view.DoctorsListTable.SelectedIndex = -1;

            // czyszczenie tabeli
            view.SourceDoctorsList.Clear();
            view.DoctorsList.Clear();

            // wyczyszczenie ID pacjenta
            view.PatientId = 0;

            // ustawienie dzisiejszej daty
            view.TheDate.SelectedDate = DateTime.Today;

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
            view.ParentWindow.RegistrarRegisterVisitDetailsView.VisitData.DateOfVisit = view.TheDate.SelectedDate.Value;
            view.ParentWindow.RegistrarRegisterVisitDetailsView.VisitData.PatientId = view.PatientId;
            view.ParentWindow.RegistrarRegisterVisitDetailsView.VisitData.DoctorId = view.DoctorsList[view.DoctorsListTable.SelectedIndex].DoctorId;
            view.ParentWindow.RegistrarRegisterVisitDetailsView.DoctorName = userBusinessService.GetWorkerName(view.ParentWindow.RegistrarRegisterVisitDetailsView.VisitData.DoctorId);

            // jeśli wybrano pacjenta, do następnego widoku przekazywane jest także jego imię i nazwisko
            if (view.PatientId > 0)
                view.ParentWindow.RegistrarRegisterVisitDetailsView.PatientName = patientBusinessService.GetPatientName(view.PatientId);

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
            view.SourceDoctorsList = new List<DoctorsListItem>();
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
            // referencja do zaznaczonego na liście lekarza
            DoctorsListItem selectedDoctor = view.DoctorsList[view.DoctorsListTable.SelectedIndex];
            // wybrana data
            DateTime date = view.TheDate.SelectedDate.Value;

            int visitsCount = 0;
            int maxVisitsCount = 0;

            do
            {
                // szukanie dnia niebędącego świętem, w którym wybrany lekarz przyjmuje pacjentów
                do
                {
                    date = date.AddDays(1.0);
                }
                while (userBusinessService.IsHoliday(date)
                    || userBusinessService.IsWorkerAbsent(selectedDoctor.DoctorId, date)
                    || (maxVisitsCount = userBusinessService.GetVisitsPerDay(selectedDoctor.DoctorId, date)) < 1);

                // pobranie liczby wizyt zarejestrowanych na ten dzień
                visitsCount = medicalBusinessService.TodaysVisitsCount(selectedDoctor.DoctorId, date);
            }
            // wyszukiwanie trwa dopóki nie zostanie znaleziony dzień, w którym nie osiągnięto maksymalnej liczby zarejestrowanych do wybranego lekarza wizyt
            while (visitsCount >= maxVisitsCount);

            // zapisanie ID lekarza
            visitsCount = selectedDoctor.DoctorId;

            // zmiana daty na "najbliższy wolny termin"
            view.TheDate.SelectedDate = date;

            // zaznaczenie na odświeżonej liście lekarza, którego dotyczyła ta operacja
            selectedDoctor = view.DoctorsList.FirstOrDefault(x => x.DoctorId == visitsCount);

            if (selectedDoctor != null)
                view.DoctorsListTable.SelectedIndex = view.DoctorsList.IndexOf(selectedDoctor);
        }

        #endregion // Public methods
    }
}
