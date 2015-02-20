using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using MedicalCenter.Services;

namespace MedicalCenter.GUI.Registrar
{
    /// <summary>
    /// Obsługa zdarzeń użytkownika dla widoku szczegółów pacjenta (działania na modelach i serwisach pod wpływem zdarzeń z widoku).
    /// </summary>
    public class PatientDetailsPresenter
    {
        #region Private fields

        /// <summary>
        /// Logika biznesowa w zakresie pacjentów.
        /// </summary>
        PatientBusinessService patientBusinessService;

        /// <summary>
        /// Widok szczegółów pacjenta zarządzany przez tego prezentera.
        /// </summary>
        PatientDetailsView view;

        /// <summary>
        /// Określa czy podczas walidacji numeru PESEL znalezione zostały błędy.
        /// </summary>
        bool peselValidationError;

        /// <summary>
        /// Określa czy podczas walidacji numeru PESEL znalezione zostały błędy.
        /// </summary>
        bool postalCodeValidationError;

        /// <summary>
        /// Określa, czy formularz jest w trybie edycji (true) czy w trybie podglądu (false).
        /// </summary>
        bool editMode;

        /// <summary>
        /// Instancja struktury Thickness, definiująca grubość linii o wartości 1.0, używana przy obramowaniach elementów formularza.
        /// </summary>
        System.Windows.Thickness thickness1;

        /// <summary>
        /// Instancja struktury Thickness, definiująca grubość linii o wartości 2.0, używana przy obramowaniach elementów formularza.
        /// </summary>
        System.Windows.Thickness thickness2;

        /// <summary>
        /// Minimalna data dla numeru PESEL.
        /// </summary>
        DateTime minDate;

        /// <summary>
        /// Maksymalna data dla numeru PESEL.
        /// </summary>
        DateTime maxDate;

        #endregion // Private fields

        #region Private properties

        /// <summary>
        /// Określa czy formularz został poprawnie wypełniony.
        /// </summary>
        bool IsFormCompleted
        {
            get
            {
                if (!peselValidationError &&
                    !postalCodeValidationError &&
                    view.LastName.Text.Length > 0 &&
                    view.FirstName.Text.Length > 0 &&
                    view.BirthDate.SelectedDate != null &&
                    view.Pesel.Text.Length > 0 &&
                    view.BuildingNumber.Text.Length > 0 &&
                    view.PostalCode.Text.Length > 0 &&
                    view.City.Text.Length > 0)

                    return true;

                else
                    return false;
            }
        }

        #endregion // Private properties

        #region Ctors

        /// <summary>
        /// Konstruktor zapisujący referencję do zarządzanego widoku i tworzący obiekt usług logiki biznesowej.
        /// </summary>
        /// <param name="view">Widok menu głównego rejestratorki zarządzany przez tego prezentera.</param>
        public PatientDetailsPresenter(PatientDetailsView view)
        {
            this.view = view;
            this.patientBusinessService = new PatientBusinessService();
            this.peselValidationError = false;
            this.editMode = true;
            this.thickness1 = new System.Windows.Thickness(1.0);
            this.thickness2 = new System.Windows.Thickness(2.0);
            this.minDate = new DateTime(1800, 1, 1);
            this.maxDate = new DateTime(2299, 12, 31);
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Sprawdza, czy wskazany klawisz należy do wskazanej grupy klawiszy.
        /// Dostępne grupy: cyfry, litery lub spacja, obie grupy.
        /// Niezależnie od wybranej grupy, zawsze sprawdzane jest, czy wskazany klawisz to delete, backspace, tab, return, escape lub enter.
        /// </summary>
        /// <param name="key">Klawisz do sprawdzenia.</param>
        /// <param name="kindOfKey">Grupa (lub kombinacja grup) klawiszy do sprawdzenia.</param>
        /// <returns>true jeśli jest to jeden z tych klawiszy, w przeciwnym razie false.</returns>
        public bool KindOfKey(Key key, GroupsOfKeys kindOfKey)
        {
            bool retval = false;

            // sprawdzenie czy klawisz jest cyfrą
            if ((kindOfKey & (GroupsOfKeys.Digits | GroupsOfKeys.Numerics)) != 0 &&
                ((key >= Key.D0 && key <= Key.D9 && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) ||
                    (key >= Key.NumPad0 && key <= Key.NumPad9)))
            {
                retval = true;
            }
            // sprawdzenie czy klawisz jest literą lub spacją (+ ew. wciśnięty alt i/lub shift)
            if ((kindOfKey & (GroupsOfKeys.Letters | GroupsOfKeys.Space)) != 0 &&
                ((key >= Key.A && key <= Key.Z) ||
                    key == Key.Space ||
                    Keyboard.Modifiers.HasFlag(ModifierKeys.Alt)))
            {
                retval = true;
            }
            // sprawdzenie czy klawisz jest jednym z wymienionych klawiszy specjalnych
            if (key == Key.Delete || key == Key.Back || key == Key.Tab || key == Key.Return || key == Key.Escape || key == Key.Enter)
                retval = true;

            return retval;
        }

        /// <summary>
        /// Usuwa z pola tekstowego wszystkie znaki nienależące do wskazanej grupy znaków.
        /// Obsługiwane grupy znaków: cyfry, litery i spacja, obie grupy naraz.
        /// </summary>
        /// <param name="textBox">Pole tekstowe, z którego mają zostać usunięte nieprawidłowe znaki. Wartość null powoduje, że ta funkcja nie dokonuje żadnej zmiany.</param>
        /// <param name="charactersGroup">Grupa (lub kombinacja grup) prawidłowych znaków.</param>
        public void TextBoxChanged(TextBox textBox, GroupsOfCharacters charactersGroup)
        {
            if (editMode)
            {
                if (textBox != null)
                {
                    bool change = false;

                    // rzucenie wyjątkiem jeśli pierwszy argument ma wartość null
                    if (textBox == null)
                        throw new ArgumentNullException("textBox", "Pole tekstowe ma wartość null");

                    // jeśli wybrana grupa to cyfry + ew. litery i spacja
                    if ((charactersGroup & GroupsOfCharacters.Digits) != 0)
                    {
                        bool both = ((charactersGroup & (GroupsOfCharacters.BigLetters | GroupsOfCharacters.SmallLetters | GroupsOfCharacters.Space)) != 0) ? true : false;

                        foreach (char c in textBox.Text)
                        {
                            // jeśli to nie cyfra            (i ew.           nie litera i nie spacja)
                            if (!char.IsDigit(c) && (!both || (!char.IsLetter(c) && c != ' ')))
                            {
                                // znajdź i zamień
                                textBox.Text = textBox.Text.Replace(c.ToString(), "");

                                change = true;
                            }
                        }
                    }
                    // jeśli wybrana grupa to litery i spacja
                    else if (charactersGroup == (GroupsOfCharacters.BigLetters | GroupsOfCharacters.SmallLetters | GroupsOfCharacters.Space))
                    {
                        foreach (char c in textBox.Text)
                        {
                            if (!char.IsLetter(c) && c != ' ')
                            {
                                textBox.Text = textBox.Text.Replace(c.ToString(), "");

                                change = true;
                            }
                        }
                    }

                    if (change)
                        textBox.CaretIndex = textBox.Text.Length;

                    // jeśli formularz został poprawnie wypełniony, należy aktywować przycisk "Zapisz"
                    view.Save.IsEnabled = IsFormCompleted;
                }
            }
        }

        /// <summary>
        /// Wywołuje swoje argumentowe przeciążenie, podając jako argument wartość określającą czy formularz jest w trybie edycji.
        /// </summary>
        public void Back()
        {
            Back(editMode);
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Powrót" w widoku szczegółów pacjenta.
        /// </summary>
        /// <param name="question">Wartość określająca czy wyświetlone ma zostać pytanie z prośbą o potwierdzenie operacji.</param>
        public void Back(bool question)
        {
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.Yes;

            // jeśli istnieje potrzeba, użytkownik pytany jest o potwierdzenie chęci powrotu do menu głównego bez zapisywania wprowadzonych zmian;
            // tylko jeśli użytkownik kliknął przycisk "Tak", należy wyczyścić wszystkie pola i wrócić do menu głównego
            if (question)
                dialogResult = System.Windows.Forms.MessageBox.Show("Czy na pewno chcesz wrócić do poprzedniej strony? Wszelkie zmiany nie zostaną zapisane.",
                                                     "Pytanie",
                                                     System.Windows.Forms.MessageBoxButtons.YesNo,
                                                     System.Windows.Forms.MessageBoxIcon.Question,
                                                     System.Windows.Forms.MessageBoxDefaultButton.Button2);

            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                // pobranie z historii widoków ostatniej pozycji
                UserControl last = view.ParentWindow.History.Pop();

                // sprawdzanie który z 3 możliwych widoków był ostatnio wyświetlany
                MainMenuView mainMenu = last as MainMenuView;

                // jeśli null, to znaczy, że to nie był ten widok
                if (mainMenu == null)
                {
                    RegisterVisitView registerVisit = last as RegisterVisitView;

                    if (registerVisit == null)
                    {
                        RegisterVisitDetailsView registerVisitDetails = last as RegisterVisitDetailsView;

                        // jeśli nie był to żaden z 3 możliwych widoków, to oznacza wystąpienie błędu
                        if (registerVisitDetails == null)
                        {
                            System.Windows.Forms.MessageBox.Show("Wystąpił nieoczekiwany błąd. Skontaktuj się z administratorem systemu.",
                                                                 "Nieoczekiwany błąd",
                                                                 System.Windows.Forms.MessageBoxButtons.OK,
                                                                 System.Windows.Forms.MessageBoxIcon.Error);

                            // wyczyszczenie historii widoków
                            view.ParentWindow.History.Clear();

                            // usunięcie referencji do widoków rejestracji wizyty
                            view.ParentWindow.RegistrarRegisterVisitView = null;
                            view.ParentWindow.RegistrarRegisterVisitDetailsView = null;

                            // powrót do menu głównego
                            view.ParentWindow.ContentArea.Content = view.ParentWindow.RegistrarMainMenuView;
                        }
                        else
                        {
                            // jeśli dodano nowego pacjenta do bazy danych
                            if (view.ViewTitle.Text.StartsWith("Dodaj"))
                                // przekazanie ID pacjenta do widoku rejestrowania wizyty
                                registerVisitDetails.VisitData.PatientId = view.PatientData.Id;
                            // jeśli zmieniono dane istniejącego w bazie pacjenta
                            else
                                // odświeżenie listy pacjentów po zmianie danych
                                registerVisitDetails.RefreshPatientsList();

                            // zmiana treści okna głównego na widok listy wizyt na dany dzień dla wybranego lekarza
                            view.ParentWindow.ContentArea.Content = registerVisitDetails;
                        }
                    }
                    else
                    {
                        // przekazanie ID pacjenta do widoku rejestrowania wizyty
                        registerVisit.PatientId = view.PatientData.Id;

                        // zmiana treści okna głównego na widok listy lekarzy
                        view.ParentWindow.ContentArea.Content = registerVisit;
                    }
                }
                else
                    view.ParentWindow.ContentArea.Content = mainMenu;

                // ustawienie wartości domyślnych w widoku szczegółów pacjenta
                view.LastName.Clear();
                view.FirstName.Clear();
                view.SecondName.Clear();
                // istotne jest, aby czyszczenie pesel'u było przed czyszczeniem płci - jeśli wybrana była kobieta i był wpisany prawidłowy pesel,
                // zmiana płci do wartości domyślnej (mężczyzna) spowoduje błąd walidacji pesel'u, który nie zostanie usunięty
                // (wyczyszczenie najpierw pesel'u, a potem płci, jest chyba najprostszym rozwiązaniem)
                view.Pesel.Clear();
                view.BirthDate.SelectedDate = null;
                view.Gender.SelectedIndex = 0;
                view.Street.Clear();
                view.BuildingNumber.Clear();
                view.Apartment.Clear();
                view.PostalCode.Clear();
                view.City.Clear();
                view.Post.Clear();
                view.IsInsured.IsChecked = true;
                view.Save.IsEnabled = false;
            }
        }

        /// <summary>
        /// Zapisuje dane formularza w bazie danych lub włącza tryb edycji formularza.
        /// </summary>
        public void Save()
        {
            if (editMode)
                // zapisanie zawartości formularza do bazy danych
                SaveChanges();
            else
                // włączenie trybu edycji
                EnableEditing(true);
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Zapisz" w widoku szczegółów pacjenta.
        /// </summary>
        public void SaveChanges()
        {
            long pesel;

            // próba konwersji PESEL'u ze string'a na 8-bajtowy int
            if (long.TryParse(view.Pesel.Text, out pesel))
            {
                // jeśli konwersja się powiodła, należy zapisać PESEL w postaci liczbowej do obiektu z danymi pacjenta
                view.PatientData.Pesel = pesel;
                
                // zapis informacji o pacjencie do bazy danych
                bool? saved = patientBusinessService.SavePatient(view.PatientData);

                // PESEL nie jest unikatowy - błąd
                if (saved == null)
                    System.Windows.Forms.MessageBox.Show("Podany numer PESEL już istnieje w bazie danych!",
                                                         "Nieprawidłowy PESEL!",
                                                         System.Windows.Forms.MessageBoxButtons.OK,
                                                         System.Windows.Forms.MessageBoxIcon.Warning);
                // wystąpił inny błąd
                else if (saved == false)
                    System.Windows.Forms.MessageBox.Show("Wystąpił błąd podczas próby zapisu informacji o pacjencie w bazie danych.",
                                                         "Błąd zapisu!",
                                                         System.Windows.Forms.MessageBoxButtons.OK,
                                                         System.Windows.Forms.MessageBoxIcon.Error);
                // prawidłowy zapis
                else
                {
                    System.Windows.Forms.MessageBox.Show("Informacje o pacjencie zostały pomyślnie zapisane w bazie danych.",
                                                         "Pomyślny zapis",
                                                         System.Windows.Forms.MessageBoxButtons.OK,
                                                         System.Windows.Forms.MessageBoxIcon.Information);

                    // pobranie z bazy danych ID nowo dodanego pacjenta
                    view.PatientData.Id = patientBusinessService.GetPatient(view.PatientData.Pesel).Id;

                    Back(false);
                }
            }
            // jeśli próba konwersji PESEL'u się nie powiodła, nie można zapisać danych pacjenta do bazy
            else
            {
                System.Console.WriteLine("Błąd podczas konwersji numeru PESEL z postaci stringowej na liczbę");

                System.Windows.Forms.MessageBox.Show("Wystąpił nieznany błąd.",
                                                     "Nieznany błąd!",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Włącza tryb edycji formularza na potrzeby dodania nowego pacjenta do bazy danych.
        /// </summary>
        public void EnableEditing()
        {
            EnableEditing(true);
            view.ViewTitle.Text = "Dodaj pacjenta";
        }

        /// <summary>
        /// Włącza lub wyłącza tryb edycji formularza na potrzeby podglądu/edycji danych istniejącego w bazie danych pacjenta.
        /// </summary>
        /// <param name="editMode">Określa czy formularz ma być w trybie edycji (true) czy podglądu (false).</param>
        public void EnableEditing(bool editMode)
        {
            // jeśli rzeczywiście nastąpić ma zmiana
            if (editMode != this.editMode)
            {
                // zapisanie nowego stanu formularza
                this.editMode = editMode;
                
                // zmiana stanu elementów formularza do domyślnego dla trybu edycji
                if (editMode)
                {
                    view.Save.Content = "Zapisz";
                    view.Save.IsEnabled = IsFormCompleted;
                    view.ViewTitle.Text = "Edycja pacjenta";
                }
                else
                {
                    view.Save.Content = "Edytuj";
                    view.Save.IsEnabled = true;
                    view.ViewTitle.Text = "Podgląd pacjenta";
                }

                view.LastName.IsEnabled =
                view.FirstName.IsEnabled =
                view.SecondName.IsEnabled =
                view.BirthDate.IsEnabled =
                view.Gender.IsEnabled =
                view.Pesel.IsEnabled =
                view.Street.IsEnabled =
                view.BuildingNumber.IsEnabled =
                view.Apartment.IsEnabled =
                view.PostalCode.IsEnabled =
                view.City.IsEnabled =
                view.Post.IsEnabled =
                view.IsInsured.IsEnabled = editMode;
            }
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola tekstowego "Pesel" w widoku szczegółów pacjenta.
        /// </summary>
        public void PeselChanged()
        {
            // jeśli pole nie jest edytowane, to wprowadzona wartość pochodzi ze zbindowanego obiektu "Patient PatientData"
            if (!view.Pesel.IsKeyboardFocused)
            {
                // pesel to liczba - jeśli jest zerem, to pole na pesel ma być puste
                if (view.Pesel.Text == "0")
                    view.Pesel.Clear();
                // pesel to liczba - brak zer z lewej strony
                else if (view.Pesel.Text.Length == 10)
                    view.Pesel.Text = "0" + view.Pesel.Text;
            }

            // usunięcie z pola nieprawidłowych znaków
            TextBoxChanged(view.Pesel, GroupsOfCharacters.Digits);
        }

        /// <summary>
        /// Walidacja wartości wprowadzonej do pola "Nr PESEL" dla pól powiązanych: "Nr PESEL", "Data urodzenia", "Płeć".
        /// </summary>
        public void ValidatePesel()
        {
            // musi być co walidować - pole na pesel nie może być puste
            if (view.Pesel.Text.Length > 0)
            {
                // określa czy podany pesel jest nieprawidłowy
                byte incorrectPesel = 0;

                // jeśli mniej niż 11 znaków, pesel jest nieprawidłowy
                if (view.Pesel.Text.Length < 11)
                {
                    incorrectPesel = 1;
                    view.Pesel.ToolTip = "Pesel musi mieć dokładnie 11 znaków!";
                }
                else
                {
                    int temp;

                    // jeśli podano datę urodzenia, można zwalidować pod tym kątem pesel
                    if (view.BirthDate.SelectedDate != null)
                    {
                        // dwie pierwsze cyfry pesel'u to młodsza połowa roku
                        int.TryParse(view.Pesel.Text.Substring(0, 2), out temp);

                        // jeśli młodsza połowa numeru roku w podanej dacie urodzenia jest inna niż ta podana w peselu,
                        // to należy zgłosić błąd w tych dwóch powiązanych ze sobą polach
                        if (view.BirthDate.SelectedDate.Value.Year % 100 != temp)
                            incorrectPesel = 2;
                        else
                        {
                            // trzecia i czwarta cyfra pesel'u tworzą nr miesiąca
                            int.TryParse(view.Pesel.Text.Substring(2, 2), out temp);

                            // miesiąc z daty urodzenia inny niż podany w pesel'u -> błąd
                            // (szczegóły tutaj: https://pl.wikipedia.org/wiki/PESEL#Data_urodzenia)
                            if (view.BirthDate.SelectedDate.Value.Year >= 1900 && view.BirthDate.SelectedDate.Value.Year < 2000)
                            {
                                if (view.BirthDate.SelectedDate.Value.Month != temp)
                                    incorrectPesel = 2;
                            }
                            else if (view.BirthDate.SelectedDate.Value.Year >= 2000 && view.BirthDate.SelectedDate.Value.Year < 2100)
                            {
                                if (view.BirthDate.SelectedDate.Value.Month != temp - 20)
                                    incorrectPesel = 2;
                            }
                            else if (view.BirthDate.SelectedDate.Value.Year >= 2100 && view.BirthDate.SelectedDate.Value.Year < 2200)
                            {
                                if (view.BirthDate.SelectedDate.Value.Month != temp - 40)
                                    incorrectPesel = 2;
                            }
                            else if (view.BirthDate.SelectedDate.Value.Year >= 2200 && view.BirthDate.SelectedDate.Value.Year < 2300)
                            {
                                if (view.BirthDate.SelectedDate.Value.Month != temp - 60)
                                    incorrectPesel = 2;
                            }
                            else if (view.BirthDate.SelectedDate.Value.Year >= 1800 && view.BirthDate.SelectedDate.Value.Year < 1900)
                            {
                                if (view.BirthDate.SelectedDate.Value.Month != temp - 80)
                                    incorrectPesel = 2;
                            }
                            else
                                incorrectPesel = 2;

                            if(incorrectPesel == 0)
                            {
                                // piąta i szósta cyfra pesel'u tworzą dzień miesiąca
                                int.TryParse(view.Pesel.Text.Substring(4, 2), out temp);

                                // dzień z daty urodzenia inny niż podany w pesel'u -> błąd
                                if (view.BirthDate.SelectedDate.Value.Day != temp)
                                    incorrectPesel = 2;
                            }
                        }
                    }

                    // walidacja pesel'u pod kątem płci
                    int.TryParse(view.Pesel.Text.Substring(9, 1), out temp);

                    // cyfra płci nieparzysta == mężczyzna, parzysta == kobieta
                    // na liście płci jest odwrotnie: SelectedIndex == 0 -> mężczyzna
                    // więc jeśli w pesel'u cyfra jest nieparzysta i SelectedIndex listy płci też jest nieparzysty (1) => błąd
                    if ((temp & 1) == view.Gender.SelectedIndex)
                    {
                        // jeśli wcześniej wykryto błąd na linii pesel <-> data urodzenia
                        if (incorrectPesel == 2)
                            incorrectPesel = 4;
                        else
                            incorrectPesel = 3;
                    }

                    // sprawdzanie cyfry kontrolnej
                    double[] multipliers = { 1.0, 3.0, 7.0, 9.0, 1.0, 3.0, 7.0, 9.0, 1.0, 3.0, 1.0, 0.0 };

                    // walidacja cyfry kontrolnej sprowadza się do następującego działania:
                    // obliczenia wyrażenia a+3b+7c+9d+e+3f+7g+9h+i+3j+k (gdzie litery oznaczają kolejne cyfry numeru)
                    for (int i = 0; i < view.Pesel.Text.Length; ++i)
                        multipliers[11] += char.GetNumericValue(view.Pesel.Text[i]) * multipliers[i];

                    // a następnie sprawdzenia czy reszta z dzielenia przez 10 jest zerem
                    if ((int)multipliers[11] % 10 != 0)
                        incorrectPesel += 10;
                }

                // jeśli pesel jest nieprawidłowy, należy to zaznaczyć w formularzu
                if (incorrectPesel > 0)
                {
                    // ustawienie flagi błędów w formularzu
                    peselValidationError = true;

                    // ustawienie czerwonych obramowań odpowiednim polom
                    view.Pesel.BorderBrush = System.Windows.Media.Brushes.Red;
                    view.Pesel.BorderThickness = thickness2;

                    // komunikat o nieprawidłowej sumie kontrolnej numeru PESEL
                    string wrongChecksum = string.Empty;

                    // zgłoszenie tego faktu odbywa się poprzez dodanie do incorrectPesel wartości 10
                    if (incorrectPesel > 9)
                    {
                        wrongChecksum = "Suma kontrolna numeru PESEL nie zgadza się!";
                        incorrectPesel -= 10;
                    }

                    if (incorrectPesel == 0 && wrongChecksum.Length > 0)
                    {
                        view.Pesel.ToolTip = wrongChecksum;

                        view.BirthDate.BorderBrush = view.Gender.BorderBrush = System.Windows.Media.Brushes.CornflowerBlue;
                        view.BirthDate.BorderThickness = view.Gender.BorderThickness = thickness1;
                        view.BirthDate.ToolTip = view.Gender.ToolTip = null;
                    }
                    else
                    {
                        if (wrongChecksum.Length > 0)
                            wrongChecksum = "\n" + wrongChecksum;

                        // jeśli błąd dotyczy także wartości dwóch pozostałych pól
                        if (incorrectPesel == 4)
                        {
                            view.BirthDate.BorderBrush = view.Gender.BorderBrush = System.Windows.Media.Brushes.Red;
                            view.BirthDate.BorderThickness = view.Gender.BorderThickness = thickness2;
                            view.BirthDate.ToolTip = view.Gender.ToolTip = view.Pesel.ToolTip = "Błąd w powiązanych polach Nr PESEL, Data urodzenia, Płeć!" + wrongChecksum;
                        }
                        // jeśli błąd dotyczy także wybranej płci
                        else if (incorrectPesel == 3)
                        {
                            view.Gender.BorderBrush = System.Windows.Media.Brushes.Red;
                            view.Gender.BorderThickness = thickness2;
                            view.Gender.ToolTip = view.Pesel.ToolTip = "Numer PESEL i płeć nie zgadzają się!" + wrongChecksum;

                            view.BirthDate.BorderBrush = System.Windows.Media.Brushes.CornflowerBlue;
                            view.BirthDate.BorderThickness = thickness1;
                            view.BirthDate.ToolTip = null;
                        }
                        // jeśli błąd dotyczy także wybranej daty urodzenia
                        else if (incorrectPesel == 2)
                        {
                            view.BirthDate.BorderBrush = System.Windows.Media.Brushes.Red;
                            view.BirthDate.BorderThickness = thickness2;
                            view.BirthDate.ToolTip = view.Pesel.ToolTip = "Numer PESEL i data urodzenia nie zgadzają się!" + wrongChecksum;

                            view.Gender.BorderBrush = System.Windows.Media.Brushes.CornflowerBlue;
                            view.Gender.BorderThickness = thickness1;
                            view.Gender.ToolTip = null;
                        }
                        else
                        {
                            view.BirthDate.BorderBrush = view.Gender.BorderBrush = System.Windows.Media.Brushes.CornflowerBlue;
                            view.BirthDate.BorderThickness = view.Gender.BorderThickness = thickness1;
                            view.BirthDate.ToolTip = view.Gender.ToolTip = null;
                        }
                    }
                }
                // pesel prawidłowy - przywracanie domyślnych ustawień pól formularza
                else
                {
                    view.Pesel.BorderThickness = view.BirthDate.BorderThickness = view.Gender.BorderThickness = thickness1;
                    view.Pesel.BorderBrush = view.BirthDate.BorderBrush = view.Gender.BorderBrush = System.Windows.Media.Brushes.CornflowerBlue;
                    view.BirthDate.ToolTip = view.Gender.ToolTip = view.Pesel.ToolTip = null;

                    peselValidationError = false;
                }

                // jeśli formularz został poprawnie wypełniony, należy aktywować przycisk "Zapisz"
                view.Save.IsEnabled = IsFormCompleted;
            }
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany daty w polu na datę urodzenia w widoku szczegółów pacjenta.
        /// </summary>
        public void BirthDateChanged()
        {
            // jeśli wybrana jest jakaś data (selectedDate != null)
            if (view.BirthDate.SelectedDate.HasValue)
            {
                // jeśli wybrana data jest wcześniejsza niż minimalna możliwa do zweryfikowania z numerem pesel,
                // należy zmienić ją na ową minimalną dla pesel'u datę
                if (view.BirthDate.SelectedDate.Value < minDate)
                    view.BirthDate.SelectedDate = minDate;
                // analogicznie dla daty maksymalnej
                else if (view.BirthDate.SelectedDate.Value > maxDate)
                    view.BirthDate.SelectedDate = maxDate;

                // jeśli formularz został poprawnie wypełniony, należy aktywować przycisk "Zapisz"
                view.Save.IsEnabled = IsFormCompleted;
            }
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola tekstowego na miejscowość w widoku szczegółów pacjenta.
        /// </summary>
        public void CityChanged()
        {
            bool change = false;
            
            // usuń z pola wszystkie znaki niebędące cyframi, literami, spacjami ani myślnikami
            foreach (char c in view.City.Text)
            {
                if (!char.IsDigit(c) && !char.IsLetter(c) && c != ' ' && c != '-')
                {
                    // znajdź i zamień
                    view.City.Text = view.City.Text.Replace(c.ToString(), "");

                    change = true;
                }
            }

            if (change)
                view.City.CaretIndex = view.City.Text.Length;

            // jeśli formularz został poprawnie wypełniony, należy aktywować przycisk "Zapisz"
            view.Save.IsEnabled = IsFormCompleted;
        }

        /// <summary>
        /// Obsługa zdarzenia wciśnięcia klawisza podczas edycji pola tekstowego na miejscowość w widoku szczegółów pacjenta.
        /// </summary>
        /// <param name="key">Klawisz do sprawdzenia.</param>
        /// <returns>true jeśli wskazany klawisz jest dozwolony w tym polu, false jeśli niedozwolony.</returns>
        public bool CityKeyDown(Key key)
        {
            // sprawdzenie, czy wskazany przycisk jest literą, spacją lub cyfrą
            bool retval = KindOfKey(key, GroupsOfKeys.Letters | GroupsOfKeys.Space | GroupsOfKeys.Digits | GroupsOfKeys.Numerics);

            // jeśli żadne z powyższych:
            if (!retval)
            {
                // sprawdzenie czy klawisz ten jest myślnikiem (minusem)
                if (key == Key.Subtract || key == Key.OemMinus)
                    retval = true;
            }

            return retval;
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany zawartości pola tekstowego na kod pocztowy w widoku szczegółów pacjenta.
        /// </summary>
        public void PostalCodeChanged()
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(view.PostalCode.Text, @"^\d{2}-\d{1,3}$"))
            {
                TextBoxChanged(view.PostalCode, GroupsOfCharacters.Digits);

                // cyfr w polu może być maksymalnie 5
                if (view.PostalCode.Text.Length == 6)
                    view.PostalCode.Text = view.PostalCode.Text.Substring(0, 5);

                // jeśli w polu wpisanych jest więcej niż 2 cyfry, należy zadbać o obecność myślnika
                if (view.PostalCode.Text.Length > 2)
                {
                    // sprawdzenie obecności myślnika na trzeciej pozycji
                    if (view.PostalCode.Text[2] != '-')
                    {
                        // zmiana tekstu wg. szablonu ^\d{2}-\d{1,3}$
                        view.PostalCode.Text = view.PostalCode.Text.Substring(0, 2) + "-" + view.PostalCode.Text.Substring(2, view.PostalCode.Text.Length - 2);
                        
                        // ustawienie karetki w polu na końcu tekstu
                        view.PostalCode.CaretIndex = view.PostalCode.Text.Length;
                    }
                }
            }
        }

        /// <summary>
        /// Walidacja kodu pocztowego.
        /// </summary>
        public void ValidatePostalCode()
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(view.PostalCode.Text, @"^\d{2}-\d{3}$"))
            {
                view.PostalCode.BorderBrush = System.Windows.Media.Brushes.Red;
                view.PostalCode.BorderThickness = thickness2;
                view.PostalCode.ToolTip = "Nieprawidłowy kod pocztowy!";

                postalCodeValidationError = true;
            }
            else
            {
                view.PostalCode.BorderBrush = System.Windows.Media.Brushes.CornflowerBlue;
                view.PostalCode.BorderThickness = thickness1;
                view.PostalCode.ToolTip = null;

                postalCodeValidationError = false;
            }

            // jeśli formularz został poprawnie wypełniony, należy aktywować przycisk "Zapisz"
            view.Save.IsEnabled = IsFormCompleted;
        }

        #endregion // Public methods
    }

    #region Enums

    /// <summary>
    /// Pole bitowe prezentujące grupy klawiszy.
    /// </summary>
    [Flags]
    public enum GroupsOfKeys : byte
    {
        /// <summary>
        /// Klawisze z cyframi (nad literami).
        /// </summary>
        Digits   = 0x01,

        /// <summary>
        /// Cyfry z klawiatury numerycznej.
        /// </summary>
        Numerics = 0x02,

        /// <summary>
        /// Klawisze z literami.
        /// </summary>
        Letters  = 0x04,

        /// <summary>
        /// Spacja.
        /// </summary>
        Space    = 0x08,

        /// <summary>
        /// Klawisze ze znakami specjalnymi.
        /// </summary>
        Special  = 0x16
    };

    /// <summary>
    /// Pole bitowe prezentujące grupy znaków.
    /// </summary>
    [Flags]
    public enum GroupsOfCharacters : byte
    {
        /// <summary>
        /// Małe litery.
        /// </summary>
        SmallLetters = 0x01,

        /// <summary>
        /// Wielkie litery.
        /// </summary>
        BigLetters   = 0x02,

        /// <summary>
        /// Spacja.
        /// </summary>
        Space        = 0x04,

        /// <summary>
        /// Białe znaki (zawierają spację).
        /// </summary>
        Whitespace   = 0x08,

        /// <summary>
        /// Cyfry.
        /// </summary>
        Digits       = 0x16,

        /// <summary>
        /// Znaki specjalne.
        /// </summary>
        Special      = 0x32
    };

    #endregion // Enums
}
