using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunningClubApp;
using System.Linq;
using System;

namespace RunningClubTests
{
    [TestClass]
    public class DbTests
    {
        [TestMethod]
        public void SaveRunnerWithoutTraining_Works()
        {
            // Arrange
            var dbContr = new ContrDB();
            var runner = RunnerFactory.Instance.Create("Тест Без Тренировок", DateTime.Today, "Москва", "000");

            // Act
            dbContr.SaveRunner(runner);

            // Assert (проверка появления бегуна в базе)
            using (var db = new RunningClubDBEntities1())
            {
                var saved = db.RunnerTable.FirstOrDefault(r => r.Name == "Тест Без Тренировок");
                Assert.IsNotNull(saved, "Бегун без тренировок не сохранился!");
            }
        }

        [TestMethod]
        public void SaveRunnerWithTraining_Works()
        {
            // Arrange
            var dbContr = new ContrDB();
            var runner = RunnerFactory.Instance.Create("Тест С Тренировкой", DateTime.Today, "Казань", "111");

            // Группа уже создана автоматически! Просто добавляем тренировку.
            var trainingType = new TrainingType("Кросс", "Бег по лесу");
            var training = new Training(DateTime.Today, trainingType, "Инфо", "Описание", DateTime.Today.AddHours(1));
            runner.AddTraining(training);

            // Act
            dbContr.SaveRunner(runner);

            // Assert
            using (var db = new RunningClubDBEntities1())
            {
                var savedRunner = db.RunnerTable.FirstOrDefault(r => r.Name == "Тест С Тренировкой");
                Assert.IsNotNull(savedRunner, "Бегун с тренировкой не сохранился!");

                var savedGroup = db.TrainingGroupTable.FirstOrDefault(g => g.Id == savedRunner.IDGroup);
                Assert.IsNotNull(savedGroup, "Группа для бегуна не сохранилась!");

                var savedTraining = db.TrainingTable.FirstOrDefault(t => t.IDTrainingGroup == savedGroup.Id);
                Assert.IsNotNull(savedTraining, "Тренировка не сохранилась в БД!");
                Assert.AreEqual("Инфо", savedTraining.DataInfo);
            }
        }

        [TestMethod]
        public void SaveRunnerWithCustomFields_Works()
        {
            // Arrange
            var dbContr = new ContrDB();

            // ВОТ ЗДЕСЬ создаем уникальное имя (добавляем текущее время)
            string uniqueName = "Тест Кастомных Полей " + DateTime.Now.Ticks;

            var runner = RunnerFactory.Instance.Create(uniqueName, DateTime.Today, "Пермь", "123");

            // Заполняем кастомные поля
            runner.SetUsrField("ShoeSize", "44.5");
            runner.SetUsrField("MedicalNotes", "Аллергия на пыльцу");
            runner.SetUsrField("PersonalBest", "Марафон за 3:15:00");

            // Act
            dbContr.SaveRunner(runner);

            // Assert (проверяем, что данные выгрузились и загрузились верно)
            var loadedRunners = dbContr.LoadRunners();

            // ВОТ ЗДЕСЬ ищем по уникальному имени
            var savedRunner = loadedRunners.FirstOrDefault(r => r.Name == uniqueName);

            Assert.IsNotNull(savedRunner, "Бегун не загрузился из БД!");

            // Проверяем, что кастомные поля не перепутались
            Assert.AreEqual("44.5", savedRunner.GetUsrField("ShoeSize"), "Размер обуви не сохранился!");
            Assert.AreEqual("Аллергия на пыльцу", savedRunner.GetUsrField("MedicalNotes"), "Мед. заметки не сохранились!");
            Assert.AreEqual("Марафон за 3:15:00", savedRunner.GetUsrField("PersonalBest"), "Личный рекорд не сохранился!");
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var db = new RunningClubDBEntities1())
            {
                // Поиск всех тестовых бегунов (у которых в имени есть слово "Тест")
                var testRunners = db.RunnerTable.Where(r => r.Name.Contains("Тест")).ToList();

                if (testRunners.Any())
                {
                    foreach (var runner in testRunners)
                    {
                        // Поиск тренировочных групп этого бегуна (если он к ним привязан)
                        if (runner.IDGroup.HasValue)
                        {
                            var group = db.TrainingGroupTable.FirstOrDefault(g => g.Id == runner.IDGroup.Value);
                            if (group != null)
                            {
                                // Удаление тренировок, привязанных к группе
                                var trainings = db.TrainingTable.Where(t => t.IDTrainingGroup == group.Id).ToList();
                                db.TrainingTable.RemoveRange(trainings);

                                // Удаление самой группы
                                db.TrainingGroupTable.Remove(group);
                            }
                        }
                    }

                    // Удаление самих бегунов
                    db.RunnerTable.RemoveRange(testRunners);
                    db.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void DeleteRunner_Works()
        {
            // Arrange
            var dbContr = new ContrDB();

            // Создаем бегуна с уникальным именем
            string uniqueName = "Тест На Удаление " + DateTime.Now.Ticks;
            var runner = RunnerFactory.Instance.Create(uniqueName, DateTime.Today, "Уфа", "999");

            // Добавляем ему тренировку
            var trainingType = new TrainingType("Кросс", "Бег по лесу");
            var training = new Training(DateTime.Today, trainingType, "Инфо", "Описание", DateTime.Today.AddHours(1));
            runner.AddTraining(training);

            // Сохраняем в БД
            dbContr.SaveRunner(runner);

            // Проверяем, что он действительно сохранился
            using (var db = new RunningClubDBEntities1())
            {
                var exists = db.RunnerTable.Any(r => r.Name == uniqueName);
                Assert.IsTrue(exists, "Бегун не сохранился перед удалением!");
            }

            // Act
            dbContr.DeleteRunner(uniqueName, "999");

            // Assert
            using (var db = new RunningClubDBEntities1())
            {
                // 1. Проверяем, что бегуна больше нет
                var exists = db.RunnerTable.Any(r => r.Name == uniqueName);
                Assert.IsFalse(exists, "Бегун не удалился из БД!");

                // 2. Проверяем, что группа бегуна ОСТАЛАСЬ в БД (согласно новой логике)
                var savedGroup = db.TrainingGroupTable.FirstOrDefault(g => g.Name == "Общая группа"); // имя группы по умолчанию
                Assert.IsNotNull(savedGroup, "Группа удалилась, а не должна! Бегуны могли в ней остаться.");
            }
        }
    }
}