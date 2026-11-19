using System.Linq;

namespace RunningClubApp
{
    /// <summary>
    /// Базовый класс для хранения всех правил фильтрации.
    /// </summary>
    public abstract class FilterRule
    {
        public string FieldName { get; protected set; }
        public abstract bool IsMatch(Runner runner);
    }

    /// <summary>
    /// Правило: сравнение значения поля (RunnerField) через логическое выражение (ILogExp).
    /// </summary>
    public class FieldFilterRule : FilterRule
    {
        private RunnerField _field;
        private ILogExp _expression;

        public FieldFilterRule(RunnerField field, ILogExp expression)
        {
            FieldName = field.Name;
            _field = field;
            _expression = expression;
        }

        public override bool IsMatch(Runner runner)
        {
            string val = _field.GetValue(runner);
            return _expression.Compare(val);
        }
    }

    /// <summary>
    /// Правило: у бегуна есть хотя бы одна любая тренировка.
    /// </summary>
    public class AnyTrainingRule : FilterRule
    {
        public AnyTrainingRule()
        {
            FieldName = "Любая тренировка";
        }

        public override bool IsMatch(Runner runner)
        {
            var mainGroup = runner.GetMainGroup();
            return mainGroup != null && mainGroup.Trainings.Count > 0;
        }
    }

    /// <summary>
    /// Правило: у бегуна есть тренировка конкретного типа.
    /// </summary>
    public class TrainingPresenceRule : FilterRule
    {
        private TrainingType _type;
        private bool _mustExist;

        public TrainingPresenceRule(TrainingType type, bool mustExist)
        {
            FieldName = "Тренировка: " + type.Name;
            _type = type;
            _mustExist = mustExist;
        }

        public override bool IsMatch(Runner runner)
        {
            var mainGroup = runner.GetMainGroup();
            if (mainGroup == null) return !_mustExist;

            bool hasTraining = mainGroup.Trainings.Any(t => t.Tpy.Equals(_type));
            return hasTraining == _mustExist;
        }
    }
}