using System;
using System.Windows.Forms;

namespace RunningClubApp
{
    public partial class Form6 : Form
    {
        private Runner _runner;
        private TrainingTypeCol _trainingTypes;

        public Form6(Runner runner, TrainingTypeCol trainingTypes)
        {
            InitializeComponent();
            _runner = runner;
            _trainingTypes = trainingTypes;
            DrawTrainings();
        }

        private void DrawTrainings()
        {
            listViewTrainings.Items.Clear();
            listViewTrainings.Columns.Clear();
            listViewTrainings.Columns.Add("Тип", 100);
            listViewTrainings.Columns.Add("Начало", 100);
            listViewTrainings.Columns.Add("Конец", 100);
            listViewTrainings.Columns.Add("Инфо", 150);

            var trainings = _runner.GetMainGroup()?.Trainings;
            if (trainings == null) return;

            foreach (var t in trainings)
            {
                var item = new ListViewItem(t.Tpy.Name);
                item.SubItems.Add(t.BeginDt.ToShortDateString());
                item.SubItems.Add(t.EndDt.ToShortDateString());
                item.SubItems.Add(t.DataInfo);
                listViewTrainings.Items.Add(item);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Передаем справочник в форму 5!
            using (var addForm = new Form5(_trainingTypes))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    var newTraining = new Training(addForm.BeginDt, addForm.SelectedType, addForm.DataInfo, addForm.Descr, addForm.EndDt);
                    _runner.AddTraining(newTraining);
                    DrawTrainings();
                }
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }
    }
}