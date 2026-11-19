using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RunningClubApp
{
    public partial class Form1 : Form
    {
        private MainContr _controller;

        public Form1()
        {
            InitializeComponent();

            // Создаем тестовые данные (для демонстрации)
            var repo = new ClubRepository();
            var factory = RunnerFactory.Instance;
            repo.Add(factory.Create("Иванов Иван", DateTime.Today, city: "Москва", phone: "+79200785688"));
            repo.Add(factory.Create("Тимофеев Тимофей", DateTime.Today, city: "Нижний Новгород", phone: "+79290650575"));
            repo.Add(factory.Create("Петров Петр", DateTime.Today, city: "Казань", phone: "+79300524622"));
            repo.Add(factory.Create("Максимов Максим", DateTime.Today, city: "Нижний Новгород", phone: "+79107804567"));

            // Инициализируем контроллер
            _controller = new MainContr(this, repo);

            // Привязываем события меню (если они есть на форме)
            menuFilter.Click += (s, e) => _controller.OpenFilter();
            menuFields.Click += (s, e) => _controller.ConfigureFields();

            // Рисуем таблицу
            _controller.RefreshView();
        }

        /// <summary>
        /// Метод отрисовки таблицы (вызывается из контроллера).
        /// </summary>
        public void DrawTable(List<Runner> runners, List<RunnerField> fields)
        {
            // Отключаем перерисовку на время обновления, чтобы не было лагов и дублей колонок
            listViewRunners.BeginUpdate();

            try
            {
                // Полностью очищаем старые данные
                listViewRunners.Items.Clear();
                listViewRunners.Columns.Clear();

                // Создаем колонки заново
                foreach (var field in fields)
                {
                    // Задаем нормальную ширину колонки (например, 150 пикселей)
                    listViewRunners.Columns.Add(field.Name, 150);
                }

                // Заполняем строки данными
                foreach (var runner in runners)
                {
                    var item = new ListViewItem();

                    for (int i = 0; i < fields.Count; i++)
                    {
                        string val = fields[i].GetValue(runner);
                        if (i == 0)
                            item.Text = val; // Первая колонка
                        else
                            item.SubItems.Add(val); // Остальные колонки
                    }
                    listViewRunners.Items.Add(item);
                }
            }
            finally
            {
                // Возобновляем отрисовку
                listViewRunners.EndUpdate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            menuFilter.Click += (s, ev) => _controller.OpenFilter(); // Событие вызывается только когда есть event sender и исполнитель
        }

        private void menuAdd_Click(object sender, EventArgs e)
        {
            _controller.AddRunner();
        }

        private void menuSaveDB_Click(object sender, EventArgs e)
        {
            _controller.SaveToDb();
        }

        private void menuLoadDB_Click(object sender, EventArgs e)
        {
            _controller.LoadFromDb();
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void listViewRunners_DoubleClick(object sender, EventArgs e)
        {
            if (listViewRunners.SelectedIndices.Count > 0)
            {
                // Получение индекса выделенной строки
                int selectedIndex = listViewRunners.SelectedIndices[0];

                // Вызов метода контроллера для редактирования
                _controller.EditRunnerByIndex(selectedIndex);
            }
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            // Проверяем, выделил ли пользователь строку в таблице
            if (listViewRunners.SelectedIndices.Count > 0)
            {
                int selectedIndex = listViewRunners.SelectedIndices[0];
                _controller.DeleteRunnerByIndex(selectedIndex);
            }
            else
            {
                MessageBox.Show("Сначала выберите бегуна в таблице!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}