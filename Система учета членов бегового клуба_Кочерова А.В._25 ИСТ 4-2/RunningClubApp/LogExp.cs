using System;

namespace RunningClubApp
{
    // Перечисление операций 
    public enum LogExpEnum { Contains, EQ, NoContains }

    // Интерфейс выражения 
    public interface ILogExp
    {
        bool Compare(string fieldValue);
    }

    // Реализация: содержание поля
    public class ExpContains : ILogExp
    {
        private string _val;
        public ExpContains(string val) { _val = val; }
        public bool Compare(string fieldValue) => fieldValue != null && fieldValue.IndexOf(_val, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    // Реализация: равенство 
    public class ExpEQ : ILogExp
    {
        private string _val;
        public ExpEQ(string val) { _val = val; }
        public bool Compare(string fieldValue) => fieldValue == _val;
    }

    // Реализация: отсутствие 
    public class ExpNoContains : ILogExp
    {
        private string _val;
        public ExpNoContains(string val) { _val = val; }
        public bool Compare(string fieldValue) => fieldValue == null || fieldValue.IndexOf(_val, StringComparison.OrdinalIgnoreCase) < 0;
    }

    // Фабрика выражений 
    public static class LogExpFactory
    {
        public static ILogExp Create(LogExpEnum type, string value)
        {
            switch (type)
            {
                case LogExpEnum.EQ: return new ExpEQ(value);
                case LogExpEnum.NoContains: return new ExpNoContains(value);
                default: return new ExpContains(value);
            }
        }
    }
}