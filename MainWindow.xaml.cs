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
#endif
            //DataContext = this;
        }

        /// <summary>
        /// Задать путь к директории с ответами
        /// </summary>
        private void PathToAnswers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
#if !DEBUG
            var folderBrowserDialog = new SWF.FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            var DialogResult = folderBrowserDialog.ShowDialog();
            Path = folderBrowserDialog.SelectedPath;
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
                var selectedId = ((sender as ListBox).SelectedItem as Answer).Id;
                var selectedContent = Answers.SingleOrDefault(a => a.Id == selectedId).Content;
                var r = new StringBuilder();
                foreach (var item in selectedContent)
                {
                    char ch = Convert.ToChar(item);
                    if (char.IsControl(ch))
                        r.Append(" ");
                    else 
                        r.Append(item);
                }

                MessageBox.Show($"{r}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        private void LoadAnswers_Click(object sender, RoutedEventArgs e)
        {
            Answers.Clear();

            var subDirectories = Directory.GetDirectories(Path);
            foreach (var dir in subDirectories)
            {
                var dirName = new DirectoryInfo(dir).Name;
                var files = Directory.GetFiles(dir);
                foreach (var file in files)
                {
                    Console.WriteLine(file);
                    var fileInfo = new FileInfo(file);
                    var fileExtention = fileInfo.Extension;
                    if (fileExtention == ".ans")
                    {
                        try
                        {
                            var content = File.ReadAllText(file, Encoding.ASCII);
                            //StreamReader sr = new StreamReader(file, ASCIIEncoding.ASCII);
                            //var content = sr.ReadToEnd();
                            Answers.Add(new Answer(dirName, content));
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    else continue; // нас интересует только текстовый файл
                }
            }

            MessageBox.Show($"Загружено {Answers.Count} записей");

            AnswersList.ItemsSource = Filter(Answers, SearchingValue.Text.Trim());
        }

        private List<Answer> Filter(List<Answer> answers, string search)
        {
            return answers.Where(a => a.Id.Contains(search)).ToList();
        }
    }
}
