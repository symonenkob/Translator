using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using Translator_1.AscendingParse;
using Translator_1.RPN;
using Path = System.IO.Path;

namespace Translator_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string filePath = Environment.CurrentDirectory + "\\ProgramExample.txt";
        OutputTable outputTable = new OutputTable();
        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(filePath))
            {
                LoadButton.IsEnabled = false;
            }

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
            else LexicStatusRectangle.Background = new SolidColorBrush(System.Windows.Media.Colors.SandyBrown);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            String textToBeTranslated = TextInputBox.Text;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            {
                file.WriteLine(textToBeTranslated);
            }
            LexicStatusRectangle.Background = new SolidColorBrush(System.Windows.Media.Colors.Green);
            LoadButton.IsEnabled = true;
        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            String textFromFile;
            using (System.IO.StreamReader file = new StreamReader(filePath))
            {
                textFromFile = file.ReadToEnd();
            }
            TextInputBox.Text = textFromFile;
            LexicStatusRectangle.Background = new SolidColorBrush(System.Windows.Media.Colors.Green);
        }

        private void TranslateButton_OnClick(object sender, RoutedEventArgs e)
        {
            String outputText;
            outputText = Translate(TextInputBox.Text);
            TextOutputBox.Text = outputText;

            AutomateButton.IsEnabled = true;
            RelationTableButton.IsEnabled = true;
            AscendingButton.IsEnabled = true;
            RpnButton.IsEnabled = true;
            IfRpnButton.IsEnabled = true;
        }

        private string Translate(string inputText)
        {
            String outputText;

            outputText = Translator.DoLexTranslate(inputText, ref outputTable);

            if (char.IsDigit(outputText[0]))
            {
                LexicStatusRectangle.Background = new SolidColorBrush(System.Windows.Media.Colors.Green);

                String synthaxResult = Translator.DoSynthaxTranslate(outputTable);
                if (synthaxResult != null)
                {
                    SynthaxStatusRectangle.Background = new SolidColorBrush(System.Windows.Media.Colors.Red);
                    outputText = synthaxResult;
                }
                else SynthaxStatusRectangle.Background = new SolidColorBrush(System.Windows.Media.Colors.Green);
            }
            else
                LexicStatusRectangle.Background = new SolidColorBrush(System.Windows.Media.Colors.Red);

            OutputList.ItemsSource = outputTable.OutputRows;
            VariableList.ItemsSource = outputTable.Variables;
            ConstantList.ItemsSource = outputTable.Constants;

            return outputText;
        }

        private void AutomatButton_OnClick(object sender, RoutedEventArgs e)
        {
            AutomateTabItem.IsSelected = true;
            Automate automate = new Automate();

            List<AutomateRow> resulingAutomateRows = automate.DoAutomateTranslate(outputTable.OutputRows);
            AutomateRowsListView.ItemsSource = resulingAutomateRows;
        }

        private async void Lab5Button_Click(object sender, RoutedEventArgs e)
        {
            Lab5TabItem.IsSelected = true;
            RelationTableStatusLabel.Background = new SolidColorBrush(Colors.Red);
            AscendingParseStatusLabel.Background = new SolidColorBrush(Colors.Red);

            if (TableConstructor.Table == null)
                await Task.Run(() => TableConstructor.Construct());
            RelationTableStatusLabel.Background = new SolidColorBrush(Colors.Green);

            List<AscOutputRow> resultingRows = AscendingTranslator.Translate(outputTable.GetLexemsOnly());
            if (resultingRows.Last().InputChain.Length == 0)
            {
                AscendingParseStatusLabel.Background = new SolidColorBrush(Colors.Green);
            }
            Lab5RowsListView.ItemsSource = resultingRows;
        }

        private void RelationTableButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Environment.CurrentDirectory + @"\translatorsLab4\translatorsLab4\bin\Debug\translatorsLab4.exe");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load file");
            }
        }

        private async void RpnButton_OnClick(object sender, RoutedEventArgs e)
        {
            RelationTableStatusLabel.Background = new SolidColorBrush(Colors.Red);
            AscendingParseStatusLabel.Background = new SolidColorBrush(Colors.Red);

            RpnStatusLabel.Visibility = Visibility.Visible;
            RpnStatusLabel.Content = "Loading...";

            if (TableConstructor.Table == null)
                await Task.Run(() => TableConstructor.Construct());
            RelationTableStatusLabel.Background = new SolidColorBrush(Colors.Green);

            List<AscOutputRow> resultingRows = AscendingTranslator.Translate(outputTable.GetLexemsOnly(), outputTable.GetLexemsOnly(true), true);

            if (resultingRows.Last().Stack == "# <врж1>")
            {
                AscendingParseStatusLabel.Background = new SolidColorBrush(Colors.Green);
            }

            bool inputSucceeded = true;
            List<string> ids = outputTable.GetIds();
            if (ids.Count > 0)
            {
                ValuesWindow win = new ValuesWindow(string.Join(" ", ids));
                win.ShowDialog();
                string[] values = win.Values.Text.Split();

                if (ids.Count == values.Length)
                {
                    ids.ForEach(
                        i => resultingRows.Last().Rpn = resultingRows.Last().Rpn.Replace(i, values[ids.IndexOf(i)]));
                }
                else
                {
                    inputSucceeded = false;
                }
            }
            Lab5TabItem.IsSelected = true;

            try
            {
                if (inputSucceeded == true && !string.IsNullOrEmpty(resultingRows.Last().InputChain))
                {
                    resultingRows.Add(new AscOutputRow()
                    {
                        Rpn = "result = " + RpnCalculator.Calculate(resultingRows.Last().Rpn)
                    });
                }
            }
            catch (DivideByZeroException ex)
            {
                resultingRows.Add(new AscOutputRow()
                {
                    Rpn = "Divide by 0 exception"
                });
                AscendingParseStatusLabel.Background = new SolidColorBrush(Colors.Red);
            }

            Lab5RowsListView.ItemsSource = resultingRows;

            RpnStatusLabel.Content = "Loaded";
            RpnStatusLabel.Background = new SolidColorBrush(Colors.Green);
        }

        private async void IfRpnButton_OnClick(object sender, RoutedEventArgs e)
        {
            RpnListView.ItemsSource = RpnConstructor.Construct(outputTable.OutputRows);
            RpnTabItem.IsSelected = true;
        }
    }
}
