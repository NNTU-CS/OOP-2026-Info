using System;

namespace RunningClubApp
{
    /// <summary>
    /// Базовый класс для поля отображения в таблице.
    /// </summary>
    public abstract class RunnerField
    {
        public string Name { get; protected set; }

        /// <summary>
        /// Возвращение строкового значения поля для конкретного бегуна.
        /// </summary>
        public abstract string GetValue(Runner runner);

        /// <summary>
        /// Создание независимой копии поля.
        /// </summary>
        public abstract RunnerField Clone();

        public override string ToString() => Name;
    }

    //Наследники для конкретных полей

    public class NameField : RunnerField
    {
        public NameField() { Name = "ФИО"; }
        public override string GetValue(Runner runner) => runner.Name;
        public override RunnerField Clone() => new NameField();
    }

    public class CityField : RunnerField
    {
        public CityField() { Name = "Город"; }
        public override string GetValue(Runner runner) => runner.City;
        public override RunnerField Clone() => new CityField();
    }

    public class PhoneField : RunnerField
    {
        public PhoneField() { Name = "Телефон"; }
        public override string GetValue(Runner runner) => runner.Phone;
        public override RunnerField Clone() => new PhoneField();
    }

    public class GroupField : RunnerField
    {
        public GroupField() { Name = "Группа"; }
        public override string GetValue(Runner runner) => runner.GetMainGroup()?.Name;
        public override RunnerField Clone() => new GroupField();
    }

    public class ShoeSizeField : RunnerField
    {
        public ShoeSizeField() { Name = "Размер обуви"; }
        public override string GetValue(Runner runner) => runner.GetUsrField("ShoeSize") ?? "";
        public override RunnerField Clone() => new ShoeSizeField();
    }

    public class MedicalNotesField : RunnerField
    {
        public MedicalNotesField() { Name = "Мед. заметки"; }
        public override string GetValue(Runner runner) => runner.GetUsrField("MedicalNotes") ?? "";
        public override RunnerField Clone() => new MedicalNotesField();
    }

    public class PersonalBestField : RunnerField
    {
        public PersonalBestField() { Name = "Личный рекорд"; }
        public override string GetValue(Runner runner) => runner.GetUsrField("PersonalBest") ?? "";
        public override RunnerField Clone() => new PersonalBestField();
    }

    public class TShirtSizeField : RunnerField
    {
        public TShirtSizeField() { Name = "Размер футболки"; }
        public override string GetValue(Runner runner) => runner.GetUsrField("TShirtSize") ?? "";
        public override RunnerField Clone() => new TShirtSizeField();
    }

    public class EmergencyPhoneField : RunnerField
    {
        public EmergencyPhoneField() { Name = "Экстренный тел."; }
        public override string GetValue(Runner runner) => runner.GetUsrField("EmergencyPhone") ?? "";
        public override RunnerField Clone() => new EmergencyPhoneField();
    }
}