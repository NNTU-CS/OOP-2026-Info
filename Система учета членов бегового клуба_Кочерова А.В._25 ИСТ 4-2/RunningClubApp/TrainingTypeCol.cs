using System;
using System.Collections;
using System.Collections.Generic;

namespace RunningClubApp
{

    public class TrainingTypeCol : IEnumerable<TrainingType>, IDisposable
    {
        private readonly List<TrainingType> _lst = new List<TrainingType>();

        public int Count { get { return _lst.Count; } }

        public TrainingType this[int index] // Индексатор позволяет обращаться к элементу как к массиву
        {
            get { return _lst[index]; }
            set { _lst[index] = value; }
        }

        public void Add(TrainingType item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _lst.Add(item);
        }

        // Очистка списка
        public void Clear()
        {
            _lst.Clear(); 
        }

        /// <summary>
        /// Возвращение итератора для поочередного чтения элементов коллекции.
        /// </summary>
        /// <returns>Итератор для обхода коллекции в цикле foreach.</returns>
        public IEnumerator<TrainingType> GetEnumerator()
        {
            return _lst.GetEnumerator();
        }

        /// <summary>
        /// Явная реализация необобщенного интерфейса IEnumerable, перенаправляющая запрос к обобщенному методу GetEnumerator().
        /// Дотупен только при приведении коллекции к устаревшему виду.
        /// </summary>
        /// <returns>Необобщенный перечислитель для обхода коллекции.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Очистка ресурсов созданного ранее объекта за их ненадобностью
        public void Dispose()
        {
            Clear();
        }
    }
}