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
    /// Obsługa zdarzeń użytkownika dla widoku listy wizyt w danym dniu, u danego lekarza, przy rejestracji wizyty.
    /// </summary>
    public class RegisterVisitDetailsPresenter
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
        /// Widok listy wizyt w danym dniu (przy rejestracji wizyty) zarządzany przez tego prezentera.
        /// </summary>
        RegisterVisitDetailsView view;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący referencję do zarządzanego widoku i tworzący obiekty usług logiki biznesowej.
        /// </summary>
        /// <param name="view">Widok listy wizyt w danym dniu (przy rejestracji wizyty) zarządzany przez tego prezentera.</param>
        public RegisterVisitDetailsPresenter(RegisterVisitDetailsView view)
        {
            medicalBusinessService = new MedicalBusinessService();
            userBusinessService = new UserBusinessService();
            patientBusinessService = new PatientBusinessService();
            this.view = view;
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Pobiera i przekazuje do widoku listę wizyt z danego dnia dla wybranego lekarza oraz wyświetla nad listą godziny, w których lekarz przyjmuje pacjentów.
        /// Jeśli liczba wizyt osiągnęła bądź przekroczyła maksimum, uaktywniany jest checkbox "Nagły przypadek".
        /// </summary>
        public void GetVisitsList()
        {
            // wyczyszczenie lub utworzenie listy wizyt
            if (view.DailyVisits == null)
                view.DailyVisits = new List<DailyVisitsListItem>();
            else
            {
                // jeśli dotychczas to pole było zaznaczone, to musi zostać odznaczone
                // odznaczenie tego pola powoduje usunięcie ostatniego elementu z listy, więc musi to zostać zrobione przed pobraniem nowej listy
                view.IsEmergency.IsChecked = false;
                view.DailyVisits.Clear();
            }

            // pobranie listy wizyt
            List<DailyVisitsListItem> temp = medicalBusinessService.GetTodaysVisits(view.VisitData.DoctorId, view.VisitData.DateOfVisit);

            // wypełnienie listy wizyt
            if (temp != null)
                view.DailyVisits.AddRange(temp);

            // odświeżenie listy
            view.DailyVisitsList.Items.Refresh();

            // jeśli lekarz dziś przyjmuje
            if (view.DailyVisits.Count > 0 && view.TheDate.SelectedDate.Value.Date == DateTime.Today)
            {
                // wszystkie godziny zajęte -> aktywacja pola "Nagły przypadek"
                if (view.DailyVisits.All(x => x.PatientName != string.Empty))
                    view.IsEmergency.IsEnabled = true;
                else
                {
                    // aktywacja pola "Nagły przypadek"
                    view.IsEmergency.IsEnabled = true;

                    DateTime now = DateTime.Now.AddMinutes(-5.0);

                    // sprawdzenie wszystkich godzin na liście
                    foreach (DailyVisitsListItem v in view.DailyVisits)
                    {
                        // jeśli bieżąca godzina jest dostatecznie późna
                        if (now <= v.DateOfVisit)
                        {
                            // i jest wolna
                            if (v.PatientName == string.Empty)
                            {
                                // pole jest dezaktywowane, bo jednak jest jeszcze możliwość zarejestrowania się w ramach planowych godzin przyjęć lekarza
                                view.IsEmergency.IsEnabled = false;
                                break;
                            }
                        }
                    }
                }
            }
            // jeśli nie -> dezaktywacja pola "Nagły przypadek"
            else
                view.IsEmergency.IsEnabled = false;

            // pobranie z bazy danych godzin przyjęć lekarza w wybranym dniu
            string hours = userBusinessService.GetWorkingHours(view.VisitData.DoctorId, view.VisitData.DateOfVisit);
            view.Hours.Content = ((hours != null) ? hours : string.Empty);
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Dodaj pacjenta" przy liście wizyt (rejestracja wizyty).
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
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót" w widoku listy wizyt na dany dzień przy rejestracji wizyty.
        /// Czyści zaznaczenie i zmienia zawartość okna głównego aplikacji na menu główne.
        /// </summary>
        public void Back()
        {
            // wyczyszczenie zaznaczenia na liście
            view.DailyVisitsList.SelectedIndex = -1;

            // czyszczenie i dezaktywacja pola "Nagły przypadek"
            view.IsEmergency.IsChecked = false;
            view.IsEmergency.IsEnabled = false;

            // wyczyszczenie ID, nazwiska i imienia pacjenta oraz ID, nazwiska i imienia lekarza
            view.VisitData.PatientId = 0;
            view.PatientName.Content = " ";
            view.VisitData.DoctorId = 0;
            view.DoctorName.Text = string.Empty;

            // przywracanie domyślnej daty (co spowoduje wyczyszczenie listy wizyt i zniknięcie godzin pracy lekarza)
            view.TheDate.SelectedDate = DateTime.Today;

            // przywrócenie widoku listy lekarzy
            view.ParentWindow.ContentArea.Content = view.ParentWindow.History.Pop();
        }

        /// <summary>
        /// Zmienia wybraną datę wizyty na inny, najbliższy dzień, w którym wybrany lekarz przyjmuje pacjentów.
        /// Wynikowa data nie będzie wcześniejsza niż dzisiejsza.
        /// </summary>
        /// <param name="change">Minimalna ilość dni, o jaką ma zostać zmieniona data wizyty.</param>
        public void ChangeDate(double change)
        {
            // zmiana o 0 to nie zmiana
            if(change != 0.0)
            {
                // wykonanie pierwszej zmiany
                DateTime tempDate = view.VisitData.DateOfVisit.AddDays(change);

                // data nie może być wcześniejsza niż dzisiejsza
                if (tempDate < DateTime.Today)
                    tempDate = DateTime.Today;
                else
                {
                    change = (change < 0.0) ? -1.0 : 1.0;

                    // przesuwanie daty o 1 dzień tak długo, aż będzie to dzień, w którym wybrany lekarz przyjmuje pacjentów
                    while (userBusinessService.IsHoliday(tempDate)
                        || !userBusinessService.IsWorking(view.VisitData.DoctorId, tempDate)
                        || userBusinessService.IsWorkerAbsent(view.VisitData.DoctorId, tempDate))
                    {
                        tempDate = tempDate.AddDays(change);

                        // jeśli wcześniejszy dzień przyjęć pacjentów u danego lekarza już miał miejsce, żadna zmiana nie zostanie dokonana
                        if (tempDate < DateTime.Today)
                        {
                            tempDate = view.VisitData.DateOfVisit;
                            break;
                        }
                    }

                    // dokonanie zmiany daty wizyty
                    view.TheDate.SelectedDate = tempDate;
                }
            }
        }

        /// <summary>
        /// Obsługa zmiany daty wizyty.
        /// Kontrola zakresu wybranej daty i pobranie listy wizyt na wybrany dzień.
        /// </summary>
        public void TheDateChanged()
        {
            // uniemożliwienie niewybrania daty - zmiana na domyślną (dzisiejszą)
            if (view.TheDate.SelectedDate == null)
                view.TheDate.SelectedDate = DateTime.Today;
            // kontrola zakresu daty - nie może być wcześniejsza niż dzisiejsza
            else if (view.TheDate.SelectedDate < DateTime.Today)
                view.TheDate.SelectedDate = DateTime.Today;

            // pobranie listy wizyt dla danego lekarza we wskazanym dniu
            GetVisitsList();
        }

        /// <summary>
        /// Dodaje na koniec/usuwa z końca listy wizyt nową pozycję, reprezentującą wolną godzinę, na którą można zarejestrować wizytę dla nagłego przypadku.
        /// </summary>
        /// <param name="isChecked">Wartość określająca, czy pole "Nagły przypadek" jest zaznaczone.</param>
        public void Emergency(bool isChecked)
        {
            // jeśli to nagły przypadek, to dodawana jest nowa pozycja
            if (isChecked)
                view.DailyVisits.Add(new DailyVisitsListItem(view.DailyVisits[view.DailyVisits.Count - 1].DateOfVisit.AddMinutes(20.0)));
            // w przeciwnym razie usuwana jest ostatnia pozycja z listy
            else
                view.DailyVisits.RemoveAt(view.DailyVisits.Count - 1);

            // odświeżenie listy
            view.DailyVisitsList.Items.Refresh();
        }

        /// <summary>
        /// Sprawdza jakiego rodzaju pozycja została zaznaczona na liście wizyt i w razie potrzeby czyści to zaznaczenie lub aktywuje przycisk "Zarejestruj".
        /// </summary>
        public void HourSelected()
        {
            if (view.DailyVisitsList.SelectedIndex > -1)
            {
                // jeśli wybrano z listy wolną godzinę, z maksymalnie 5-minutowym opóźnieniem
                if (view.DailyVisits[view.DailyVisitsList.SelectedIndex].PatientName == string.Empty
                 && DateTime.Now.AddMinutes(-5.0) <= view.DailyVisits[view.DailyVisitsList.SelectedIndex].DateOfVisit)
                {
                    // zapisanie wybranej godziny
                    view.VisitData.DateOfVisit = view.DailyVisits[view.DailyVisitsList.SelectedIndex].DateOfVisit;

                    // oraz wybrano pacjenta, to przycisk "Zarejestruj" jest aktywowany
                    if (view.VisitData.PatientId > 0)
                        view.Register.IsEnabled = true;
                    // w przeciwnym razie przycisk ten jest dezaktywowany
                    else
                        view.Register.IsEnabled = false;
                }
                else
                {
                    view.Register.IsEnabled = false;
                    
                    // jeśli wybrano zajętą lub zbyt późną godzinę, zaznaczenie zostaje wyczyszczone
                    view.DailyVisitsList.SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// Pobranie z bazy danych kolekcji wszystkich pacjentów i wypełnienie nimi listy pacjentów.
        /// </summary>
        public void ChoosePatientFill()
        {
            // wyczyszczenie lub stworzenie w razie potrzeby listy źródłowej pacjentów
            if (view.PatientsListView.Patients == null)
                view.PatientsListView.Patients = new List<Patient>();
            else
                view.PatientsListView.Patients.Clear();

            // pobranie listy pacjentów
            view.PatientsListView.SourcePatients = patientBusinessService.GetPatients();
            view.PatientsListView.Patients.AddRange(view.PatientsListView.SourcePatients);

            // odświeżenie listy
            view.PatientsListView.PatientsListBox.Items.Refresh();
        }

        /// <summary>
        /// Odświeża zawartość listy pacjentów, zachowując zaznaczenie.
        /// </summary>
        public void ChoosePatientRefresh()
        {
            // zapisanie indeksu wybranego elementu (pacjenta)
            int selectedIndex = view.PatientsListView.PatientsListBox.SelectedIndex;

            // odświeżenie zawartości listy
            ChoosePatientFill();

            // zaznaczenie dotychczas wybranego elementu
            view.PatientsListView.PatientsListBox.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// Wyświetlenie listy pacjentów i ew. zaznaczenie wybranego już pacjenta (obsługa kliknięcia przycisku "Wybierz pacjenta" pod listą wizyt).
        /// </summary>
        public void ChoosePatient()
        {
            // wypełnienie listy pacjentów
            ChoosePatientFill();

            // zaznaczenie na liście pacjentów wybranego dotychczas pacjenta
            if (view.VisitData.PatientId > 0)
                view.PatientsListView.PatientsListBox.SelectedIndex = view.PatientsListView.Patients.IndexOf(view.PatientsListView.Patients.Find(x => x.Id == view.VisitData.PatientId));

            // pokazanie listy pacjentów
            view.PatientsListView.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Filtruje listę pacjentów.
        /// </summary>
        public void ChoosePatientFilter()
        {
            // wyczyszczenie listy pacjentów
            view.PatientsListView.Patients.Clear();

            IEnumerable<Patient> temp = new List<Patient>();
            
            // jeśli wpisano co najmniej 3 znaki, następuje filtrowanie pacjentów po nazwiskach
            if (view.PatientsListView.FilterPatientName.Text.Length > 2)
                temp = view.PatientsListView.SourcePatients.Where(x => x.LastName.ToLower().StartsWith(view.PatientsListView.FilterPatientName.Text.ToLower())
                                                                    || view.PatientsListView.FilterPatientName.Text.ToLower().StartsWith(x.LastName.ToLower()));
            // jeśli wpisano mniej niż 3 znaki, wyświetlana jest pełna lista pacjentów
            else
                temp = view.PatientsListView.SourcePatients;

            // uwzględnienie sortowania
            if (view.PatientsListView.SortDescending)
                view.PatientsListView.Patients.AddRange(temp.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName));
            else
                view.PatientsListView.Patients.AddRange(temp.OrderBy(x => x.LastName).ThenBy(x => x.FirstName));

            // odświeżenie listy
            view.PatientsListView.PatientsListBox.Items.Refresh();
        }

        /// <summary>
        /// Sortuje listę pacjentów rosnąco lub malejąco wg. nazwisk i imion.
        /// </summary>
        public void ChoosePatientSort()
        {
            // wyczyszczenie źródłowej listy pacjentów
            view.PatientsListView.Patients.Clear();

            // jeśli dotychczas lista była posortowana malejąco
            if (view.PatientsListView.SortDescending)
            {
                // zmiana sortowania na rosnące
                view.PatientsListView.SortDescending = false;
                view.PatientsListView.Sort.Content = "↑";
                view.PatientsListView.Sort.ToolTip = "Sortowanie rosnące. Kliknij, aby posortować malejąco.";
                view.PatientsListView.Patients.AddRange(view.PatientsListView.SourcePatients.OrderBy(x => x.LastName).ThenBy(x => x.FirstName));
            }
            else
            {
                // zmiana sortowania na rosnące
                view.PatientsListView.SortDescending = true;
                view.PatientsListView.Sort.Content = "↓";
                view.PatientsListView.Sort.ToolTip = "Sortowanie malejące. Kliknij, aby posortować rosnąco.";
                view.PatientsListView.Patients.AddRange(view.PatientsListView.SourcePatients.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName));
            }

            // odświeżanie listy
            view.PatientsListView.PatientsListBox.Items.Refresh();
        }

        /// <summary>
        /// Aktywuje/dezaktywuje przyciski "Szczegóły" i "Wybierz" w zależności od zaznaczenia pacjenta bądź jego braku.
        /// </summary>
        public void ChoosePatientSelected()
        {
            // wybranie pacjenta -> aktywacja przycisków
            if (view.PatientsListView.PatientsListBox.SelectedIndex > -1)
            {
                view.PatientsListView.Details.IsEnabled = true;
                view.PatientsListView.Choose.IsEnabled = true;
            }
            else
            {
                view.PatientsListView.Details.IsEnabled = false;
                view.PatientsListView.Choose.IsEnabled = false;
            }
        }

        /// <summary>
        /// Obsługa kliknięcia przycisku "Powrót" pod listą pacjentów.
        /// </summary>
        public void ChoosePatientBack()
        {
            // ukrycie widoku listy pacjentów
            view.PatientsListView.Visibility = System.Windows.Visibility.Collapsed;

            // czyszczenie listy pacjentów
            view.PatientsListView.Patients.Clear();
            view.PatientsListView.SourcePatients = null;
            view.PatientsListView.PatientsListBox.Items.Refresh();

            // wyczyszczenie filtra
            view.PatientsListView.FilterPatientName.Clear();
        }

        /// <summary>
        /// Wyświetla szczegóły pacjenta (obsługa kliknięcia przycisku "Szczegóły" pod listą pacjentów).
        /// </summary>
        public void ChoosePatientDetails()
        {
            // dodanie tego widoku do historii
            view.ParentWindow.History.Push(view);

            // jeśli widok szczegółów pacjenta nie był dotychczas używany, należy go utworzyć
            if (view.ParentWindow.RegistrarPatientDetailsView == null)
                view.ParentWindow.RegistrarPatientDetailsView = new PatientDetailsView(view.ParentWindow);
            
            // zmiana treści okna głównego na widok szczegółów pacjenta
            view.ParentWindow.ContentArea.Content = view.ParentWindow.RegistrarPatientDetailsView;

            // ustawienie trybu podglądu danych pacjenta
            view.ParentWindow.RegistrarPatientDetailsView.EnableEditing(false);

            // przekazanie danych pacjenta do nowego widoku
            view.ParentWindow.RegistrarPatientDetailsView.PatientData = view.PatientsListView.Patients[view.PatientsListView.PatientsListBox.SelectedIndex];
            view.ParentWindow.RegistrarPatientDetailsView.Apartment.Text = view.ParentWindow.RegistrarPatientDetailsView.PatientData.Apartment;
            view.ParentWindow.RegistrarPatientDetailsView.BirthDate.SelectedDate = view.ParentWindow.RegistrarPatientDetailsView.PatientData.BirthDate;
            view.ParentWindow.RegistrarPatientDetailsView.BuildingNumber.Text = view.ParentWindow.RegistrarPatientDetailsView.PatientData.BuildingNumber;
            view.ParentWindow.RegistrarPatientDetailsView.City.Text = view.ParentWindow.RegistrarPatientDetailsView.PatientData.City;
            view.ParentWindow.RegistrarPatientDetailsView.FirstName.Text = view.ParentWindow.RegistrarPatientDetailsView.PatientData.FirstName;
            view.ParentWindow.RegistrarPatientDetailsView.Gender.SelectedIndex = ((view.ParentWindow.RegistrarPatientDetailsView.PatientData.Gender) ? 1 : 0);
            view.ParentWindow.RegistrarPatientDetailsView.IsInsured.IsChecked = view.ParentWindow.RegistrarPatientDetailsView.PatientData.IsInsured;
            view.ParentWindow.RegistrarPatientDetailsView.LastName.Text = view.ParentWindow.RegistrarPatientDetailsView.PatientData.LastName;
            view.ParentWindow.RegistrarPatientDetailsView.Pesel.Text = view.ParentWindow.RegistrarPatientDetailsView.PatientData.Pesel.ToString();
            view.ParentWindow.RegistrarPatientDetailsView.Post.Text = view.ParentWindow.RegistrarPatientDetailsView.PatientData.Post;
            view.ParentWindow.RegistrarPatientDetailsView.PostalCode.Text = view.ParentWindow.RegistrarPatientDetailsView.PatientData.PostalCode;
            view.ParentWindow.RegistrarPatientDetailsView.SecondName.Text = view.ParentWindow.RegistrarPatientDetailsView.PatientData.SecondName;
            view.ParentWindow.RegistrarPatientDetailsView.Street.Text = view.ParentWindow.RegistrarPatientDetailsView.PatientData.Street;
        }

        /// <summary>
        /// Przekazanie ID wybranego pacjenta do głównego widoku i schowanie widoku listy pacjentów (obsługa kliknięcia przycisku "Wybierz" pod listą pacjentów).
        /// </summary>
        public void ChoosePatientSelect()
        {
            // przekazanie ID oraz nazwiska i imienia wybranego pacjenta
            view.VisitData.PatientId = view.PatientsListView.Patients[view.PatientsListView.PatientsListBox.SelectedIndex].Id;
            view.PatientName.Content = view.PatientsListView.Patients[view.PatientsListView.PatientsListBox.SelectedIndex].LastName + " "
                                     + view.PatientsListView.Patients[view.PatientsListView.PatientsListBox.SelectedIndex].FirstName;
            
            // schowanie listy pacjentów
            ChoosePatientBack();

            // ew. aktywacja przycisku "Zarejestruj"
            if(view.DailyVisitsList.SelectedIndex > -1)
                view.Register.IsEnabled = true;
            else
                view.Register.IsEnabled = false;
        }

        /// <summary>
        /// Rejestruje nową wizytę i powraca do menu głównego.
        /// </summary>
        public void RegisterVisit()
        {
            // zapis informacji o wizycie do bazy danych
            bool? saved = medicalBusinessService.RegisterVisit(view.VisitData, view.ParentWindow.Id);

            // wystąpił błąd
            if (saved != true)
                System.Windows.Forms.MessageBox.Show("Wystąpił błąd podczas próby zapisu informacji o wizycie w bazie danych.",
                                                     "Błąd zapisu!",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Error);
            else
            {
                System.Windows.Forms.MessageBox.Show("Wizyta została pomyślnie zarejestrowana.",
                                                     "Pomyślny zapis",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Information);

                // powrót do menu głównego
                Back();
                view.ParentWindow.RegistrarRegisterVisitView.ViewBack();
            }
        }

        #endregion // Public methods
    }
}
