using System;
using System.Collections.Generic;
using System.Linq;
using RunningClubApp;

namespace RunningClubApp
{
    public class ContrDB
    {
        /// <summary>
        /// Класс-переводчик бизнес-логики на язык БД
        /// </summary>
        /// <param name="runner"></param>
        public void SaveRunner(Runner runner)
        {
            // Получение данных о члене клуба
            using (var db = new RunningClubDBEntities1()) // Ресурсы, выделенные под объект БД, освободятся по завершении всех операций метода
            {
                var mainGroup = runner.GetMainGroup();
                TrainingGroupTable dbGroup = null;

                // Поиск или создание группы в справочнике (без привязки к бегуну)
                if (mainGroup != null)
                {
                    dbGroup = db.TrainingGroupTable.FirstOrDefault(g => g.Name == mainGroup.Name);
                    if (dbGroup == null)
                    {
                        // Требуется ID типа группы. Для простоты создадим или найдем "Основной"
                        var groupType = db.GroupTypeTable.FirstOrDefault(t => t.IsMain);
                        if (groupType == null)
                        {
                            groupType = new GroupTypeTable { Name = "Основной тип", IsMain = true };
                            db.GroupTypeTable.Add(groupType);
                            db.SaveChanges();
                        }

                        dbGroup = new TrainingGroupTable
                        {
                            IDGroupType = groupType.Id,
                            Name = mainGroup.Name,
                            CoachName = mainGroup.CoachName
                        };
                        db.TrainingGroupTable.Add(dbGroup);
                        db.SaveChanges();
                    }

                    // Сохранение тренировок для этой группы
                    foreach (var training in mainGroup.Trainings)
                    {
                        // Поиск или создание типа тренировки
                        var trainingType = db.TrainingTypeTable.FirstOrDefault(t => t.Name == training.Tpy.Name);
                        if (trainingType == null)
                        {
                            trainingType = new TrainingTypeTable { Name = training.Tpy.Name, Note = training.Tpy.Note };
                            db.TrainingTypeTable.Add(trainingType);
                            db.SaveChanges();
                        }

                        // Проверка существования такой же тренировки
                        var existingTraining = db.TrainingTable.FirstOrDefault(t =>
                            t.IDTrainingGroup == dbGroup.Id && t.BeginDt == training.BeginDt);

                        if (existingTraining == null)
                        {
                            db.TrainingTable.Add(new TrainingTable
                            {
                                IDTrainingGroup = dbGroup.Id,
                                IDTrainingType = trainingType.Id,
                                BeginDt = training.BeginDt,
                                EndDt = training.EndDt,
                                DataInfo = training.DataInfo,
                                Descr = training.Descr
                            });
                        }
                    }
                    db.SaveChanges();
                }

                // Сохранение самого бегуна
                var existingRunner = db.RunnerTable.FirstOrDefault(r => r.Name == runner.Name && r.Phone == runner.Phone);

                RunnerTable dbRunner;
                if (existingRunner != null)
                {
                    dbRunner = existingRunner;
                    dbRunner.City = runner.City;
                    dbRunner.DateIn = runner.DateIn;
                    dbRunner.Field1 = runner.GetUsrField("ShoeSize");
                    dbRunner.Field2 = runner.GetUsrField("MedicalNotes");
                    dbRunner.Field3 = runner.GetUsrField("PersonalBest");
                    dbRunner.Field4 = runner.GetUsrField("TShirtSize");
                    dbRunner.Field5 = runner.GetUsrField("EmergencyPhone");
                }
                else
                {
                    dbRunner = new RunnerTable
                    {
                        Name = runner.Name,
                        City = runner.City,
                        Phone = runner.Phone,
                        DateIn = runner.DateIn,
                        Field1 = runner.GetUsrField("ShoeSize"),
                        Field2 = runner.GetUsrField("MedicalNotes"),
                        Field3 = runner.GetUsrField("PersonalBest"),
                        Field4 = runner.GetUsrField("TShirtSize"),
                        Field5 = runner.GetUsrField("EmergencyPhone")
                    };
                    db.RunnerTable.Add(dbRunner);
                }

                // Привязка бегуна к группе
                if (dbGroup != null) dbRunner.IDGroup = dbGroup.Id;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Загрузка всех бегунов из БД в бизнес-объекты.
        /// </summary>
        public List<Runner> LoadRunners()
        {
            var result = new List<Runner>();

            using (var db = new RunningClubDBEntities1())
            {
                var dbRunners = db.RunnerTable.ToList();

                foreach (var dbR in dbRunners)
                {
                    // Восстанавление бегуна через фабрику
                    var factory = RunnerFactory.Instance;
                    var runner = factory.Create(dbR.Name, dbR.DateIn, dbR.City, dbR.Phone, "");

                    // Восстанавливление кастомных полей
                    runner.SetUsrField("ShoeSize", dbR.Field1 ?? "");
                    runner.SetUsrField("MedicalNotes", dbR.Field2 ?? "");
                    runner.SetUsrField("PersonalBest", dbR.Field3 ?? "");
                    runner.SetUsrField("TShirtSize", dbR.Field4 ?? "");
                    runner.SetUsrField("EmergencyPhone", dbR.Field5 ?? "");

                    // Восстанавление группы и тренировки
                    if (dbR.TrainingGroupTable != null)
                    {
                        var gType = new GroupType(dbR.TrainingGroupTable.GroupTypeTable?.Name ?? "Основной тип", true);
                        var group = new TrainingGroup(dbR.TrainingGroupTable.Name, dbR.TrainingGroupTable.CoachName, "", "", gType);

                        foreach (var dbT in dbR.TrainingGroupTable.TrainingTable)
                        {
                            var tType = new TrainingType(dbT.TrainingTypeTable.Name, "");
                            group.AddTraining(new Training(dbT.BeginDt, tType, dbT.DataInfo, dbT.Descr, dbT.EndDt));
                        }
                        runner.Group = group;
                    }

                    result.Add(runner);
                }
            }
            return result;
        }

        /// <summary>
        /// Удаление бегуна из БД.
        /// </summary>
        public void DeleteRunner(string name, string phone)
        {
            using (var db = new RunningClubDBEntities1())
            {
                // Поиск бегуна в БД по имени и телефону
                var dbRunner = db.RunnerTable.FirstOrDefault(r => r.Name == name && r.Phone == phone);
                if (dbRunner == null) return; // Если его нет в БД, ничего не делаем

                // Удаляем только самого бегуна, группа и тренировки остаются
                db.RunnerTable.Remove(dbRunner);
                db.SaveChanges();
            }
        }
    }
}