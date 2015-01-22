using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedicalCenter.GUI.Registrar
{
    /// <summary>
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : UserControl
    {
        #region Public properties

        /// <summary>
        /// Okno główne, którego treść stanowi ten widok.
        /// </summary>
        public MainWindow ParentWindow;

        #endregion // Public properties

        #region Ctors

        /// <summary>
        /// Konstruktor inicjalizujący pola i właściwości.
        /// </summary>
        /// <param name="parentWindow">Okno główne, którego treść stanowi ten widok.</param>
        public MainMenuView(MainWindow parentWindow)
        {
            InitializeComponent();

            this.ParentWindow = parentWindow;
        }

        #endregion // Ctors
    }
}
