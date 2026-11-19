using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RunningClubApp
{
    /// <summary>
    /// Класс-посредник между формой и бизнес-логикой приложения
    /// </summary>
    public class MainContr
    {
        private Form1 _mainForm;
        private ClubRepository _repository;
        private RunnerMngr _mngr;

        private ContrDB _dbController = new ContrDB();
        private TrainingTypeCol _trainingTypes = new TrainingTypeCol();

        public MainContr(Form1 form, ClubRepository repo)
        {
            _mainForm = form;
            _repository = repo;
            _mngr = new RunnerMngr(_repository.All.ToList());

            // Заполнение справочника
            _trainingTypes.Add(new TrainingType("Интервалы", "Темповая тренировка"));
            _trainingTypes.Add(new TrainingType("Кросс", "Тренировка Vdot"));
            _trainingTypes.Add(new TrainingType("LSD", "Восстановительный бег"));
            _trainingTypes.Add(new TrainingType("Трейл", "Бег по пересечённой местности"));
            _trainingTypes.Add(new TrainingType("СБУ", "ОФП бегуна"));
            _trainingTypes.Add(new TrainingType("Силовая", "С доп.отягощением"));
        }

        public void RefreshView()
        {
            _mainForm.DrawTable(_mngr.Runners, _mngr.DisplayFields);
        }

        public void ConfigureFields()
        {
            using (var fieldsForm = new Form4(_mngr.DisplayFields))
            {
                if (fieldsForm.ShowDialog() == DialogResult.OK)
                {
                    _mngr.SetFields(fieldsForm.SelectedFields);
                    RefreshView();
                }
            }
        }

        public void AddRunner()
        {
            // Передача справочника
            using (var addForm = new Form3(_trainingTypes))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    var factory = RunnerFactory.Instance;
                    var newRunner = factory.Create(
                        addForm.RunnerName,
                        DateTime.Today,
                        city: addForm.RunnerCity,
                        phone: addForm.RunnerPhone
                    );

                    // Создание группы на основе введенного названия
                    string groupName = string.IsNullOrWhiteSpace(addForm.GroupName) ? "Общая группа" : addForm.GroupName;
                    var groupType = new GroupType("Основной тип", true);
                    newRunner.Group = new TrainingGroup(groupName, "Тренер", "", "", groupType);

                    newRunner.SetUsrField("ShoeSize", addForm.ShoeSize);
                    newRunner.SetUsrField("MedicalNotes", addForm.MedicalNotes);
                    newRunner.SetUsrField("PersonalBest", addForm.PersonalBest);
                    newRunner.SetUsrField("TShirtSize", addForm.TShirtSize);
                    newRunner.SetUsrField("EmergencyPhone", addForm.EmergencyPhone);

                    _repository.Add(newRunner);
                    _mngr.SetRunners(_repository.All.ToList());
                    RefreshView();
                }
            }
        }

        public void EditRunnerByIndex(int index)
        {
            if (index >= 0 && index < _mngr.Runners.Count)
            {
                var runnerToEdit = _mngr.Runners[index]; // Ссылка на объект в конкретной ячейке памяти

                // Передача бегуна и справочника
                using (var editForm = new Form3(runnerToEdit, _trainingTypes))
                {
                    // Перезапись значений после нажатия кнопки сохранения изменённых данных
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        runnerToEdit.Name = editForm.RunnerName;
                        runnerToEdit.City = editForm.RunnerCity;
                        runnerToEdit.Phone = editForm.RunnerPhone;

                        if (!string.IsNullOrWhiteSpace(editForm.GroupName))
                        {
                            runnerToEdit.RenameMainGroup(editForm.GroupName);
                        }

                        // Обновление информации в динамически настраиваемых кастомных полях через словарь
                        runnerToEdit.SetUsrField("ShoeSize", editForm.ShoeSize);
                        runnerToEdit.SetUsrField("MedicalNotes", editForm.MedicalNotes);
                        runnerToEdit.SetUsrField("PersonalBest", editForm.PersonalBest);
                        runnerToEdit.SetUsrField("TShirtSize", editForm.TShirtSize);
                        runnerToEdit.SetUsrField("EmergencyPhone", editForm.EmergencyPhone);

                        RefreshView();
                        MessageBox.Show("Данные обновлены!");
                    }
                }
            }
        }

        /// <summary>
        /// Открытие формы фильтрации
        /// </summary>
        public void OpenFilter()
        {
            using (var filterForm = new Form2(_mngr.DisplayFields.ToList(), _trainingTypes)) // Открытие полей фильтрации как редактируемого списка
            {
                if (filterForm.ShowDialog() == DialogResult.OK)
                {
                    var rules = filterForm.Rules;
                    // Возвращение членов клуба из репозитория, соответствующих всем правилам фильтрации
                    var filtered = _repository.All
                        .Where(runner => rules.All(rule => rule.IsMatch(runner)))
                        .ToList();

                    _mngr.SetRunners(filtered);
                    RefreshView();
                }
            }
        }

        public void SaveToDb()
        {
            foreach (var runner in _mngr.Runners)
            {
                _dbController.SaveRunner(runner);
            }
            MessageBox.Show("Все бегуны успешно сохранены в БД!");
        }

        public void LoadFromDb()
        {
            var loadedRunners = _dbController.LoadRunners();
            _repository.Clear(); // Защита от дубликатов за счёт удаления всех бегунов репозитория, находящихся в данный момент в ОП


            foreach (var runner in loadedRunners)
            {
                _repository.Add(runner);
            }
            _mngr.SetRunners(loadedRunners);
            RefreshView();
            MessageBox.Show($"Загружено {loadedRunners.Count} бегунов из БД!");
        }

        public void DeleteRunnerByIndex(int index)
        {
            if (index >= 0 && index < _mngr.Runners.Count)
            {
                var runnerToDelete = _mngr.Runners[index];

                // Запрос подтверждение
                var result = MessageBox.Show($"Вы уверены, что хотите удалить бегуна {runnerToDelete.Name}?",
                                             "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Удаление из БД (только бегуна, группа и тренировки остаются)
                    _dbController.DeleteRunner(runnerToDelete.Name, runnerToDelete.Phone);

                    // Удаление из памяти
                    _repository.Remove(runnerToDelete);

                    _mngr.SetRunners(_repository.All.ToList());
                    RefreshView();

                    MessageBox.Show("Бегун успешно удален!");
                }
            }
        }
    }
}