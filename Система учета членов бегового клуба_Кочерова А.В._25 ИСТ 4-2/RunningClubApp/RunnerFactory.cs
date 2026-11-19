using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace RunningClubApp
{
    public class RunnerFactory
    {
        private static RunnerFactory _instance;
        private static readonly object _lock = new object();

        private readonly Dictionary<string, string> _flds;

        private RunnerFactory(IEnumerable<string> fieldNames) // Приватность конструктора обеспечивает защиту от создания объектов некорректной
                                                              // структуры при помощи стандартной операции new
        {
            _flds = new Dictionary<string, string>();
            foreach (var f in fieldNames)
                _flds[f] = string.Empty; // Значения по умолчанию для кастомных полей
        }

        /// <summary>
        /// Создание единственного экземпляра фабрики 
        /// </summary>
        /// <returns> Ссылку на единственный объект </returns>
        public static RunnerFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            // Получение списка полей
                            var customFields = System.Configuration.ConfigurationManager.AppSettings["CustomFields"]?.Split(',')
                                               ?? new[] { "ShoeSize", "MedicalNotes", "PersonalBest", "TShirtSize", "EmergencyPhone" };

                            // Передача списка полей в конструктор
                            _instance = new RunnerFactory(customFields);
                        }
                    }
                }
                return _instance;
            }
        }

        public IReadOnlyList<string> fldNames()
        {
            return _flds.Keys.ToList();
        }

        public Runner Create(string name, DateTime dateIn, string city = "", string phone = "", string email = "")
        {
            var usr = new Dictionary<string, string>(_flds);
            return new Runner(name, dateIn, city, phone, email, usr);
        }
    }
}