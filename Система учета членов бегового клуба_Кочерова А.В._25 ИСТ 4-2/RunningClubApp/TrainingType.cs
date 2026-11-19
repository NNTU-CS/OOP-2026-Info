using System;

namespace RunningClubApp
{
    public class TrainingType
    {
        private string _name;
        private string _note;

        public TrainingType(string name, string note)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            _name = name;
            _note = note ?? string.Empty;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _name = value;
            }
        }

        public string Note
        {
            get { return _note; }
            set { _note = value ?? string.Empty; }
        }

        public override string ToString() { return _name; }

        /// <summary>
        /// Определяет, равен ли текущий тип тренировки другому объекту.
        /// Равенство определяется совпадением имени типа.
        /// </summary>
        /// <param name="obj">Объект для сравнения с текущим типом тренировки.</param>
        /// <returns>true, если obj является TrainingType с тем же именем; иначе false.</returns>
        public override bool Equals(object obj)
        {
            TrainingType other = obj as TrainingType;
            if (other == null) return false;
            return _name == other._name;
        }

        public override int GetHashCode()
        {
            return _name == null ? 0 : _name.GetHashCode();
        }
    }
}