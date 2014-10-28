using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Translator_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string filePath = "c:\\SavedText.txt";
        public MainWindow()
        {
            InitializeComponent();

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
            else LexicStatusRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.SandyBrown);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            String textToBeTranslated = TextInputBox.Text;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            {
                file.WriteLine(textToBeTranslated);
            }
            LexicStatusRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Green);
        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            String textFromFile;
            using (System.IO.StreamReader file = new StreamReader(filePath))
            {
                textFromFile = file.ReadToEnd();
            }
            TextInputBox.Text = textFromFile;
            LexicStatusRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Green);
        }

        private void TranslateButton_OnClick(object sender, RoutedEventArgs e)
        {
            String outputText;
            outputText = Translate(TextInputBox.Text);
            TextOutputBox.Text = outputText;
        }

        private string Translate(string inputText)
        {
            String outputText;
            OutputTable outputTable = new OutputTable();
            outputText = Translator.DoLexTranslate(inputText, ref outputTable);

            if (char.IsDigit(outputText[0]))
            {
                LexicStatusRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Green);

                String synthaxResult = Translator.DoSynthaxTranslate(outputTable);
                if (synthaxResult != null)
                {
                    SynthaxStatusRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red);
                    outputText = synthaxResult;
                }
                else SynthaxStatusRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Green);
            }
            else
                LexicStatusRectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red);

            OutputList.ItemsSource = outputTable.OutputRows;
            VariableList.ItemsSource = outputTable.Variables;
            ConstantList.ItemsSource = outputTable.Constants;

            

            return outputText;
        }
    }
}
