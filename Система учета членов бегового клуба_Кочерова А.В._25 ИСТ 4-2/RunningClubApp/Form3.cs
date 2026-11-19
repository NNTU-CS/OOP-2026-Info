using System;
using System.Windows.Forms;

namespace RunningClubApp
{
    public partial class Form3 : Form
    {
        public string RunnerName { get; private set; }
        public string RunnerCity { get; private set; }
        public string RunnerPhone { get; private set; }

        public string ShoeSize { get; private set; }
        public string MedicalNotes { get; private set; }
        public string PersonalBest { get; private set; }
        public string TShirtSize { get; private set; }
        public string EmergencyPhone { get; private set; }

        public string GroupName { get; private set; }

        private Runner _editableRunner;
        private TrainingTypeCol _trainingTypes;

        // Конструктор для добавления нового бегуна
        public Form3(TrainingTypeCol trainingTypes)
        {
            InitializeComponent();
            _trainingTypes = trainingTypes;
        }

        // Конструктор для редактирования существующего
        public Form3(Runner runner, TrainingTypeCol trainingTypes) : this(trainingTypes)
        {
            _editableRunner = runner;
            txtName.Text = runner.Name;
            txtCity.Text = runner.City;
            txtPhone.Text = runner.Phone;

            var group = runner.GetMainGroup();
            if (group != null)
            {
                txtGroupName.Text = group.Name;
            }

            txtShoeSize.Text = runner.GetUsrField("ShoeSize");
            txtMedical.Text = runner.GetUsrField("MedicalNotes");
            txtPB.Text = runner.GetUsrField("PersonalBest");
            txtTShirt.Text = runner.GetUsrField("TShirtSize");
            txtEmergency.Text = runner.GetUsrField("EmergencyPhone");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите ФИО!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string phone = txtPhone.Text.Trim();
            if (!IsValidPhone(phone))
            {
                MessageBox.Show(
                    "Некорректный номер телефона.\n" +
                    "Допустимы цифры, пробелы, +, -, скобки.\n" +
                    "Количество цифр — от 10 до 15.\n" +
                    "Пример: +7 (920) 078-56-88",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            string emergency = txtEmergency.Text.Trim();
            if (emergency.Length > 0 && !IsValidPhone(emergency))
            {
                MessageBox.Show(
                    "Некорректный экстренный телефон.\n" +
                    "Оставьте поле пустым или введите номер в правильном формате.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmergency.Focus();
                return;
            }

            RunnerName = txtName.Text.Trim();
            RunnerCity = txtCity.Text.Trim();
            RunnerPhone = phone;
            GroupName = txtGroupName.Text.Trim();

            ShoeSize = txtShoeSize.Text.Trim();
            MedicalNotes = txtMedical.Text.Trim();
            PersonalBest = txtPB.Text.Trim();
            TShirtSize = txtTShirt.Text.Trim();
            EmergencyPhone = emergency;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Проверяет, является ли строка корректным номером телефона.
        /// Допустимы цифры, пробелы, символы +, -, (, ).
        /// Количество цифр — от 10 до 15; знак + — только в начале и один.
        /// </summary>
        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            phone = phone.Trim();

            // 1. Только допустимые символы
            foreach (char c in phone)
            {
                if (!char.IsDigit(c) &&
                    c != '+' && c != '-' && c != ' ' && c != '(' && c != ')')
                    return false;
            }

            // 2. Количество цифр — от 10 до 15
            int digits = 0;
            foreach (char c in phone)
                if (char.IsDigit(c)) digits++;

            if (digits < 10 || digits > 15)
                return false;

            // 3. Знак '+' — только в начале и только один
            int plusIndex = phone.IndexOf('+');
            if (plusIndex > 0) return false;
            if (plusIndex == 0 && phone.IndexOf('+', 1) >= 0) return false;

            return true;
        }

        private void btnManageTrainings_Click(object sender, EventArgs e)
        {
            if (_editableRunner != null)
            {
                using (var f6 = new Form6(_editableRunner, _trainingTypes))
                {
                    f6.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Сначала сохраните бегуна!");
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form3_Load_1(object sender, EventArgs e)
        {

        }
    }
}