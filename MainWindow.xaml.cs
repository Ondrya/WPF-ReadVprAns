using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections;
using SWF = System.Windows.Forms;
using System.Windows.Media;

namespace ReadVprAns
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string Path;
        public List<Answer> Answers, AnswersFilterd;

        public MainWindow()
        {
            InitializeComponent();
            Answers = new List<Answer>();
            AnswersFilterd = new List<Answer>();
#if DEBUG
            Path = "C:\\Users\\ondry\\Desktop\\ВПР англ 8\\ВПР англ 8";
            SelectedPath.Text = Path;
#endif
            //DataContext = this;
        }

        /// <summary>
        /// Задать путь к директории с ответами
        /// </summary>
        private void PathToAnswers_Click(object sender, RoutedEventArgs e)
        {
#if !DEBUG
            var folderBrowserDialog = new SWF.FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            var DialogResult = folderBrowserDialog.ShowDialog();
            Path = folderBrowserDialog.SelectedPath;
            SelectedPath.Text = Path;
            if (Path != null) PathToAnswers.Background = Brushes.Green;
#endif

            // TODO добавить проверку, найти хотя бы одну поддиректорию с фалом .ans
        }

        /// <summary>
        /// Фильтруем список загруженных работ
        /// </summary>
        private void SearchingValue_KeyUp(object sender, KeyEventArgs e)
        {
            AnswersList.ItemsSource = Filter(Answers, (sender as TextBox).Text.Trim());
        }

        /// <summary>
        /// Вывести данные выбранного элемента
        /// </summary>
        private void AnswersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedAnswer = ((sender as ListBox).SelectedItem as Answer);

                if (selectedAnswer == null) return;

                AnswerInfoGroupBox.Header = $"Результаты работы {selectedAnswer.Id}";
                SelectedAnswer.Text = selectedAnswer.RealAnswers[0];
                SelectedAnswerTask1.Text = selectedAnswer.RealAnswers[1];
                SelectedAnswerTask2.Text = selectedAnswer.RealAnswers[2];
                SelectedAnswerTask3.Text = selectedAnswer.RealAnswers[3];
                SelectedAnswerTask4.Text = selectedAnswer.RealAnswers[4];
                SelectedAnswerTask5.Text = selectedAnswer.RealAnswers[5];
                SelectedAnswerTask6.Text = selectedAnswer.RealAnswers[6];


            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
            }
            
        }

        /// <summary>
        /// Очистка строки от непечатных символов, остаются только ответы
        /// </summary>
        /// <param name="ans"></param>
        /// <returns></returns>
        private static string RemoveIsControlChar(string ans)
        {
            return new string(ans.Where(c => !char.IsControl(c)).ToArray());
        }

        /// <summary>
        /// Загрузить данные из выбранной папки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadAnswers_Click(object sender, RoutedEventArgs e)
        {
            if (Path == null)
            {
                MessageBox.Show($"Не выбран каталог с ответами");
                return;
            }
            
            Answers.Clear();

            var subDirectories = Directory.GetDirectories(Path);
            foreach (var dir in subDirectories)
            {
                var dirName = new DirectoryInfo(dir).Name;
                var files = Directory.GetFiles(dir);
                foreach (var file in files)
                {
                    //Console.WriteLine(file);
                    var fileInfo = new FileInfo(file);
                    var fileExtention = fileInfo.Extension;
                    if (fileExtention == ".ans")
                    {
                        try
                        {
                            //var content = File.ReadAllText(file, Encoding.ASCII);
                            var content = File.ReadAllText(file, Encoding.UTF8);

                            var splitter = '\u0001'; //SOH
                            var _content = content.Split(splitter);
                            var realAnswersVariant = _content[_content.Length - 1]; // последняя строка содержит вариант работы
                            var _realAnswers = _content[_content.Length - 2]; // предпоследняя строка содержит ответы на вопросы (текстовые)
                            var realAnswers = _realAnswers.Split('\b'); // каждый ответ начинается с кода BS

                            var realAnRealAnswers = new string[7];

                            realAnRealAnswers[0] = RemoveIsControlChar(realAnswersVariant);
                            realAnRealAnswers[1] = RemoveIsControlChar(realAnswers[1]);
                            realAnRealAnswers[2] = RemoveIsControlChar(realAnswers[2]);
                            realAnRealAnswers[3] = RemoveIsControlChar(realAnswers[3]);
                            realAnRealAnswers[4] = RemoveIsControlChar(realAnswers[4]);
                            realAnRealAnswers[5] = RemoveIsControlChar(realAnswers[5]);
                            realAnRealAnswers[6] = RemoveIsControlChar(realAnswers[6]);

                            //RemoveIsControlChar(realAnswers[1]);

                            Answers.Add(new Answer(dirName, content, realAnRealAnswers));
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    else continue; // нас интересует только текстовый файл
                }
            }

            LastAction.Text = $"Загружено {Answers.Count} записей";

            AnswersList.ItemsSource = Filter(Answers, SearchingValue.Text.Trim());
        }

        private List<Answer> Filter(List<Answer> answers, string search)
        {
            return answers.Where(a => a.Id.Contains(search)).ToList();
        }
    }
}
