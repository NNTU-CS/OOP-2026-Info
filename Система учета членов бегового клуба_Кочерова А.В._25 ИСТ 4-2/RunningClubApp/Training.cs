using System;

namespace RunningClubApp
{
    /// <summary>
    /// Тренировка бегуна.
    /// </summary>
    public class Training
    {
        private DateTime _beginDt;
        private DateTime _endDt;
        private string _descr;
        private string _dataInfo;
        private TrainingType _tpy;

        public Training(DateTime beginDt, TrainingType tType, string dataInfo, string descr, DateTime endDt)
        {
            if (tType == null) throw new ArgumentNullException(nameof(tType));
            if (endDt < beginDt) throw new ArgumentException("Дата окончания не может быть раньше начала.");

            _beginDt = beginDt;
            _tpy = tType;
            _dataInfo = dataInfo ?? string.Empty;
            _descr = descr ?? string.Empty;
            _endDt = endDt;
        }

        public DateTime BeginDt { get { return _beginDt; } }
        public DateTime EndDt { get { return _endDt; } }
        public string Descr { get { return _descr; } }
        public string DataInfo { get { return _dataInfo; } }
        public TrainingType Tpy { get { return _tpy; } }

        public Training Clone() // Создание независимой копии
        {
            return new Training(_beginDt, _tpy, _dataInfo, _descr, _endDt);
        }

        // Переопределение равенства по значениям полей, а не по адресу в памяти
        public override bool Equals(object obj)
        {
            Training o = obj as Training; // Приведение типов: сущность Training должна быть объектом
            if (o == null) return false;
            return _beginDt == o._beginDt && _endDt == o._endDt
                && _descr == o._descr && _dataInfo == o._dataInfo
                && Equals(_tpy, o._tpy); // Вызов статического метода - защита от возможной "пустоты" объекта TrainingType
        }

        public override int GetHashCode()
        {
            unchecked // Отключение проверки переполнения буфера при выполнении арифметических операций
            {
                int h = _beginDt.GetHashCode();
                h = (h * 397) ^ _endDt.GetHashCode(); // Умножение на простое число перемешивает биты так, что изменения в старших битах влияют на
                                                      // младшие
                h = (h * 397) ^ (_descr != null ? _descr.GetHashCode() : 0);
                h = (h * 397) ^ (_dataInfo != null ? _dataInfo.GetHashCode() : 0);
                h = (h * 397) ^ (_tpy != null ? _tpy.GetHashCode() : 0);
                return h;
            }
        }
    }
}