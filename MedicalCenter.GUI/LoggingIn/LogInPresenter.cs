using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;
using MedicalCenter.Services;
using MedicalCenter.Models.LoggingIn;

namespace MedicalCenter.GUI.LoggingIn
{
    /// <summary>
    /// Obsługa zdarzeń użytkownika dla widoku logowania (działania na modelach i serwisach pod wpływem zdarzeń z widoku).
    /// </summary>
    public class LogInPresenter
    {
        #region Private fields

        /// <summary>
        /// Logika biznesowa w zakresie użytkowników systemu.
        /// </summary>
        UserBusinessService userBusinessService;

        /// <summary>
        /// Widok logowania zarządzany przez tego prezentera.
        /// </summary>
        LogInView view;

        #endregion // Private fields

        #region Ctors

        /// <summary>
        /// Konstruktor tworzący obiekt logiki biznesowej w zakresie użytkowników systemu i
        /// zapisujący referencję do zarządzanego widoku.
        /// </summary>
        /// <param name="view">Widok logowania zarządzany przez tego prezentera.</param>
        public LogInPresenter(LogInView view)
        {
            userBusinessService = new UserBusinessService();
            this.view = view;
        }

        #endregion // Ctors

        #region Public methods

        /// <summary>
        /// Nadaje focus polu na login.
        /// </summary>
        public void FocusLogin()
        {
            view.Login.Focus();
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Zaloguj" w formularzu logowania.
        /// </summary>
        public void Logon()
        {
            // zahashowanie hasła i zapisanie hashu w obiekcie danych użytkownika
            view.UserData.Password = view.Password.Password;

            try
            {
                // sprawdzenie poprawności podanych poświadczeń
                userBusinessService.LogIn(view.UserData);
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                Console.WriteLine("--------------------"
                                + "\nWystąpił błąd podczas próby połączenia się z serwerem."
                                + "\nInformacja: {0}"
                                + "\nŹródło: {1}"
                                + "\nW metodzie: {2}"
                                + "\nPomoc: {3}"
                                + "\nDodatkowe informacje: {4}"
                                + "\nStack trace:\n{5}"
                                + "\n--------------------\n"
                                , sqlEx.Message, sqlEx.Source, sqlEx.TargetSite, sqlEx.HelpLink, sqlEx.Data, sqlEx.StackTrace);

                view.Message.Content = "Wystąpił błąd podczas próby połączenia się z serwerem";
                view.UserData.Id = -0x0F;
            }
            catch (Exception ex)
            {
                Console.WriteLine("--------------------"
                                + "\nWystąpił błąd podczas próby sprawdzenia poprawności danych logowania."
                                + "\nInformacja: {0}"
                                + "\nŹródło: {1}"
                                + "\nW metodzie: {2}"
                                + "\nPomoc: {3}"
                                + "\nDodatkowe informacje: {4}"
                                + "\nStack trace:\n{5}"
                                + "\n--------------------\n"
                                , ex.Message, ex.Source, ex.TargetSite, ex.HelpLink, ex.Data, ex.StackTrace);

                view.Message.Content = "Wystąpił błąd przy sprawdzaniu poprawności danych";
                view.UserData.Id = -0x0F;
            }

            // jeśli były poprawne
            if (view.UserData.Id > 0)
            {
                if (view.UserData.JobTitleCode.StartsWith("REJ"))
                {
                    // wyczyszczenie pól z loginem, hasłem i hashem hasła oraz etykiety z komunikatem
                    view.Login.Clear();
                    view.UserData.Password = string.Empty;
                    view.Password.Clear();
                    view.Message.Content = string.Empty;

                    // zmiana tytułu okna głównego
                    view.ParentWindow.Title = view.UserData.Title;

                    // zapisanie ID aktualnie zalogowanej osoby
                    view.ParentWindow.Id = view.UserData.Id;

                    // zmiana ekranu logowania na menu główne
                    //if (view.UserData.JobTitleCode.StartsWith("REJ"))
                    //{
                    // jeśli widok menu głównego nie był dotychczas wyświetlany, należy go najpierw utworzyć
                    if (view.ParentWindow.RegistrarMainMenuView == null)
                        view.ParentWindow.RegistrarMainMenuView = new Registrar.MainMenuView(view.ParentWindow);

                    // zmiana ekranu logowania na menu główne
                    view.ParentWindow.ContentArea.Content = view.ParentWindow.RegistrarMainMenuView;
                }
                else
                {
                    // zaślepka
                    view.Message.Content = "Brak implementacji modułu dla tego stanowiska";
                    view.Login.Focus();
                    view.Login.SelectAll();
                    view.UserData.Id = 0;
                }
            }
            else
            {
                // wyświetlenie komunikatu o nieprawidłowych poświadczeniach
                if (view.UserData.Id == 0)
                    view.Message.Content = "Nieprawidłowy login i/lub hasło";
                // wyświetlenie komunikatu o dezaktywacji konta
                else
                {
                    if (view.UserData.Id == -1)
                        view.Message.Content = "Wskazane konto zostało dezaktywowane";
                    
                    // przywrócenie wartości domyślnej
                    view.UserData.Id = 0;
                }

                // zaznaczenie zawartości pola na login
                view.Login.Focus();
                view.Login.SelectAll();
            }
        }

        /// <summary>
        /// Zaznacza całą zawartość pola tekstowego/hasłowego przy nadaniu mu focusa.
        /// </summary>
        /// <param name="source">Źródło zdarzenia (przekazane z metody obsługującej zdarzenie GotFocus).</param>
        public void TextBoxFocused(object source)
        {
            // rzutowanie - sprawdzenie czy źródłem zdarzenia jest pole na login
            TextBox loginTextBox = source as TextBox;

            // jeśli to nie pole na login
            if (loginTextBox == null)
            {
                // rzutowanie - sprawdzenie czy źródłem zdarzenia jest pole na hasło (jedyna pozostała możliwość, ale...)
                PasswordBox passwordTextBox = source as PasswordBox;

                if (passwordTextBox != null)
                    passwordTextBox.SelectAll();
            }
            else
                loginTextBox.SelectAll();
        }

        /// <summary>
        /// Sprawdza czy login i hasło zostały podane. Jeśli tak, aktywuje przycisk "Zaloguj". W przeciwnym razie dezaktywuje go.
        /// </summary>
        public void FormCompleted()
        {
            // jeśli wpisano login i hasło, możliwe jest podjęcie próby zalogowania się
            if (view.Login.Text.Length > 0 && view.Password.Password.Length > 0)
                view.Logon.IsEnabled = true;
            else
            // jeśli nie, przycisk "Zaloguj" jest dezaktywowany
                view.Logon.IsEnabled = false;
        }

        /// <summary>
        /// Wyświetla dodatkowy widok konfiguracji połączenia z serwerem bazy danych.
        /// </summary>
        public void Configure()
        {
            // wyświetlenie widoku konfiguracji połączenia
            view.ConfigureConnectionView.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Aktywuje przycisk "Zapisz" w widoku konfiguracji połączenia z serwerem bazy danych, jeśli pole na adres serwera jest niepuste.
        /// W przeciwnym razie dezaktywuje ten przycisk.
        /// </summary>
        public void ConfigureAddressChanged()
        {
            if (view.ConfigureConnectionView.Address.Text.Length > 0)
                view.ConfigureConnectionView.Save.IsEnabled = true;
            else
                view.ConfigureConnectionView.Save.IsEnabled = false;
        }

        /// <summary>
        /// Obsługa kliknięcia przycisku "Powrót" w widoku konfiguracji połączenia z serwerem bazy danych.
        /// </summary>
        public void ConfigureBack()
        {
            // ukrycie widoku konfiguracji połączenia
            view.ConfigureConnectionView.Visibility = System.Windows.Visibility.Collapsed;

            // wyczyszczenie pola na adres serwera
            view.ConfigureConnectionView.Address.Clear();
        }

        /// <summary>
        /// Obsługa kliknięcia przycisku "Zapisz" w widoku konfiguracji połączenia z serwerem bazy danych.
        /// </summary>
        public void ConfigureSave()
        {
            // utworzenie obiektu klasy XmlSerializer, parsującej pliki XML
            XmlSerializer guiConfigSerializer = new XmlSerializer(typeof(GuiConfig));
            // utworzenie obiektu klasy XmlSerializerNamespaces, by usunąć domyślne przestrzeni nazw
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            // utworzenie referencji do obiektu klasy definiującej strukturę pliku konfiguracyjnego
            GuiConfig guiConfig;

            bool write = false;

            try
            {
                // wczytanie i sparsowanie zawartości pliku konfiguracyjnego
                using (StreamReader reader = new StreamReader("MedicalCenter.exe.config"))
                {
                    guiConfig = guiConfigSerializer.Deserialize(reader) as GuiConfig;
                }

                // utworzenie nowego obiektu ustawień połączenia z nowym adresem serwera:

                if (guiConfig.ConnectionStrings == null)
                    guiConfig.ConnectionStrings = new ConnectionStringsStructure();
                if (guiConfig.ConnectionStrings.Add == null)
                    guiConfig.ConnectionStrings.Add = new AddStructure();

                guiConfig.ConnectionStrings.Add.Name = "MedicalCenterDBContainer";
                guiConfig.ConnectionStrings.Add.ProviderName = "System.Data.EntityClient";
                guiConfig.ConnectionStrings.Add.ConnectionString
                    = "metadata=res://*/MedicalCenterDB.csdl|res://*/MedicalCenterDB.ssdl|res://*/MedicalCenterDB.msl;provider=System.Data.SqlClient;provider connection string=\"data source="
                    + view.ConfigureConnectionView.Address.Text
                    + ";initial catalog=MedicalCenter;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework\"";

                write = true;

                // dodanie pustej przetrzeni, by XmlSerializer nie dodał domyślnych przestrzeni jako atrybuty głównego elementu (powoduje crash)
                ns.Add("", "");

                // zapisanie nowej zawartości pliku konfiguracyjnego
                using (StreamWriter writer = new StreamWriter("MedicalCenter.exe.config", false))
                {
                    guiConfigSerializer.Serialize(writer, guiConfig, ns);
                }
            }
            catch (InvalidOperationException opEx)
            {
                Console.WriteLine("--------------------"
                                + (write ? "\nWystąpił błąd podczas próby zapisu ustawień do pliku konfiguracyjnego." : "\nWystąpił błąd podczas próby odczytu ustawień z pliku konfiguracyjnego.")
                                + "\nInformacja: {0}"
                                + "\nŹródło: {1}"
                                + "\nW metodzie: {2}"
                                + "\nPomoc: {3}"
                                + "\nDodatkowe informacje: {4}"
                                + "\nStack trace:\n{5}"
                                + "\n--------------------\n"
                                , opEx.InnerException.Message, opEx.InnerException.Source, opEx.InnerException.TargetSite, opEx.InnerException.HelpLink, opEx.InnerException.Data, opEx.StackTrace);

                System.Windows.Forms.MessageBox.Show((write
                                                     ? "Wystąpił błąd podczas próby zapisu ustawień do pliku konfiguracyjnego. Plik mógł zostać uszkodzony. Skontaktuj się z administratorem systemu."
                                                     : "Wystąpił błąd podczas próby odczytu ustawień z pliku konfiguracyjnego. Przejdź ponownie proces zmiany adresu serwera bądź skontaktuj się z administratorem systemu.")
                                                   , "Błąd " + (write ? "zapisu do" : "odczytu z") + " pliku"
                                                   , System.Windows.Forms.MessageBoxButtons.OK
                                                   , System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch (IOException ioEx)
            {
                Console.WriteLine("--------------------"
                                + "\nWystąpił błąd podczas próby otwarcia pliku konfiguracyjnego."
                                + "\nInformacja: {0}"
                                + "\nŹródło: {1}"
                                + "\nW metodzie: {2}"
                                + "\nPomoc: {3}"
                                + "\nDodatkowe informacje: {4}"
                                + "\nStack trace:\n{5}"
                                + "\n--------------------\n"
                                , ioEx.Message, ioEx.Source, ioEx.TargetSite, ioEx.HelpLink, ioEx.Data, ioEx.StackTrace);

                System.Windows.Forms.MessageBox.Show("Wystąpił błąd podczas próby otwarcia pliku konfiguracyjnego."
                                                     + (write ? " Zmiany mogły nie zostać zapisane." : "")
                                                     + " Przejdź ponownie proces zmiany adresu serwera bądź skontaktuj się z administratorem systemu."
                                                   , "Błąd otwacia pliku"
                                                   , System.Windows.Forms.MessageBoxButtons.OK
                                                   , System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine("--------------------"
                                + "\nWystąpił inny błąd podczas operacji zmiany ustawień połączenia z serwerem."
                                + "\nInformacja: {0}"
                                + "\nŹródło: {1}"
                                + "\nW metodzie: {2}"
                                + "\nPomoc: {3}"
                                + "\nDodatkowe informacje: {4}"
                                + "\nStack trace:\n{5}"
                                + "\n--------------------\n"
                                , ex.Message, ex.Source, ex.TargetSite, ex.HelpLink, ex.Data, ex.StackTrace);

                System.Windows.Forms.MessageBox.Show("Wystąpił nieznany błąd podczas operacji zmiany ustawień połączenia z serwerem. Skontaktuj się z administratorem systemu."
                                                   , "Nieznany błąd"
                                                   , System.Windows.Forms.MessageBoxButtons.OK
                                                   , System.Windows.Forms.MessageBoxIcon.Error);
            }

            // wyczyszczenie pola na adres serwera, ukrycie widoku konfiguracji połączenia
            ConfigureBack();
        }

        #endregion // Public methods
    }

    #region Configuration file structures

    #region Root elements

    /// <summary>
    /// Reprezentuje strukturę pliku konfiguracyjnego głównego pliku wykonywalnego aplikacji MedicalCenter.exe.
    /// </summary>
    [XmlRoot("configuration")]
    public class GuiConfig
    {
        /// <summary>
        /// Element "startup".
        /// </summary>
        [XmlElement("startup")]
        public StartupStructure Startup;

        /// <summary>
        /// Element "connectionStrings".
        /// </summary>
        [XmlElement("connectionStrings")]
        public ConnectionStringsStructure ConnectionStrings;
    }

    #endregion // Root elements

    #region Elements

    #region Level 1

    /// <summary>
    /// Reprezentuje strukturę elementu "startup" pliku konfiguracyjnego.
    /// </summary>
    public class StartupStructure
    {
        /// <summary>
        /// Element "supportedRuntime".
        /// </summary>
        [XmlElement("supportedRuntime")]
        public SupportedRuntimeStructure SupportedRuntime;
    }

    /// <summary>
    /// Reprezentuje strukturę elementu "connectionStrings" pliku konfiguracyjnego.
    /// </summary>
    public class ConnectionStringsStructure
    {
        /// <summary>
        /// Element "add".
        /// </summary>
        [XmlElement("add")]
        public AddStructure Add;
    }

    #endregion // Level 1

    #region Level 2

    /// <summary>
    /// Reprezentuje strukturę elementu "supportedRuntime", z elementu "startup", pliku konfiguracyjnego.
    /// </summary>
    public class SupportedRuntimeStructure
    {
        /// <summary>
        /// Atrybut "version" elementu "supportedRuntime".
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get; set; }

        /// <summary>
        /// Atrybut "sku" elementu "supportedRuntime".
        /// </summary>
        [XmlAttribute("sku")]
        public string Sku { get; set; }
    }

    /// <summary>
    /// Reprezentuje strukturę elementu "add", z elementu "connectionStrings", pliku konfiguracyjnego.
    /// </summary>
    public class AddStructure
    {
        /// <summary>
        /// Atrybut "name" elementu "add".
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Atrybut "connectionString" elementu "add".
        /// </summary>
        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Atrybut "providerName" elementu "add".
        /// </summary>
        [XmlAttribute("providerName")]
        public string ProviderName { get; set; }
    }

    #endregion // Level 2

    #endregion // Elements

    #endregion // Configuration file structures
}
