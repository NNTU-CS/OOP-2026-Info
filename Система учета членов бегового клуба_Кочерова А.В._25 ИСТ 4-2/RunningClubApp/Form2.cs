using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static RunningClubApp.FilterRule;

namespace RunningClubApp
{
    public partial class Form2 : Form
    {
        public List<FilterRule> Rules { get; private set; }

        private class FilterRow
        {
            public RunnerField Field;
            public CheckBox Check;
            public TextBox Text;
            public ComboBox Combo;
        }

        private List<FilterRow> _rows = new List<FilterRow>();
        private Panel panelFilters;

        // Выносим элементы фильтра тренировок на уровень класса
        private ComboBox cmbTrainType;
        private CheckBox chkHasTraining;
        private TrainingTypeCol _trainingTypes;

        public Form2(List<RunnerField> currentFields, TrainingTypeCol trainingTypes)
        {
            InitializeComponent();
            Rules = new List<FilterRule>();
            _trainingTypes = trainingTypes;

            // Создаем панель
            panelFilters = new Panel();
            panelFilters.Location = new Point(10, 10);
            panelFilters.Size = new Size(450, 300);
            panelFilters.AutoScroll = true;
            this.Controls.Add(panelFilters);

            int y = 10;

            // Генерируем поля для фильтрации по атрибутам
            foreach (var field in currentFields)
            {
                var chk = new CheckBox { Text = field.Name, Location = new Point(10, y), Width = 120 };
                var txt = new TextBox { Location = new Point(140, y), Width = 150 };
                var cmb = new ComboBox { Location = new Point(300, y), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };

                cmb.Items.AddRange(new object[] { "Содержит", "Равно", "Не содержит" });
                cmb.SelectedIndex = 0;

                panelFilters.Controls.Add(chk);
                panelFilters.Controls.Add(txt);
                panelFilters.Controls.Add(cmb);

                _rows.Add(new FilterRow { Field = field, Check = chk, Text = txt, Combo = cmb });
                y += 30;
            }

            // Блок фильтрации по тренировкам
            y += 20;
            var lblTraining = new Label { Text = "Тренировки:", Location = new Point(10, y), Width = 120 };

            cmbTrainType = new ComboBox { Location = new Point(140, y), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTrainType.Items.Add("Любая");

            // Заполняем из справочника!
            foreach (var type in _trainingTypes)
            {
                cmbTrainType.Items.Add(type.Name);
            }
            cmbTrainType.SelectedIndex = 0;

            chkHasTraining = new CheckBox { Text = "Есть тренировка", Location = new Point(300, y), Width = 150, Checked = false };

            panelFilters.Controls.Add(lblTraining);
            panelFilters.Controls.Add(cmbTrainType);
            panelFilters.Controls.Add(chkHasTraining);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Rules.Clear();

            // Правила по полям
            foreach (var row in _rows)
            {
                if (row.Check.Checked && !string.IsNullOrWhiteSpace(row.Text.Text))
                {
                    LogExpEnum op = LogExpEnum.Contains;
                    if (row.Combo.SelectedIndex == 1) op = LogExpEnum.EQ;
                    if (row.Combo.SelectedIndex == 2) op = LogExpEnum.NoContains;

                    var exp = LogExpFactory.Create(op, row.Text.Text.Trim());
                    Rules.Add(new FieldFilterRule(row.Field, exp));
                }
            }

            // Правило по тренировкам
            if (chkHasTraining.Checked)
            {
                string selectedType = cmbTrainType.SelectedItem.ToString();
                if (selectedType == "Любая")
                {
                    Rules.Add(new AnyTrainingRule());
                }
                else
                {
                    Rules.Add(new TrainingPresenceRule(new TrainingType(selectedType, ""), true));
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
        }
    }
}