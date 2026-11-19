using System;
using System.Collections;
using System.Collections.Generic;

namespace RunningClubApp
{
    public class GroupTypeCol : IEnumerable<GroupType>, IDisposable
    {
        private readonly List<GroupType> _lst = new List<GroupType>();

        public int Count { get { return _lst.Count; } }

        public GroupType this[int index] // Индексатор позволяет обращаться к элементу как к массиву
        {
            get { return _lst[index]; }
            set { _lst[index] = value; }
        }

        public void Add(GroupType item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _lst.Add(item);
        }

        /// <summary>
        /// Находит и возвращает тип группы, помеченный как «Основной».
        /// </summary>
        /// <returns>Основной тип группы или null, если такого нет.</returns>
        public GroupType GetMain()
        {
            foreach (var t in _lst)
            {
                if (t.IsMain) return t;
            }
            return null;
        }
        /// <summary>
        /// Возвращение итератора для поочередного чтения элементов коллекции.
        /// </summary>
        /// <returns>Итератор для обхода коллекции в цикле foreach.</returns>
        public IEnumerator<GroupType> GetEnumerator()
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
            _lst.Clear();
        }
    }
}