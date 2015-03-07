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
using System.Windows.Shapes;

namespace Translator_1
{
    /// <summary>
    /// Interaction logic for values.xaml
    /// </summary>
    public partial class ValuesWindow : Window
    {
        public ValuesWindow()
        {
            InitializeComponent();
        }

        public ValuesWindow(string text)
        {
            InitializeComponent();

            IdLabel.Content = "Please input values of theese variables: " + text;
        }
    }
}
