using System;
using System.Collections.Generic;
using System.Linq;

namespace RunningClubApp
{
    /// <summary>
    /// Репозиторий клуба с выборкой по полям/тренировкам и массовыми операциями
    /// </summary>
    /// 
    public class ClubRepository
    {
        private readonly List<Runner> _runners = new List<Runner>();

        public IReadOnlyList<Runner> All { get { return _runners; } } // Интерфейс, разрешающий прочтение приватного списка из храниилща

        public void Add(Runner runner)
        {
            if (runner == null) throw new ArgumentNullException(nameof(runner));
            _runners.Add(runner);
        }

        public void Clear()
        {
            _runners.Clear();
        }
        /// <summary>
        /// Фильтрация по функции-предикату
        /// </summary>
        /// <returns> Список членов клуба, соответствующих заданному параметру </returns>
        public List<Runner> Filter(Func<Runner, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return _runners.Where(predicate).ToList();
        }

        /// <summary>
        /// Выборка по наличию тренировки заданного типа
        /// </summary>
        /// <returns> Список бегунов </returns>
        public List<Runner> FilterByTrainingPresence(TrainingType type, bool mustExist = true)
        {
            return _runners.Where(r => r.ExistTrainingOfType(type) == mustExist).ToList();
        }

        /// <summary>
        /// Выборка по наличию тренировки заданного типа заданном периоде
        /// </summary>
        /// <returns> Список бегунов </returns>
        public List<Runner> FilterByTrainingInPeriod(TrainingType type, DateTime from, DateTime to)
        {
            return _runners.Where(r => r.ExistTrainingOfTypeInPeriod(type, from, to)).ToList();
        }

        /// <summary>
        /// Массовая простановка значения пользовательского поля
        /// </summary>
        public void SetFieldForMany(IEnumerable<Runner> runners, string fieldName, string value)
        {
            foreach (var r in runners)
                r.SetUsrField(fieldName, value);
        }

        /// <summary>
        /// Массовое добавление тренировки (клонируется для каждого члена клуба)
        /// </summary>
        public void AddTrainingForMany(IEnumerable<Runner> runners, Training sample)
        {
            if (sample == null) throw new ArgumentNullException(nameof(sample));
            foreach (var r in runners)
                r.AddTraining(sample.Clone());
        }

        /// <summary>
        /// Удаление члена клуба из хранилища в памяти.
        /// </summary>
        public void Remove(Runner runner)
        {
            if (runner != null)
            {
                _runners.Remove(runner);
            }
        }
    }
}