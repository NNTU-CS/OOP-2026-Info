using System;
using System.Collections.Generic;

namespace RunningClubApp
{
    /// <summary>
    /// Тренировочная группа, в которой состоит бегун.
    /// </summary>
    public class TrainingGroup
    {
        private string _name;
        private string _coachName;
        private string _tel;
        private string _email;
        private GroupType _tpy;
        private readonly List<Training> _trainings = new List<Training>();

        public TrainingGroup(string name, string coachName, string tel, string email, GroupType tpy)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (tpy == null) throw new ArgumentNullException(nameof(tpy));

            _name = name;
            _coachName = coachName ?? string.Empty;
            _tel = tel ?? string.Empty;
            _email = email ?? string.Empty;
            _tpy = tpy;
        }

        public string Name { get { return _name; } set { _name = value; } }
        public string CoachName { get { return _coachName; } set { _coachName = value; } }
        public string Tel { get { return _tel; } set { _tel = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public GroupType Tpy { get { return _tpy; } set { _tpy = value; } }

        public IReadOnlyList<Training> Trainings { get { return _trainings; } }

        public void AddTraining(Training training) 
        {
            if (training == null) throw new ArgumentNullException(nameof(training));
            _trainings.Add(training.Clone());  // Клонирование защищает от совпадения ссылок
        }

        public bool ExistTraining(Training training)
        {
            if (training == null) return false;
            foreach (var t in _trainings)
                if (t.Equals(training)) return true;
            return false;
        }


        /// <summary>
        /// Проверяет, есть ли в данной тренировочной группе хотя бы одна тренировка заданного типа.
        /// </summary>
        /// <param name="type">Тип тренировки (например, "Интервалы").</param>
        /// <returns>true, если в группе есть такая тренировка, иначе false.</returns>
        public bool HasTrainingOfType(TrainingType type)
        {
            if (type == null) return false;

            foreach (var t in _trainings)
            {
                if (t.Tpy.Equals(type))
                    return true;
            }
            return false;
        }
    }
}