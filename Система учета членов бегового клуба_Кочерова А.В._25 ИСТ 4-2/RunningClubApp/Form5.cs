using System;
using System.Windows.Forms;

namespace RunningClubApp
{
    public partial class Form5 : Form
    {
        public TrainingType SelectedType { get; private set; }
        public DateTime BeginDt { get; private set; }
        public DateTime EndDt { get; private set; }
        public string DataInfo { get; private set; }
        public string Descr { get; private set; }

        public Form5(TrainingTypeCol trainingTypes)
        {
            InitializeComponent();

            // Очистка списка
            cmbType.Items.Clear();

            // Добавление объектов 
            foreach (var type in trainingTypes)
            {
                cmbType.Items.Add(type);
            }

            // Первый элемент выбран по умолчанию
            if (cmbType.Items.Count > 0)
                cmbType.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dtpEnd.Value < dtpBegin.Value)
            {
                MessageBox.Show("Дата окончания не может быть раньше начала!");
                return;
            }

            SelectedType = (TrainingType)cmbType.SelectedItem;
            BeginDt = dtpBegin.Value;
            EndDt = dtpEnd.Value;
            DataInfo = txtDataInfo.Text;
            Descr = txtDescr.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}