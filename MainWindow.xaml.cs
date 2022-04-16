using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileInteractor fileInteractor = new FileInteractor("thrlist.xlsx", "db.txt");
        List<TableItem> tableItems;
        List<Threat> threats;
        List<Threat>[] threatsSubList;
        int page = 1;
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists("thrlist.xlsx"))
            {
                WebClient wc = new WebClient();
                wc.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", "thrlist.xlsx");
                wc.Dispose();
            }
            if (!File.Exists("db.txt"))
            {
                MessageBox.Show("Файла с локальной базой не существует! Приложение создаст новый файл на основе актуальных данных.");
                fileInteractor.CreateDBFile();
            }
            pageShower.Content = page;
            leftButton.IsEnabled = false;
        }

        private void mainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            tableItems = new List<TableItem>();
            threats = fileInteractor.DeserializeDBFile();
            threatsSubList = new List<Threat>[(int)Math.Round((double)threats.Count / 15.0)];
            int i = 0;
            foreach (var item in threats)
            {
                if (threatsSubList[i] is null)
                {
                    threatsSubList[i] = new List<Threat>();
                }
                if (threatsSubList[i].Count == 15)
                {
                    i++;
                }
                if (threatsSubList[i] is null)
                {
                    threatsSubList[i] = new List<Threat>();
                }
                item.ID = "УБИ." + item.ID;
                threatsSubList[i].Add(item);
            }
            foreach (var item in threatsSubList[0])
            {
                tableItems.Add(item);
            }
            mainGrid.ItemsSource = tableItems;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, string> updateResult = fileInteractor.UpdateDBFile();
                StringBuilder message = new StringBuilder();
                message.Append($"Общее количество обновлённых записей: {updateResult.Count}. \n");
                foreach (var item in updateResult)
                {
                    message.Append($"ID измененной записи: {item.Key} - {item.Value} \n");
                }
                MessageBox.Show(message.ToString(), "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                if (updateResult.Count > 0)
                {
                    tableItems.Clear();
                    threats = fileInteractor.DeserializeDBFile();
                    threatsSubList = new List<Threat>[(int)Math.Round((double)threats.Count / 15.0)];
                    int i = 0;
                    foreach (var item in threats)
                    {
                        if (threatsSubList[i] is null)
                        {
                            threatsSubList[i] = new List<Threat>();
                        }
                        if (threatsSubList[i].Count == 15)
                        {
                            i++;
                        }
                        if (threatsSubList[i] is null)
                        {
                            threatsSubList[i] = new List<Threat>();
                        }
                        item.ID = "УБИ." + item.ID;
                        threatsSubList[i].Add(item);
                    }
                    foreach (var item in threatsSubList[page - 1])
                    {
                        tableItems.Add(item);
                    }
                    mainGrid.Items.Refresh();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void mainGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (mainGrid.SelectedItem != null)
            {
                Threat selectedItem = (Threat)mainGrid.SelectedItem;
                StringBuilder message = new StringBuilder();
                message.Append("Идентификатор угрозы: " + selectedItem.ID + ".\n\n");
                message.Append("Наименование угрозы: " + selectedItem.Name + ".\n\n");
                message.Append("Описание угрозы: " + selectedItem.Description + ".\n\n");
                message.Append("Источник угрозы: " + selectedItem.Origin + ".\n\n");
                message.Append("Нарушение конфиденциальности: " + selectedItem.ConfidenceThreat + ".\n\n");
                message.Append("Нарушение целостности: " + selectedItem.IntegrityThreat + ".\n\n");
                message.Append("Нарушение доступности: " + selectedItem.AccessibilityThreat + ".");
                MessageBox.Show(message.ToString());
            }
        }
        private void updateList()
        {
            tableItems.Clear();
            foreach (var item in threatsSubList[page-1])
            {
                tableItems.Add(item);
            }
            mainGrid.Items.Refresh();
        }

        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            page--;
            if (page == 1)
            {
                leftButton.IsEnabled = false;
            }
            if (!rightButton.IsEnabled)
            {
                rightButton.IsEnabled = true;
            }
            pageShower.Content = page;
            updateList();
        }

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            page++;
            if (page == threatsSubList.Length)
            {
                rightButton.IsEnabled = false;
            }
            if (!leftButton.IsEnabled)
            {
                leftButton.IsEnabled = true;
            }
            pageShower.Content = page;
            updateList();
        }
    }
}
