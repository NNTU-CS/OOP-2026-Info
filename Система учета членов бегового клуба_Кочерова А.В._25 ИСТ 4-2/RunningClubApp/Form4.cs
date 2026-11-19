using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RunningClubApp
{
    public partial class Form4 : Form
    {
        public List<RunnerField> SelectedFields { get; private set; }

        public Form4(List<RunnerField> currentFields)
        {
            InitializeComponent();
            SelectedFields = new List<RunnerField>();

            var allFields = new List<RunnerField>
            {
                new NameField(),
                new CityField(),
                new PhoneField(),
                new GroupField(),
                new ShoeSizeField(),        
                new MedicalNotesField(),     
                new PersonalBestField(),     
                new TShirtSizeField(),       
                new EmergencyPhoneField()
            };

            foreach (var field in allFields)
            {
                bool isChecked = currentFields.Exists(f => f.GetType() == field.GetType());
                checkedListBoxFields.Items.Add(field, isChecked);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SelectedFields.Clear();

            foreach (var item in checkedListBoxFields.CheckedItems)
            {
                SelectedFields.Add((RunnerField)item);
            }

            if (SelectedFields.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один столбец!");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkedListBoxFields_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}