using System;
using System.Collections.Generic;
using System.Linq;

namespace RunningClubApp
{
    /// <summary>
    /// Бегун — главный объект учета в системе.
    /// </summary>
    public class Runner
    {
        private string _name;
        private DateTime _dateIn;
        private string _city;
        private string _phone;
        private string _email;

       //Группа, в которой состоит бегун
        public TrainingGroup Group { get; set; }

        private readonly Dictionary<string, string> _usrFields; // Словарь кастомных полей

        // Создание объекта только в рамках текущей сборки и только шаблонно, с использованием фабрики
        internal Runner(string name, DateTime dateIn, string city, string phone, string email, Dictionary<string, string> usrFields)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _dateIn = dateIn;
            _city = city ?? string.Empty;
            _phone = phone ?? string.Empty;
            _email = email ?? string.Empty;
            _usrFields = usrFields ?? throw new ArgumentNullException(nameof(usrFields));
        }

        public void RenameMainGroup(string newName)
        {
            var group = GetMainGroup();
            if (group != null)
            {
                group.Name = newName;
            }
        }

        public string Name { get { return _name; } set { _name = value; } }
        public DateTime DateIn { get { return _dateIn; } set { _dateIn = value; } }
        public string City { get { return _city; } set { _city = value; } }
        public string Phone { get { return _phone; } set { _phone = value; } }
        public string Email { get { return _email; } set { _email = value; } }

        public IReadOnlyDictionary<string, string> UsrFields { get { return _usrFields; } }

        // Поиск группы основного типа
        public TrainingGroup GetMainGroup()
        {
            return Group;
        }

        public void AddTraining(Training training)
        {
            // Если группы нет (бегун только создан), создаётся ему дефолтная общая
            if (Group == null)
            {
                var mainType = new GroupType("Основной тип", true);
                Group = new TrainingGroup("Общая группа", "Главный тренер", "", "", mainType);
            }
            Group.AddTraining(training);
        }

        // Проверка, есть ли у бегуна тренировка заданного типа
        public bool ExistTrainingOfType(TrainingType type)
        {
            return Group != null && Group.Trainings.Any(t => t.Tpy.Equals(type));
        }

        // Проверка, есть ли у бегуна тренировка конкретного типа в заданном периоде
        public bool ExistTrainingOfTypeInPeriod(TrainingType type, DateTime from, DateTime to)
        {
            return Group != null && Group.Trainings.Any(t =>
                t.Tpy.Equals(type) && t.BeginDt >= from && t.EndDt <= to);
        }

        public void SetUsrField(string fieldName, string value)
        {
            if (!_usrFields.ContainsKey(fieldName))
                throw new ArgumentException($"Поле '{fieldName}' не существует.");
            _usrFields[fieldName] = value;
        }

        public string GetUsrField(string fieldName)
        {
            string val;
            return _usrFields.TryGetValue(fieldName, out val) ? val : null;
        }

        /// <summary>
        /// Проверяет, является ли строка корректным номером телефона.
        /// Допустимы цифры, пробелы, символы +, -, (, ).
        /// Количество цифр — от 10 до 15; знак + — только в начале и один.
        /// </summary>
        private bool IsPhoneValid(string phone)
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
    }
}