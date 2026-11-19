using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RunningClubApp;

namespace RunningClubTests
{
    /// <summary>
    /// Модульные тесты для проверки работоспособности классов модели 
    /// предметной области (Runner, TrainingGroup, Training, RunnerFactory).
    /// </summary>
    [TestClass]
    public class RunnerModelTests
    {
        private TrainingType _intervalType;
        private TrainingType _longRunType;

        /// <summary>
        /// Инициализация общих тестовых данных перед запуском каждого теста.
        /// Создаёт справочные типы тренировок (Интервалы, Длинная).
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _intervalType = new TrainingType("Интервалы", "Работа на скорость");
            _longRunType = new TrainingType("Длинная", "Кросс 20+ км");
        }

        /// <summary>
        /// Проверяет, что фабрика создаёт бегуна, и группа появляется только после добавления тренировки.
        /// </summary>
        [TestMethod]
        public void Factory_CreatesRunnerWithMainGroup()
        {
            var factory = RunnerFactory.Instance;
            var runner = factory.Create("Иванов Иван", DateTime.Today);

            // Группа не должна создаваться сразу при создании бегуна
            Assert.IsNull(runner.GetMainGroup(), "У нового бегуна не должно быть группы по умолчанию.");

            // Создаем тренировку, которая вызовет создание группы
            var t = new Training(DateTime.Today, _intervalType, "10x400м", "Хорошо отработал", DateTime.Today.AddHours(1));
            runner.AddTraining(t);

            Assert.IsNotNull(runner.GetMainGroup(), "После добавления тренировки группа должна создаться автоматически.");
            Assert.IsTrue(runner.GetMainGroup().Tpy.IsMain, "Тип группы должен быть помечен как основной.");
            Assert.AreEqual(5, runner.UsrFields.Count, "У бегуна должно быть ровно 5 пользовательских полей.");
        }

        /// <summary>
        /// Проверяет, что при добавлении тренировки бегуну она клонируется. 
        /// Изменение исходного объекта-образца не должно влиять на сохранённую тренировку.
        /// </summary>
        [TestMethod]
        public void AddTraining_ClonesTraining()
        {
            var runner = RunnerFactory.Instance.Create("Петров Петр", DateTime.Today);
            var t = new Training(DateTime.Today, _intervalType, "10x400м", "Хорошо отработал", DateTime.Today.AddHours(1));

            runner.AddTraining(t);

            Assert.AreEqual(1, runner.GetMainGroup().Trainings.Count, "Тренировка должна быть добавлена в основную группу.");
            Assert.AreNotSame(t, runner.GetMainGroup().Trainings[0], "Сохранённая тренировка должна быть клоном, а не той же ссылкой.");
        }

        /// <summary>
        /// Проверяет выборку бегунов по одному из полей (Город). 
        /// Метод Filter должен возвращать только тех бегунов, которые удовлетворяют условию.
        /// </summary>
        [TestMethod]
        public void FilterByCity_ReturnsOnlyMatching()
        {
            var repo = new ClubRepository();
            repo.Add(RunnerFactory.Instance.Create("Бегун 1", DateTime.Today, city: "Москва"));
            repo.Add(RunnerFactory.Instance.Create("Бегун 2", DateTime.Today, city: "Казань"));
            repo.Add(RunnerFactory.Instance.Create("Бегун 3", DateTime.Today, city: "Нижний Новгород"));
            repo.Add(RunnerFactory.Instance.Create("Бегун 4", DateTime.Today, city: "Нижний Новгород"));

            var result = repo.Filter(r => r.City == "Нижний Новгород");
            Assert.AreEqual(2, result.Count, "Должно быть найдено 2 бегуна из Нижнего Новгорода.");
        }

        /// <summary>
        /// Проверяет выборку бегунов по наличию или отсутствию тренировки заданного типа.
        /// </summary>
        [TestMethod]
        public void FilterByTrainingPresence_Works()
        {
            var repo = new ClubRepository();
            var r1 = RunnerFactory.Instance.Create("Бегун 1", DateTime.Today);
            var r2 = RunnerFactory.Instance.Create("Бегун 2", DateTime.Today);

            r1.AddTraining(new Training(DateTime.Today, _intervalType, "i", "d", DateTime.Today.AddHours(1)));
            repo.Add(r1);
            repo.Add(r2);

            var withInterval = repo.FilterByTrainingPresence(_intervalType, true);
            var withoutInterval = repo.FilterByTrainingPresence(_intervalType, false);

            Assert.AreEqual(1, withInterval.Count, "Должен быть найден 1 бегун с тренировкой 'Интервалы'.");
            Assert.AreEqual(1, withoutInterval.Count, "Должен быть найден 1 бегун без тренировки 'Интервалы'.");
            Assert.AreEqual("Бегун 1", withInterval[0].Name, "Бегун с тренировкой должен называться 'Бегун 1'.");
            Assert.AreEqual("Бегун 2", withoutInterval[0].Name, "Бегун без тренировки должен называться 'Бегун 2'.");
        }

        /// <summary>
        /// Проверяет массовое добавление тренировки-образца множеству бегунов.
        /// Убеждается, что каждому бегуну достаётся независимая копия (клон) тренировки.
        /// </summary>
        [TestMethod]
        public void AddTrainingForMany_ClonesPerRunner()
        {
            var repo = new ClubRepository();
            var r1 = RunnerFactory.Instance.Create("Бегун 1", DateTime.Today);
            var r2 = RunnerFactory.Instance.Create("Бегун 2", DateTime.Today);
            repo.Add(r1); repo.Add(r2);

            var sample = new Training(DateTime.Today, _longRunType, "i", "d", DateTime.Today.AddHours(2));
            repo.AddTrainingForMany(new[] { r1, r2 }, sample);

            Assert.AreEqual(1, r1.GetMainGroup().Trainings.Count, "У первого бегуна должна быть 1 тренировка.");
            Assert.AreEqual(1, r2.GetMainGroup().Trainings.Count, "У второго бегуна должна быть 1 тренировка.");
            Assert.AreNotSame(r1.GetMainGroup().Trainings[0], r2.GetMainGroup().Trainings[0], "У бегунов должны быть разные экземпляры тренировок.");
        }

        /// <summary>
        /// Проверяет массовую простановку значения пользовательского поля 
        /// (например, размера обуви) для набора бегунов, попавших в выборку.
        /// </summary>
        [TestMethod]
        public void SetFieldForMany_ChangesValue()
        {
            var repo = new ClubRepository();
            var r1 = RunnerFactory.Instance.Create("Бегун 1", DateTime.Today);
            var r2 = RunnerFactory.Instance.Create("Бегун 2", DateTime.Today);
            repo.Add(r1); repo.Add(r2);

            repo.SetFieldForMany(new[] { r1, r2 }, "ShoeSize", "42");

            Assert.AreEqual("42", r1.GetUsrField("ShoeSize"), "Поле ShoeSize у первого бегуна должно измениться на '42'.");
            Assert.AreEqual("42", r2.GetUsrField("ShoeSize"), "Поле ShoeSize у второго бегуна должно измениться на '42'.");
        }

        /// <summary>
        /// Проверяет выборку бегунов по наличию тренировки заданного типа в указанном периоде.
        /// Тест создаёт двух бегунов с тренировками в разные годы (2024 и 2025) 
        /// и проверяет, что фильтр по периоду 2024 года возвращает только первого бегуна.
        /// </summary>
        [TestMethod]
        public void FilterByTrainingInPeriod_Works()
        {
            var repo = new ClubRepository();
            var r1 = RunnerFactory.Instance.Create("Бегун 1", DateTime.Today);
            var r2 = RunnerFactory.Instance.Create("Бегун 2", DateTime.Today);

            r1.AddTraining(new Training(new DateTime(2024, 3, 1), _intervalType, "i", "d", new DateTime(2024, 3, 1).AddHours(1)));
            r2.AddTraining(new Training(new DateTime(2025, 3, 1), _intervalType, "i", "d", new DateTime(2025, 3, 1).AddHours(1)));

            repo.Add(r1); repo.Add(r2);

            var result = repo.FilterByTrainingInPeriod(_intervalType, new DateTime(2024, 1, 1), new DateTime(2024, 12, 31));

            Assert.AreEqual(1, result.Count, "Должен быть найден ровно 1 бегун с тренировкой в 2024 году.");
            Assert.AreEqual("Бегун 1", result[0].Name, "Найденный бегун должен называться 'Бегун 1'.");
        }
    }
}